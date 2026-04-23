using System;
using System.Collections.Generic;

namespace MahlukHidup.Backend.DTOs;

public class MaterialRequestDto
{
    public Guid Id { get; set; }
    public string RequestNumber { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public int RequesterId { get; set; }
    public string RequesterName { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    public Guid? BranchId { get; set; }
    public string BranchName { get; set; } = string.Empty;
    public string Justification { get; set; } = string.Empty;
    public decimal TotalEstimatedPrice { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    
    public List<MaterialRequestItemDto> Items { get; set; } = new();
    public List<ApprovalRouteDto> ApprovalRoutes { get; set; } = new();
}

public class MaterialRequestItemDto
{
    public Guid Id { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public string UnitOfMeasure { get; set; } = string.Empty;
    public decimal? EstimatedUnitPrice { get; set; }
    public decimal? TotalPrice { get; set; }
}

public class ApprovalRouteDto
{
    public Guid Id { get; set; }
    public int ApproverUserId { get; set; }
    public string ApproverName { get; set; } = string.Empty;
    public int StepOrder { get; set; }
    public bool IsCurrentStep { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public DateTime? ActionTakenAt { get; set; }
}

public class CreateMaterialRequestDto
{
    public string Type { get; set; } = "Saprotan"; // Saprotan or Asset
    public string Justification { get; set; } = string.Empty;
    public List<CreateMaterialRequestItemDto> Items { get; set; } = new();
}

public class CreateMaterialRequestItemDto
{
    public string ItemName { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public string UnitOfMeasure { get; set; } = string.Empty;
    public decimal EstimatedUnitPrice { get; set; }
}

public class ApprovalActionDto
{
    public string Action { get; set; } = "Approve"; // Approve or Reject
    public string Notes { get; set; } = string.Empty;
}
