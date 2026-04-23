using System;

namespace MahlukHidup.Backend.Models;

public class JobPosition : BaseEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty; // e.g. "Senior Agronomist", "IT Admin"
    public string Description { get; set; } = string.Empty;
}
