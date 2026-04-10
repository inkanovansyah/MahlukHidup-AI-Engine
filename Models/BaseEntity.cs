using System;

namespace MahlukHidup.Backend.Models;

public abstract class BaseEntity
{
    public bool IsActive { get; set; } = true;
    public bool IsModified { get; set; } = false;
    public DateTime CreateDate { get; set; } = DateTime.UtcNow;
    public DateTime? UpdateDate { get; set; }
}
