using Microsoft.EntityFrameworkCore;
using MahlukHidup.Backend.Models;

namespace MahlukHidup.Backend.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    
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
