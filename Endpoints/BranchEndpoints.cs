using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MahlukHidup.Backend.Data;
using MahlukHidup.Backend.DTOs;
using MahlukHidup.Backend.Models;

namespace MahlukHidup.Backend.Endpoints;

public static class BranchEndpoints
{
    public static void MapBranchEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/branches").RequireAuthorization().WithOpenApi();

        // GET: /api/branches - Get all branches
        group.MapGet("/", async (AppDbContext context) =>
        {
            var branches = await context.Branches
                .Include(b => b.Company)
                .OrderByDescending(b => b.CreateDate)
                .ToListAsync();

            var result = branches.Select(b => new BranchDto
            {
                Id = b.Id,
                CompanyId = b.CompanyId,
                CompanyName = b.Company != null ? b.Company.Name : "Unknown",
                Name = b.Name,
                Code = b.Code,
                Address = b.Address,
                Phone = b.Phone,
                Email = b.Email,
                Latitude = b.Latitude,
                Longitude = b.Longitude,
                IsActive = b.IsActive,
                CreateDate = b.CreateDate
            });

            return Results.Ok(result);
        })
        .WithName("GetBranches");

        // GET: /api/branches/company/{companyId} - Get branches by company
        group.MapGet("/company/{companyId:guid}", async (Guid companyId, AppDbContext context) =>
        {
            var branches = await context.Branches
                .Include(b => b.Company)
                .Where(b => b.CompanyId == companyId)
                .OrderByDescending(b => b.CreateDate)
                .ToListAsync();

            var result = branches.Select(b => new BranchDto
            {
                Id = b.Id,
                CompanyId = b.CompanyId,
                CompanyName = b.Company != null ? b.Company.Name : "Unknown",
                Name = b.Name,
                Code = b.Code,
                Address = b.Address,
                Phone = b.Phone,
                Email = b.Email,
                Latitude = b.Latitude,
                Longitude = b.Longitude,
                IsActive = b.IsActive,
                CreateDate = b.CreateDate
            });

            return Results.Ok(result);
        })
        .WithName("GetBranchesByCompany");

        // GET: /api/branches/{id} - Get branch by ID
        group.MapGet("/{id:guid}", async (Guid id, AppDbContext context) =>
        {
            var branch = await context.Branches
                .Include(b => b.Company)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (branch == null) return Results.NotFound("Branch not found.");

            var result = new BranchDto
            {
                Id = branch.Id,
                CompanyId = branch.CompanyId,
                CompanyName = branch.Company != null ? branch.Company.Name : "Unknown",
                Name = branch.Name,
                Code = branch.Code,
                Address = branch.Address,
                Phone = branch.Phone,
                Email = branch.Email,
                Latitude = branch.Latitude,
                Longitude = branch.Longitude,
                IsActive = branch.IsActive,
                CreateDate = branch.CreateDate
            };

            return Results.Ok(result);
        })
        .WithName("GetBranchById");

        // POST: /api/branches - Create new branch
        group.MapPost("/", async ([FromBody] BranchCreateDto dto, AppDbContext context) =>
        {
            // Check if company exists
            var company = await context.Companies.FindAsync(dto.CompanyId);
            if (company == null) return Results.BadRequest("Company not found.");

            // Check if code already exists
            if (await context.Branches.AnyAsync(b => b.Code == dto.Code))
                return Results.BadRequest("Branch code already exists.");

            var branch = new Branch
            {
                CompanyId = dto.CompanyId,
                Name = dto.Name,
                Code = dto.Code,
                Address = dto.Address,
                Phone = dto.Phone,
                Email = dto.Email,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                IsActive = true
            };

            context.Branches.Add(branch);
            await context.SaveChangesAsync();

            return Results.Created($"/api/branches/{branch.Id}", new BranchDto
            {
                Id = branch.Id,
                CompanyId = branch.CompanyId,
                CompanyName = company.Name,
                Name = branch.Name,
                Code = branch.Code,
                Address = branch.Address,
                Phone = branch.Phone,
                Email = branch.Email,
                Latitude = branch.Latitude,
                Longitude = branch.Longitude,
                IsActive = branch.IsActive,
                CreateDate = branch.CreateDate
            });
        })
        .WithName("CreateBranch");

        // PUT: /api/branches/{id} - Update branch
        group.MapPut("/{id:guid}", async (Guid id, [FromBody] BranchUpdateDto dto, AppDbContext context) =>
        {
            var branch = await context.Branches.FindAsync(id);
            if (branch == null) return Results.NotFound("Branch not found.");

            if (dto.Name != null) branch.Name = dto.Name;
            if (dto.Address != null) branch.Address = dto.Address;
            if (dto.Phone != null) branch.Phone = dto.Phone;
            if (dto.Email != null) branch.Email = dto.Email;
            if (dto.Latitude.HasValue) branch.Latitude = dto.Latitude.Value;
            if (dto.Longitude.HasValue) branch.Longitude = dto.Longitude.Value;
            if (dto.IsActive.HasValue) branch.IsActive = dto.IsActive.Value;
            
            branch.UpdateDate = DateTime.UtcNow;

            await context.SaveChangesAsync();

            var company = await context.Companies.FindAsync(branch.CompanyId);

            return Results.Ok(new BranchDto
            {
                Id = branch.Id,
                CompanyId = branch.CompanyId,
                CompanyName = company != null ? company.Name : "Unknown",
                Name = branch.Name,
                Code = branch.Code,
                Address = branch.Address,
                Phone = branch.Phone,
                Email = branch.Email,
                Latitude = branch.Latitude,
                Longitude = branch.Longitude,
                IsActive = branch.IsActive,
                CreateDate = branch.CreateDate
            });
        })
        .WithName("UpdateBranch");

        // DELETE: /api/branches/{id} - Delete branch (soft delete)
        group.MapDelete("/{id:guid}", async (Guid id, AppDbContext context) =>
        {
            var branch = await context.Branches.FindAsync(id);
            if (branch == null) return Results.NotFound("Branch not found.");

            branch.IsActive = false;
            branch.UpdateDate = DateTime.UtcNow;

            await context.SaveChangesAsync();

            return Results.Ok("Branch deactivated.");
        })
        .WithName("DeleteBranch");
    }
}
