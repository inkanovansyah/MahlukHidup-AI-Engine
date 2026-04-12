using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MahlukHidup.Backend.Data;
using MahlukHidup.Backend.DTOs;
using MahlukHidup.Backend.Models;
using MahlukHidup.Backend.Services;

namespace MahlukHidup.Backend.Endpoints;

public static class DiseasePhotoEndpoints
{
    public static void MapDiseasePhotoEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/disease-photos")
                      .WithTags("Disease Photos")
                      .RequireAuthorization();

        // Upload disease photo
        group.MapPost("/upload", async (
            IFormFile file,
            [FromForm] Guid agriculturalSectorId,
            [FromForm] Guid companyId,
            [FromForm] string? description,
            [FromForm] DateTime? capturedAt,
            AppDbContext context,
            IPhotoUploadService uploadService) =>
        {
            try
            {
                // Verify company exists and get name
                var company = await context.Companies.FindAsync(companyId);
                if (company == null)
                {
                    return Results.NotFound(new { message = "Company tidak ditemukan." });
                }

                // Verify agricultural sector exists
                var sector = await context.AgriculturalSectors.FindAsync(agriculturalSectorId);
                if (sector == null)
                {
                    return Results.NotFound(new { message = "Sector pertanian tidak ditemukan." });
                }

                // Upload file
                var photoUrl = await uploadService.UploadDiseasePhotoAsync(file, companyId, company.Name);

                // Create disease photo record
                var diseasePhoto = new DiseasePhoto
                {
                    AgriculturalSectorId = agriculturalSectorId,
                    CompanyId = companyId,
                    PhotoUrl = photoUrl,
                    FileName = file.FileName,
                    FileSize = file.Length,
                    Description = description,
                    CapturedAt = capturedAt ?? DateTime.UtcNow
                };

                context.DiseasePhotos.Add(diseasePhoto);
                await context.SaveChangesAsync();

                var response = new DiseasePhotoResponseDto
                {
                    Id = diseasePhoto.Id,
                    AgriculturalSectorId = diseasePhoto.AgriculturalSectorId,
                    CompanyId = diseasePhoto.CompanyId,
                    PhotoUrl = diseasePhoto.PhotoUrl,
                    FileName = diseasePhoto.FileName,
                    FileSize = diseasePhoto.FileSize,
                    Description = diseasePhoto.Description,
                    CapturedAt = diseasePhoto.CapturedAt,
                    CreatedAt = diseasePhoto.CreatedAt
                };

                return Results.Ok(new { message = "Foto berhasil diupload.", data = response });
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return Results.Problem($"Terjadi kesalahan: {ex.Message}");
            }
        })
        .DisableAntiforgery()
        .Accepts<IFormFile>("multipart/form-data")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Upload foto penyakit tanaman",
            Description = "Upload foto penyakit tanaman dengan struktur folder dinamis berdasarkan company dan tanggal"
        });

        // Soft delete disease photo (POST instead of DELETE)
        group.MapPost("/delete", async (
            DiseasePhotoDeleteDto dto,
            AppDbContext context,
            IPhotoUploadService uploadService) =>
        {
            var photo = await context.DiseasePhotos.FindAsync(dto.Id);
            if (photo == null)
            {
                return Results.NotFound(new { message = "Foto tidak ditemukan." });
            }

            // Delete file from storage
            await uploadService.DeletePhotoAsync(photo.PhotoUrl);

            // Soft delete - set IsActive to false
            photo.IsActive = false;
            photo.IsDeleted = true;
            photo.UpdatedAt = DateTime.UtcNow;

            await context.SaveChangesAsync();

            return Results.Ok(new { message = "Foto berhasil dihapus." });
        })
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Hapus foto penyakit (soft delete)",
            Description = "Set status foto menjadi tidak aktif (IsActive = false, IsDeleted = true)"
        });
    }
}
