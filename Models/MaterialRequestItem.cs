using System;

namespace MahlukHidup.Backend.Models;

public class MaterialRequestItem : BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid MaterialRequestId { get; set; }
    public MaterialRequest? MaterialRequest { get; set; }
    
    public string ItemName { get; set; } = string.Empty; 
    public decimal Quantity { get; set; }
    public string UnitOfMeasure { get; set; } = string.Empty; // e.g. "Kg", "Unit"
    public decimal? EstimatedUnitPrice { get; set; } 
    public decimal? TotalPrice => Quantity * (EstimatedUnitPrice ?? 0);
}
