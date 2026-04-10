namespace MahlukHidup.Backend.Models;

public class DroneDevice : BaseEntity
{
    public string Id { get; set; } = string.Empty;
    public int SystemHealth { get; set; }
    public int BatteryLevel { get; set; }
    public string Status { get; set; } = string.Empty;
    public string CurrentTask { get; set; } = string.Empty;
}
