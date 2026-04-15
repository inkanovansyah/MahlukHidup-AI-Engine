using System;

namespace MahlukHidup.Backend.Models;

public class SectorWater : BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid AgriculturalSectorId { get; set; }
    public int Usage { get; set; }
    public int Rainfall { get; set; }
    public int Efficiency { get; set; }

    public AgriculturalSector? Sector { get; set; }
}
