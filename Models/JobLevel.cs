using System;

namespace MahlukHidup.Backend.Models;

public class JobLevel : BaseEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty; // e.g. "Staff", "Manager", "CEO"
    public int Rank { get; set; } // e.g. 10 (Staff), 50 (Manager), 100 (CEO)
    public bool CanApprove { get; set; } = false; // Is this level allowed to approve requests?
    public string Description { get; set; } = string.Empty;
}
