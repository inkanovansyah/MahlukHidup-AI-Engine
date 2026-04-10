using System;

namespace MahlukHidup.Backend.Models;

public class MonthlyRevenue : BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Month { get; set; } = string.Empty;
    public decimal RevenueGenerated { get; set; }
    public decimal YieldTonnage { get; set; }
}
