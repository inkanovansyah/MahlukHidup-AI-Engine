using System;

namespace MahlukHidup.Backend.Models;

public class SectorPest : BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid AgriculturalSectorId { get; set; }
    public int WerengCount { get; set; }
    public int UlatCount { get; set; }
    public int KutuCount { get; set; }
    public int BelalangCount { get; set; }
    public DateTime DateReported { get; set; } = DateTime.UtcNow;

    public AgriculturalSector? Sector { get; set; }
}
