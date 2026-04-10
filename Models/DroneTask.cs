using System;

namespace MahlukHidup.Backend.Models;

public class DroneTask : BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string TaskName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TimeSpan ScheduledTime { get; set; }
    public string Priority { get; set; } = string.Empty; 
}
