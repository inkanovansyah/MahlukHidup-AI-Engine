using System;

namespace MahlukHidup.Backend.Models;

public class RecordActivity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Action { get; set; } = string.Empty;      // e.g. "CREATE", "UPDATE", "DELETE"
    public string EntityName { get; set; } = string.Empty;  // e.g. "AgriculturalSector", "User"
    public string EntityId { get; set; } = string.Empty;    // Record ID that was modified
    public string Details { get; set; } = string.Empty;     // JSON snapshot or desc
    
    public DateTime CreateDate { get; set; } = DateTime.UtcNow;
    public Guid? UserId { get; set; } // ID user yang melakukan aksi (opsional jika system)
}
