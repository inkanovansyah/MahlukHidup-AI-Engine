using MahlukHidup.Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace MahlukHidup.Backend.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        // Seed JobLevels (Hirarki Jabatan)
        if (!await context.JobLevels.AnyAsync())
        {
            context.JobLevels.AddRange(
                new JobLevel
                {
                    Name = "Staff / Operator",
                    Rank = 10,
                    CanApprove = false,
                    Description = "Pekerja lapangan",
                    IsActive = true,
                    IsDeleted = false,
                    CreatedAt = DateTime.UtcNow
                },
                new JobLevel
                {
                    Name = "Kepala / Manager",
                    Rank = 50,
                    CanApprove = true,
                    Description = "Atasan pemutus",
                    IsActive = true,
                    IsDeleted = false,
                    CreatedAt = DateTime.UtcNow
                },
                new JobLevel
                {
                    Name = "SEO",
                    Rank = 100,
                    CanApprove = true,
                    Description = "Atasan pemutus tertinggi",
                    IsActive = true,
                    IsDeleted = false,
                    CreatedAt = DateTime.UtcNow
                }
            );
            await context.SaveChangesAsync();
        }

        // Seed Departments
        if (!await context.Departments.AnyAsync())
        {
            context.Departments.AddRange(
                new Department { Name = "Operasional Lapangan", Description = "Tim yang bertugas langsung di lapangan", IsActive = true, IsDeleted = false, CreatedAt = DateTime.UtcNow },
                new Department { Name = "Manajemen", Description = "Tim pengelola dan pengambil keputusan", IsActive = true, IsDeleted = false, CreatedAt = DateTime.UtcNow },
                new Department { Name = "Teknologi & IT", Description = "Tim pengembang dan infrastruktur teknologi", IsActive = true, IsDeleted = false, CreatedAt = DateTime.UtcNow },
                new Department { Name = "Agronom & Riset", Description = "Tim peneliti pertanian dan riset", IsActive = true, IsDeleted = false, CreatedAt = DateTime.UtcNow }
            );
            await context.SaveChangesAsync();
        }

        // Seed JobPositions
        if (!await context.JobPositions.AnyAsync())
        {
            context.JobPositions.AddRange(
                new JobPosition { Name = "Operator Lapangan", Description = "Bertugas operasional langsung", IsActive = true, IsDeleted = false, CreatedAt = DateTime.UtcNow },
                new JobPosition { Name = "Teknisi Drone", Description = "Mengoperasikan dan merawat drone", IsActive = true, IsDeleted = false, CreatedAt = DateTime.UtcNow },
                new JobPosition { Name = "Agronom", Description = "Ahli pertanian dan tanaman", IsActive = true, IsDeleted = false, CreatedAt = DateTime.UtcNow },
                new JobPosition { Name = "Manager Operasional", Description = "Kepala tim operasional", IsActive = true, IsDeleted = false, CreatedAt = DateTime.UtcNow },
                new JobPosition { Name = "Direktur / CEO", Description = "Pimpinan tertinggi perusahaan", IsActive = true, IsDeleted = false, CreatedAt = DateTime.UtcNow }
            );
            await context.SaveChangesAsync();
        }
    }
}
