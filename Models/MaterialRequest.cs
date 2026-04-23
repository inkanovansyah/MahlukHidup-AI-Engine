using System;
using System.Collections.Generic;

namespace MahlukHidup.Backend.Models;

public enum RequestType { Saprotan, Asset }
public enum RequestStatus { Pending, InProgress, Approved, Rejected, Completed }

public class MaterialRequest : BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string RequestNumber { get; set; } = string.Empty; // e.g. "REQ-SAP-2026-001"
    public RequestType Type { get; set; } 
    
    // Siapa yang mengajukan
    public int RequesterId { get; set; }
    public User? Requester { get; set; }
    
    // Dari Departemen mana
    public int DepartmentId { get; set; }
    public Department? Department { get; set; }

    // Cabang mana yang membuat pengajuan
    public Guid? BranchId { get; set; }
    public Branch? Branch { get; set; }

    public string Justification { get; set; } = string.Empty;
    public decimal TotalEstimatedPrice { get; set; } // Penentu limit hierarki approval

    public RequestStatus Status { get; set; } = RequestStatus.Pending;
    
    public ICollection<MaterialRequestItem> Items { get; set; } = new List<MaterialRequestItem>();
    public ICollection<ApprovalRoute> ApprovalRoutes { get; set; } = new List<ApprovalRoute>();
}
