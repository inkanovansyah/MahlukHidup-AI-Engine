using System;

namespace MahlukHidup.Backend.Models;

public class SectorSoil : BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid AgriculturalSectorId { get; set; }
    public double PhLevel { get; set; }
    public int MoistureLevel { get; set; }
    public int NitrogenLevel { get; set; }
    public int OrganicLevel { get; set; }
    public string FertilizerStatus { get; set; } = string.Empty;

    public AgriculturalSector? Sector { get; set; }
}
