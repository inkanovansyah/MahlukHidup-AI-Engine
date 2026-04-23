using System;

namespace MahlukHidup.Backend.Models;

public class Department : BaseEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty; // e.g. "Operasional Kebun", "IT Support"
    public string Description { get; set; } = string.Empty;
}
