using System;

namespace MahlukHidup.Backend.Models;

public class SolarPanel : BaseEntity
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public double CurrentOutput { get; set; }
    public double Capacity { get; set; }
    public int SystemHealth { get; set; }
    public string Status { get; set; } = string.Empty;
    public string CurrentTask { get; set; } = string.Empty;
}
