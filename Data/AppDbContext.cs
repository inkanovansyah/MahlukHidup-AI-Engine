using Microsoft.EntityFrameworkCore;
using MahlukHidup.Backend.Models;

namespace MahlukHidup.Backend.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    
    // HR / Organization Entities
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<JobLevel> JobLevels => Set<JobLevel>();
    public DbSet<JobPosition> JobPositions => Set<JobPosition>();

    // Request & Workflow Entities
    public DbSet<MaterialRequest> MaterialRequests => Set<MaterialRequest>();
    public DbSet<MaterialRequestItem> MaterialRequestItems => Set<MaterialRequestItem>();
    public DbSet<ApprovalRoute> ApprovalRoutes => Set<ApprovalRoute>();
    
    // IoT Dashboard Entities
    public DbSet<AgriculturalSector> AgriculturalSectors => Set<AgriculturalSector>();
    public DbSet<SectorSoil> SectorSoils => Set<SectorSoil>();
    public DbSet<SectorClimate> SectorClimates => Set<SectorClimate>();
    public DbSet<SectorFinance> SectorFinances => Set<SectorFinance>();
    public DbSet<SectorPhoto> SectorPhotos => Set<SectorPhoto>();
    
    public DbSet<DroneDevice> DroneDevices => Set<DroneDevice>();
    public DbSet<DroneTask> DroneTasks => Set<DroneTask>();
    public DbSet<MonthlyRevenue> MonthlyRevenues => Set<MonthlyRevenue>();

    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Branch> Branches => Set<Branch>();

    public DbSet<RecordActivity> RecordActivities => Set<RecordActivity>();

    public DbSet<DiseasePhoto> DiseasePhotos => Set<DiseasePhoto>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Konfigurasi tabel User
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.Name).IsRequired().HasMaxLength(100);
            entity.Property(u => u.Email).IsRequired().HasMaxLength(150);

            entity.HasOne(u => u.Department).WithMany().HasForeignKey(u => u.DepartmentId).OnDelete(DeleteBehavior.SetNull);
            entity.HasOne(u => u.JobLevel).WithMany().HasForeignKey(u => u.JobLevelId).OnDelete(DeleteBehavior.SetNull);
            entity.HasOne(u => u.JobPosition).WithMany().HasForeignKey(u => u.JobPositionId).OnDelete(DeleteBehavior.SetNull);
            entity.HasOne(u => u.ReportsToUser).WithMany().HasForeignKey(u => u.ReportsToUserId).OnDelete(DeleteBehavior.SetNull);
        });

        // Konfigurasi MaterialRequest
        modelBuilder.Entity<MaterialRequest>(entity =>
        {
            entity.HasIndex(m => m.RequestNumber).IsUnique();
            entity.HasOne(m => m.Requester).WithMany().HasForeignKey(m => m.RequesterId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(m => m.Department).WithMany().HasForeignKey(m => m.DepartmentId).OnDelete(DeleteBehavior.Restrict);
        });

        // Konfigurasi ApprovalRoute
        modelBuilder.Entity<ApprovalRoute>(entity =>
        {
            entity.HasOne(a => a.MaterialRequest).WithMany(m => m.ApprovalRoutes).HasForeignKey(a => a.MaterialRequestId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(a => a.ApproverUser).WithMany().HasForeignKey(a => a.ApproverUserId).OnDelete(DeleteBehavior.Restrict);
        });

        // Konfigurasi tabel Company
        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasIndex(c => c.Code).IsUnique();
            entity.Property(c => c.Name).IsRequired().HasMaxLength(200);
            entity.Property(c => c.Code).IsRequired().HasMaxLength(50);
        });

        // Konfigurasi tabel Branch
        modelBuilder.Entity<Branch>(entity =>
        {
            entity.HasIndex(b => b.Code).IsUnique();
            entity.Property(b => b.Name).IsRequired().HasMaxLength(200);
            entity.Property(b => b.Code).IsRequired().HasMaxLength(50);

            entity.HasOne(b => b.Company)
                  .WithMany(c => c.Branches)
                  .HasForeignKey(b => b.CompanyId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Konfigurasi tabel DiseasePhoto
        modelBuilder.Entity<DiseasePhoto>(entity =>
        {
            entity.Property(d => d.PhotoUrl).IsRequired().HasMaxLength(500);
            entity.Property(d => d.FileName).IsRequired().HasMaxLength(255);

            entity.HasOne(d => d.Sector)
                  .WithMany(s => s.DiseasePhotos)
                  .HasForeignKey(d => d.AgriculturalSectorId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(d => d.Company)
                  .WithMany()
                  .HasForeignKey(d => d.CompanyId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
