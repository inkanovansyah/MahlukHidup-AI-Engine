using System;
using System.ComponentModel.DataAnnotations;

namespace MahlukHidup.Backend.DTOs;

public class DiseasePhotoUploadDto
{
    [Required]
    public Guid AgriculturalSectorId { get; set; }

    [Required]
    public Guid CompanyId { get; set; }

    public string? Description { get; set; }
    
    public DateTime? CapturedAt { get; set; }
}

public class DiseasePhotoResponseDto
{
    public Guid Id { get; set; }
    public Guid AgriculturalSectorId { get; set; }
    public Guid CompanyId { get; set; }
    public string PhotoUrl { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string? Description { get; set; }
    public DateTime CapturedAt { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class DiseasePhotoListDto
{
    public Guid Id { get; set; }
    public Guid AgriculturalSectorId { get; set; }
    public Guid CompanyId { get; set; }
    public string PhotoUrl { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string? Description { get; set; }
    public DateTime CapturedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? SectorName { get; set; }
    public string? CompanyName { get; set; }
}

public class DiseasePhotoDeleteDto
{
    public Guid Id { get; set; }
}
