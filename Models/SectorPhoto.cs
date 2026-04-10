using System;

namespace MahlukHidup.Backend.Models;

public class SectorPhoto : BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid AgriculturalSectorId { get; set; }
    public string PhotoUrl { get; set; } = string.Empty;
    public DateTime CapturedAt { get; set; } = DateTime.UtcNow;

    public AgriculturalSector? Sector { get; set; }
}
