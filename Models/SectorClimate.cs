using System;

namespace MahlukHidup.Backend.Models;

public class SectorClimate : BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid AgriculturalSectorId { get; set; }
    public double TemperatureCelcius { get; set; }
    public string Condition { get; set; } = string.Empty;

    public AgriculturalSector? Sector { get; set; }
}
