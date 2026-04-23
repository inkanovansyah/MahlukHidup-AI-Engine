using System;

namespace MahlukHidup.Backend.Models;

public enum ApprovalStatus { Pending, Approved, Rejected, Bypassed }

public class ApprovalRoute : BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public Guid MaterialRequestId { get; set; }
    public MaterialRequest? MaterialRequest { get; set; }
    
    public int ApproverUserId { get; set; } // Siapa atasan yang harus nge-klik ACC
    public User? ApproverUser { get; set; }
    
    public int StepOrder { get; set; } // Urutan ke-1, ke-2, ke-3, dst.
    public bool IsCurrentStep { get; set; } = false; // Flag apakah sekarang giliran atasan ini?
    
    public ApprovalStatus Status { get; set; } = ApprovalStatus.Pending;
    public string Notes { get; set; } = string.Empty; // Catatan atasan saat menolak/setuju
    
    public DateTime? ActionTakenAt { get; set; }
}
