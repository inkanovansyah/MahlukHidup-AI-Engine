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
                .OrderByDescending(c => c.CreateDate)
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
                CreateDate = c.CreateDate,
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
                    CreateDate = b.CreateDate
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
                CreateDate = company.CreateDate,
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
                    CreateDate = b.CreateDate
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
                CreateDate = company.CreateDate,
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
            
            company.UpdateDate = DateTime.UtcNow;

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
                CreateDate = company.CreateDate
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
            company.UpdateDate = DateTime.UtcNow;
            
            foreach (var branch in company.Branches)
            {
                branch.IsActive = false;
                branch.UpdateDate = DateTime.UtcNow;
            }

            await context.SaveChangesAsync();

            return Results.Ok("Company and its branches deactivated.");
        })
        .WithName("DeleteCompany");
    }
}
