using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MahlukHidup.Backend.Data;
using MahlukHidup.Backend.DTOs;
using MahlukHidup.Backend.Models;
using System.Security.Claims;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MahlukHidup.Backend.Endpoints;

public static class MaterialRequestEndpoints
{
    public static void MapMaterialRequestEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/requests").RequireAuthorization().WithOpenApi();

        // 1. GET ALL REQUESTS (Filter by Type)
        group.MapGet("/", async ([FromQuery] string? type, AppDbContext context) =>
        {
            var query = context.MaterialRequests
                .Include(m => m.Requester)
                .Include(m => m.Department)
                .Include(m => m.Branch)
                .Include(m => m.Items)
                .Include(m => m.ApprovalRoutes)
                    .ThenInclude(a => a.ApproverUser)
                .Where(m => m.IsActive);

            if (!string.IsNullOrEmpty(type) && Enum.TryParse<RequestType>(type, true, out var parsedType))
            {
                query = query.Where(m => m.Type == parsedType);
            }

            var requests = await query.OrderByDescending(m => m.CreatedAt).ToListAsync();

            var result = requests.Select(MapToDto);
            return Results.Ok(result);
        }).WithName("GetMaterialRequests");

        // 2. CREATE REQUEST
        group.MapPost("/", async ([FromBody] CreateMaterialRequestDto dto, HttpContext httpContext, AppDbContext context) =>
        {
            var userIdString = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
                return Results.Unauthorized();

            // Ambil Info Pemohon
            var requester = await context.Users
                .Include(u => u.Department)
                .FirstOrDefaultAsync(u => u.Id == userId);
                
            if (requester == null) return Results.NotFound("User tidak ditemukan");
            if (requester.DepartmentId == null) return Results.BadRequest("User belum ditugaskan ke Departemen apapun.");

            // Validasi Tipe
            if (!Enum.TryParse<RequestType>(dto.Type, true, out var requestType))
                return Results.BadRequest("Tipe request tidak valid. Gunakan 'Saprotan' atau 'Asset'.");

            if (dto.Items == null || dto.Items.Count == 0)
                return Results.BadRequest("Harus ada minimal 1 barang yang diajukan.");

            var request = new MaterialRequest
            {
                RequestNumber = $"REQ-{requestType.ToString().ToUpper().Substring(0, 3)}-{DateTime.UtcNow:yyyyMMdd}-{new Random().Next(1000,9999)}",
                Type = requestType,
                RequesterId = requester.Id,
                DepartmentId = requester.DepartmentId.Value,
                BranchId = requester.BranchId, // Set Branch pengaju
                Justification = dto.Justification,
                Status = RequestStatus.Pending,
                CreatedBy = requester.Id,
                TotalEstimatedPrice = dto.Items.Sum(i => i.Quantity * i.EstimatedUnitPrice)
            };

            foreach (var item in dto.Items)
            {
                request.Items.Add(new MaterialRequestItem
                {
                    ItemName = item.ItemName,
                    Quantity = item.Quantity,
                    UnitOfMeasure = item.UnitOfMeasure,
                    EstimatedUnitPrice = item.EstimatedUnitPrice
                });
            }

            // --- Logika DYNAMIC APPROVAL ROUTING ---
            var routes = await GenerateApprovalRoutes(requester, request.TotalEstimatedPrice, context);
            if(routes.Count == 0) {
                // Tidak ada atasan? Langsung Approved. (Bisa disesuaikan dengan rule perusahaan)
                request.Status = RequestStatus.Approved;
            } else {
                foreach (var route in routes)
                {
                    request.ApprovalRoutes.Add(route);
                }
            }

            context.MaterialRequests.Add(request);

            // Record Activity
            context.RecordActivities.Add(new RecordActivity {
                Action = "CREATE_REQUEST",
                EntityName = "MaterialRequest",
                EntityId = request.Id.ToString(),
                Details = $"Pengajuan {requestType.ToString()} oleh {requester.Name} senilai {request.TotalEstimatedPrice}",
                CreatedBy = userId
            });

            await context.SaveChangesAsync();

            return Results.Ok(MapToDto(request));
        }).WithName("CreateMaterialRequest");

        // 3. APPROVAL ACTION (Approve or Reject)
        group.MapPost("/{id:guid}/approve", async (Guid id, [FromBody] ApprovalActionDto dto, HttpContext httpContext, AppDbContext context) => 
        {
            var userIdString = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
                return Results.Unauthorized();

            var request = await context.MaterialRequests
                .Include(r => r.ApprovalRoutes)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (request == null) return Results.NotFound("Request tidak ditemukan.");

            // Cari tahapan atasan untuk user yang sedang login
            var currentStep = request.ApprovalRoutes.FirstOrDefault(a => a.IsCurrentStep && a.ApproverUserId == userId);

            if (currentStep == null)
            {
                return Results.BadRequest("Bukan giliran Anda untuk proses approval ini.");
            }

            if (dto.Action == "Approve")
            {
                currentStep.Status = ApprovalStatus.Approved;
                currentStep.IsCurrentStep = false;
                currentStep.Notes = dto.Notes;
                currentStep.ActionTakenAt = DateTime.UtcNow;

                // Cari Next Step
                var nextStep = request.ApprovalRoutes.FirstOrDefault(a => a.StepOrder == currentStep.StepOrder + 1);
                if (nextStep != null)
                {
                    nextStep.IsCurrentStep = true;
                    // Status secara keseluruhan masih Pending
                }
                else
                {
                    // Tidak ada step lagi = Selesai semua
                    request.Status = RequestStatus.Approved;
                }
            }
            else if (dto.Action == "Reject")
            {
                currentStep.Status = ApprovalStatus.Rejected;
                currentStep.IsCurrentStep = false;
                currentStep.Notes = dto.Notes;
                currentStep.ActionTakenAt = DateTime.UtcNow;
                
                request.Status = RequestStatus.Rejected;
            }
            else 
            {
                return Results.BadRequest("Action harus 'Approve' atau 'Reject'");
            }

            // Record Activity
            context.RecordActivities.Add(new RecordActivity {
                Action = $"ACTION_{dto.Action.ToUpper()}",
                EntityName = "ApprovalRoute",
                EntityId = currentStep.Id.ToString(),
                Details = $"Atasan ID {userId} menanggapi pengajuan: {dto.Action}. Catatan: {dto.Notes}",
                CreatedBy = userId
            });

            await context.SaveChangesAsync();

            return Results.Ok(new { message = $"Berhasil di-{dto.Action}." });
        }).WithName("ApproveMaterialRequest");
    }

    // --- Private Helper ---
    private static async Task<List<ApprovalRoute>> GenerateApprovalRoutes(User requester, decimal totalAmount, AppDbContext context)
    {
        var routes = new List<ApprovalRoute>();
        int step = 1;
        var currentApprover = await context.Users
            .Include(u => u.JobLevel)
            .FirstOrDefaultAsync(u => u.Id == requester.ReportsToUserId && u.BranchId == requester.BranchId);

        while (currentApprover != null)
        {
            // Tambahkan rute
            routes.Add(new ApprovalRoute
            {
                ApproverUserId = currentApprover.Id,
                StepOrder = step,
                IsCurrentStep = (step == 1), // Step 1 langsung aktif
                Status = ApprovalStatus.Pending
            });

            // LOGIKA LIMIT: Misal > 50 Juta butuh level CEO (> Rank 80)
            if (totalAmount <= 5000000 && currentApprover.JobLevel != null && currentApprover.JobLevel.Rank >= 50)
            {
                // Kalau cuma 5 Juta, disetujui Manager(Rank 50) sudah cukup. Stop looping.
                break; 
            }
            
            if (totalAmount > 5000000 && currentApprover.JobLevel != null && currentApprover.JobLevel.Rank >= 80)
            {
                 // Udah nyampe Direktur/CEO. Stop.
                 break;
            }

            step++;
            currentApprover = await context.Users
                .Include(u => u.JobLevel)
                .FirstOrDefaultAsync(u => u.Id == currentApprover.ReportsToUserId && u.BranchId == requester.BranchId);

             // Fallback to prevent infinite loop just in case
             if (step > 10) break;
        }

        return routes;
    }

    private static MaterialRequestDto MapToDto(MaterialRequest m)
    {
        return new MaterialRequestDto
        {
            Id = m.Id,
            RequestNumber = m.RequestNumber,
            Type = m.Type.ToString(),
            RequesterId = m.RequesterId,
            RequesterName = m.Requester?.Name ?? "Unknown",
            DepartmentId = m.DepartmentId,
            DepartmentName = m.Department?.Name ?? "Unknown",
            BranchId = m.BranchId,
            BranchName = m.Branch?.Name ?? "Unknown",
            Justification = m.Justification,
            TotalEstimatedPrice = m.TotalEstimatedPrice,
            Status = m.Status.ToString(),
            CreatedAt = m.CreatedAt,
            Items = m.Items.Select(i => new MaterialRequestItemDto
            {
                Id = i.Id,
                ItemName = i.ItemName,
                Quantity = i.Quantity,
                UnitOfMeasure = i.UnitOfMeasure,
                EstimatedUnitPrice = i.EstimatedUnitPrice,
                TotalPrice = i.TotalPrice
            }).ToList(),
            ApprovalRoutes = m.ApprovalRoutes.OrderBy(a => a.StepOrder).Select(a => new ApprovalRouteDto
            {
                Id = a.Id,
                ApproverUserId = a.ApproverUserId,
                ApproverName = a.ApproverUser?.Name ?? "Unknown",
                StepOrder = a.StepOrder,
                IsCurrentStep = a.IsCurrentStep,
                Status = a.Status.ToString(),
                Notes = a.Notes,
                ActionTakenAt = a.ActionTakenAt
            }).ToList()
        };
    }
}
