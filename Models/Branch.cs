using System;

namespace MahlukHidup.Backend.Models;

public class Branch : BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CompanyId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public bool IsMaster { get; set; } = false;
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    // Navigation Properties
    public Company? Company { get; set; }
}
