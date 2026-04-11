using System;
using System.Collections.Generic;

namespace MahlukHidup.Backend.Models;

public class Company : BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? LogoUrl { get; set; }

    // Navigation Properties
    public ICollection<Branch> Branches { get; set; } = new List<Branch>();
}
