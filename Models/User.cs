namespace MahlukHidup.Backend.Models;

public class User : BaseEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string? ProfilePictureUrl { get; set; }
    
    // Organizational Structure
    public int? DepartmentId { get; set; }
    public Department? Department { get; set; }

    public int? JobLevelId { get; set; }
    public JobLevel? JobLevel { get; set; }

    public int? JobPositionId { get; set; }
    public JobPosition? JobPosition { get; set; }

    // Direct Manager Loop (Who does this user report to?)
    public int? ReportsToUserId { get; set; }
    public User? ReportsToUser { get; set; }

    public Guid? BranchId { get; set; }
    public Branch? Branch { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiry { get; set; }
}
