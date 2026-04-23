using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MahlukHidup.Backend.Data;
using MahlukHidup.Backend.DTOs;
using MahlukHidup.Backend.Models;

namespace MahlukHidup.Backend.Endpoints;

public static class CompanyEndpoints
{
    public static void MapCompanyEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/companies").RequireAuthorization().WithOpenApi();

        // GET: /api/companies - Get all companies with branches
        group.MapGet("/", async (AppDbContext context) =>
        {
            var companies = await context.Companies
                .Include(c => c.Branches)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            var result = companies.Select(c => new CompanyDto
            {
                Id = c.Id,
                Name = c.Name,
                Code = c.Code,
                Description = c.Description,
                Address = c.Address,
                Phone = c.Phone,
                Email = c.Email,
                LogoUrl = c.LogoUrl,
                IsActive = c.IsActive,
                CreatedAt = c.CreatedAt,
                Branches = c.Branches.Select(b => new BranchDto
                {
                    Id = b.Id,
                    CompanyId = b.CompanyId,
                    CompanyName = c.Name,
                    Name = b.Name,
                    Code = b.Code,
                    Address = b.Address,
                    Phone = b.Phone,
                    Email = b.Email,
                    Latitude = b.Latitude,
                    Longitude = b.Longitude,
                    IsActive = b.IsActive,
                    IsMaster = b.IsMaster,
                    CreatedAt = b.CreatedAt
                }).ToList()
            });

            return Results.Ok(result);
        })
        .WithName("GetCompanies");

        // GET: /api/companies/{id} - Get company by ID
        group.MapGet("/{id:guid}", async (Guid id, AppDbContext context) =>
        {
            var company = await context.Companies
                .Include(c => c.Branches)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (company == null) return Results.NotFound("Company not found.");

            var result = new CompanyDto
            {
                Id = company.Id,
                Name = company.Name,
                Code = company.Code,
                Description = company.Description,
                Address = company.Address,
                Phone = company.Phone,
                Email = company.Email,
                LogoUrl = company.LogoUrl,
                IsActive = company.IsActive,
                CreatedAt = company.CreatedAt,
                Branches = company.Branches.Select(b => new BranchDto
                {
                    Id = b.Id,
                    CompanyId = b.CompanyId,
                    CompanyName = company.Name,
                    Name = b.Name,
                    Code = b.Code,
                    Address = b.Address,
                    Phone = b.Phone,
                    Email = b.Email,
                    Latitude = b.Latitude,
                    Longitude = b.Longitude,
                    IsActive = b.IsActive,
                    IsMaster = b.IsMaster,
                    CreatedAt = b.CreatedAt
                }).ToList()
            };

            return Results.Ok(result);
        })
        .WithName("GetCompanyById");

        // POST: /api/companies - Create new company
        group.MapPost("/", async ([FromBody] CompanyCreateDto dto, AppDbContext context) =>
        {
            // Check if code already exists
            if (await context.Companies.AnyAsync(c => c.Code == dto.Code))
                return Results.BadRequest("Company code already exists.");

            var company = new Company
            {
                Name = dto.Name,
                Code = dto.Code,
                Description = dto.Description,
                Address = dto.Address,
                Phone = dto.Phone,
                Email = dto.Email,
                LogoUrl = dto.LogoUrl,
                IsActive = true
            };

            context.Companies.Add(company);
            await context.SaveChangesAsync();

            return Results.Created($"/api/companies/{company.Id}", new CompanyDto
            {
                Id = company.Id,
                Name = company.Name,
                Code = company.Code,
                Description = company.Description,
                Address = company.Address,
                Phone = company.Phone,
                Email = company.Email,
                LogoUrl = company.LogoUrl,
                IsActive = company.IsActive,
                CreatedAt = company.CreatedAt,
                Branches = new List<BranchDto>()
            });
        })
        .WithName("CreateCompany");

        // PUT: /api/companies/{id} - Update company
        group.MapPut("/{id:guid}", async (Guid id, [FromBody] CompanyUpdateDto dto, AppDbContext context) =>
        {
            var company = await context.Companies.FindAsync(id);
            if (company == null) return Results.NotFound("Company not found.");

            if (dto.Name != null) company.Name = dto.Name;
            if (dto.Description != null) company.Description = dto.Description;
            if (dto.Address != null) company.Address = dto.Address;
            if (dto.Phone != null) company.Phone = dto.Phone;
            if (dto.Email != null) company.Email = dto.Email;
            if (dto.LogoUrl != null) company.LogoUrl = dto.LogoUrl;
            if (dto.IsActive.HasValue) company.IsActive = dto.IsActive.Value;
            
            company.UpdatedAt = DateTime.UtcNow;

            await context.SaveChangesAsync();

            return Results.Ok(new CompanyDto
            {
                Id = company.Id,
                Name = company.Name,
                Code = company.Code,
                Description = company.Description,
                Address = company.Address,
                Phone = company.Phone,
                Email = company.Email,
                LogoUrl = company.LogoUrl,
                IsActive = company.IsActive,
                CreatedAt = company.CreatedAt
            });
        })
        .WithName("UpdateCompany");

        // DELETE: /api/companies/{id} - Delete company (soft delete)
        group.MapDelete("/{id:guid}", async (Guid id, AppDbContext context) =>
        {
            var company = await context.Companies
                .Include(c => c.Branches)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (company == null) return Results.NotFound("Company not found.");

            // Soft delete: deactivate company and all branches
            company.IsActive = false;
            company.UpdatedAt = DateTime.UtcNow;
            
            foreach (var branch in company.Branches)
            {
                branch.IsActive = false;
                branch.UpdatedAt = DateTime.UtcNow;
            }

            await context.SaveChangesAsync();

            return Results.Ok("Company and its branches deactivated.");
        })
        .WithName("DeleteCompany");

        // ----------------------------------------------------
        // BRANCHES MANAGEMENT (Nested under Company)
        // ----------------------------------------------------

        // POST: /api/companies/{companyId}/branches - Create new branch
        group.MapPost("/{companyId:guid}/branches", async (Guid companyId, [FromBody] BranchCreateDto dto, AppDbContext context) =>
        {
            var company = await context.Companies.FindAsync(companyId);
            if (company == null) return Results.NotFound("Company not found.");

            // Check if code already exists
            if (await context.Branches.AnyAsync(b => b.Code == dto.Code))
                return Results.BadRequest("Branch code already exists.");

            // Only one master branch per company
            if (dto.IsMaster && await context.Branches.AnyAsync(b => b.CompanyId == companyId && b.IsMaster))
                return Results.BadRequest("A master branch (PT Utama) already exists for this company.");

            var branch = new Branch
            {
                CompanyId = companyId,
                Name = dto.Name,
                Code = dto.Code,
                Address = dto.Address,
                Phone = dto.Phone,
                Email = dto.Email,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                IsMaster = dto.IsMaster,
                IsActive = true
            };

            context.Branches.Add(branch);
            await context.SaveChangesAsync();

            return Results.Created($"/api/companies/{companyId}/branches/{branch.Id}", new BranchDto
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
                IsMaster = branch.IsMaster,
                IsActive = branch.IsActive,
                CreatedAt = branch.CreatedAt
            });
        })
        .WithName("CreateCompanyBranch");
    }
}
