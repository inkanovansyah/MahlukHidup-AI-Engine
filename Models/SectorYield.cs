using System;

namespace MahlukHidup.Backend.Models;

public class SectorYield : BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid AgriculturalSectorId { get; set; }
    public double Current { get; set; }
    public double Target { get; set; }
    public string Unit { get; set; } = "Ton";

    public AgriculturalSector? Sector { get; set; }
}
