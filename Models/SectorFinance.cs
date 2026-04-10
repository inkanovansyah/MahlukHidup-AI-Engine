using System;

namespace MahlukHidup.Backend.Models;

public class SectorFinance : BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid AgriculturalSectorId { get; set; }
    public decimal PotentialProfit { get; set; }
    public decimal PotentialRisk { get; set; }

    public AgriculturalSector? Sector { get; set; }
}
