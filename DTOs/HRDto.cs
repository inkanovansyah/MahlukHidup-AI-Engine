using System;

namespace MahlukHidup.Backend.DTOs;

public class DepartmentDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class JobLevelDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Rank { get; set; }
    public bool CanApprove { get; set; }
    public string Description { get; set; } = string.Empty;
}

public class JobPositionDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class HRCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class JobLevelCreateDto : HRCreateDto
{
    public int Rank { get; set; }
    public bool CanApprove { get; set; }
}
