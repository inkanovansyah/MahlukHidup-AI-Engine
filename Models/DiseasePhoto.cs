using System;

namespace MahlukHidup.Backend.Models;

public class DiseasePhoto : BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid AgriculturalSectorId { get; set; }
    public Guid CompanyId { get; set; }
    public string PhotoUrl { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string? Description { get; set; }
    public DateTime CapturedAt { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    public AgriculturalSector? Sector { get; set; }
    public Company? Company { get; set; }
}
