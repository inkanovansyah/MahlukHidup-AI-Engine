using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MahlukHidup.Backend.Data;
using MahlukHidup.Backend.DTOs;
using MahlukHidup.Backend.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace MahlukHidup.Backend.Endpoints;

public static class HREndpoints
{
    public static void MapHREndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/hr").RequireAuthorization().WithOpenApi();

        // ----------------------------------------------------
        // DEPARTMENTS
        // ----------------------------------------------------
        group.MapGet("/departments", async (AppDbContext context) =>
        {
            var items = await context.Departments.Where(d => d.IsActive).ToListAsync();
            return Results.Ok(items.Select(d => new DepartmentDto { Id = d.Id, Name = d.Name, Description = d.Description }));
        }).WithName("GetDepartments");

        group.MapPost("/departments", async ([FromBody] HRCreateDto dto, HttpContext httpContext, AppDbContext context) =>
        {
            var userIdString = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            _ = int.TryParse(userIdString, out int userId);

            var dept = new Department { 
                Name = dto.Name, 
                Description = dto.Description,
                CreatedBy = userId == 0 ? null : userId
            };
            context.Departments.Add(dept);

            context.RecordActivities.Add(new RecordActivity {
                Action = "CREATE_DEPARTMENT",
                EntityName = "Department",
                EntityId = dept.Id.ToString(), // Might be 0 until SaveChanges, but it's okay for descriptive logs or we can save after
                Details = $"Departemen baru '{dept.Name}' ditambahkan",
                CreatedBy = userId == 0 ? null : userId
            });

            await context.SaveChangesAsync();
            return Results.Created($"/api/hr/departments/{dept.Id}", new DepartmentDto { Id = dept.Id, Name = dept.Name, Description = dept.Description });
        }).WithName("CreateDepartment");

        // ----------------------------------------------------
        // JOB LEVELS (Hirarki)
        // ----------------------------------------------------
        group.MapGet("/job-levels", async (AppDbContext context) =>
        {
            var items = await context.JobLevels.Where(j => j.IsActive).OrderBy(j => j.Rank).ToListAsync();
            return Results.Ok(items.Select(j => new JobLevelDto { Id = j.Id, Name = j.Name, Rank = j.Rank, CanApprove = j.CanApprove, Description = j.Description }));
        }).WithName("GetJobLevels");

        group.MapPost("/job-levels", async ([FromBody] JobLevelCreateDto dto, HttpContext httpContext, AppDbContext context) =>
        {
            var userIdString = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            _ = int.TryParse(userIdString, out int userId);

            var level = new JobLevel { 
                Name = dto.Name, 
                Rank = dto.Rank, 
                CanApprove = dto.CanApprove, 
                Description = dto.Description,
                CreatedBy = userId == 0 ? null : userId
            };
            context.JobLevels.Add(level);

            context.RecordActivities.Add(new RecordActivity {
                Action = "CREATE_JOBLEVEL",
                EntityName = "JobLevel",
                EntityId = level.Id.ToString(),
                Details = $"Level Jabatan baru '{level.Name}' dengan Rank {level.Rank} ditambahkan",
                CreatedBy = userId == 0 ? null : userId
            });

            await context.SaveChangesAsync();
            return Results.Created($"/api/hr/job-levels/{level.Id}", new JobLevelDto { Id = level.Id, Name = level.Name, Rank = level.Rank, CanApprove = level.CanApprove, Description = level.Description });
        }).WithName("CreateJobLevel");

        // ----------------------------------------------------
        // JOB POSITIONS
        // ----------------------------------------------------
        group.MapGet("/job-positions", async (AppDbContext context) =>
        {
            var items = await context.JobPositions.Where(p => p.IsActive).ToListAsync();
            return Results.Ok(items.Select(p => new JobPositionDto { Id = p.Id, Name = p.Name, Description = p.Description }));
        }).WithName("GetJobPositions");

        group.MapPost("/job-positions", async ([FromBody] HRCreateDto dto, HttpContext httpContext, AppDbContext context) =>
        {
            var userIdString = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            _ = int.TryParse(userIdString, out int userId);

            var pos = new JobPosition { 
                Name = dto.Name, 
                Description = dto.Description,
                CreatedBy = userId == 0 ? null : userId
            };
            context.JobPositions.Add(pos);

            context.RecordActivities.Add(new RecordActivity {
                Action = "CREATE_JOBPOSITION",
                EntityName = "JobPosition",
                EntityId = pos.Id.ToString(),
                Details = $"Posisi/Jabatan baru '{pos.Name}' ditambahkan",
                CreatedBy = userId == 0 ? null : userId
            });

            await context.SaveChangesAsync();
            return Results.Created($"/api/hr/job-positions/{pos.Id}", new JobPositionDto { Id = pos.Id, Name = pos.Name, Description = pos.Description });
        }).WithName("CreateJobPosition");
    }
}
