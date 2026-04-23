using MahlukHidup.Backend.Models;

namespace MahlukHidup.Backend.DTOs;

public class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class RegisterRequest
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    
    public int? DepartmentId { get; set; }
    public int? JobLevelId { get; set; }
    public int? JobPositionId { get; set; }
    public int? ReportsToUserId { get; set; }
}

public class AuthResponse
{
    public string Token { get; set; } = string.Empty;
    public UserDto User { get; set; } = null!;
}

public class UserDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty; // For frontend backward compatibility, we will map JobLevelName here
    
    // New HR fields
    public int? DepartmentId { get; set; }
    public string? DepartmentName { get; set; }
    public int? JobLevelId { get; set; }
    public string? JobLevelName { get; set; }
    public int? JobPositionId { get; set; }
    public string? JobPositionName { get; set; }
    public int? JobLevelRank { get; set; }
}
