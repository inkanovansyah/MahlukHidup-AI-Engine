using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MahlukHidup.Backend.Data;
using MahlukHidup.Backend.DTOs;
using MahlukHidup.Backend.Models;

namespace MahlukHidup.Backend.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<AuthResponse?> Login(LoginRequest request)
    {
        var user = await _context.Users
            .Include(u => u.Department)
            .Include(u => u.JobLevel)
            .Include(u => u.JobPosition)
            .FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return null;
        }

        var token = CreateToken(user);

        // Catat aktivitas Login berhasil
        _context.RecordActivities.Add(new RecordActivity
        {
            Action = "Login",
            EntityName = "User",
            EntityId = user.Id.ToString(),
            Details = $"User dengan email {user.Email} melakukan login.",
            CreatedBy = user.Id,
            CreatedAt = DateTime.UtcNow,
            IsActive = true,
            IsDeleted = false
        });

        await _context.SaveChangesAsync();

        return new AuthResponse
        {
            Token = token,
            User = new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.JobLevel?.Name ?? "Employee", // Dynamic Role from Job Level
                DepartmentId = user.DepartmentId,
                DepartmentName = user.Department?.Name,
                JobLevelId = user.JobLevelId,
                JobLevelName = user.JobLevel?.Name,
                JobPositionId = user.JobPositionId,
                JobPositionName = user.JobPosition?.Name,
                JobLevelRank = user.JobLevel?.Rank
            }
        };
    }

    public async Task<User?> Register(RegisterRequest request)
    {
        if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            return null;

        // Validate foreign key references before assigning
        int? departmentId = null;
        if (request.DepartmentId.HasValue && await _context.Departments.AnyAsync(d => d.Id == request.DepartmentId.Value))
            departmentId = request.DepartmentId;

        int? jobLevelId = null;
        if (request.JobLevelId.HasValue && await _context.JobLevels.AnyAsync(j => j.Id == request.JobLevelId.Value))
            jobLevelId = request.JobLevelId;

        int? jobPositionId = null;
        if (request.JobPositionId.HasValue && await _context.JobPositions.AnyAsync(p => p.Id == request.JobPositionId.Value))
            jobPositionId = request.JobPositionId;

        int? reportsToUserId = null;
        if (request.ReportsToUserId.HasValue && await _context.Users.AnyAsync(u => u.Id == request.ReportsToUserId.Value))
            reportsToUserId = request.ReportsToUserId;

        var user = new User
        {
            Name = request.Name,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            DepartmentId = departmentId,
            JobLevelId = jobLevelId,
            JobPositionId = jobPositionId,
            ReportsToUserId = reportsToUserId
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync(); // get generated Id

        // Record activity
        _context.RecordActivities.Add(new RecordActivity
        {
            Action = "Register",
            EntityName = "User",
            EntityId = user.Id.ToString(),
            Details = $"User baru terdaftar dengan email {user.Email}.",
            CreatedBy = user.Id,
            CreatedAt = DateTime.UtcNow,
            IsActive = true,
            IsDeleted = false
        });
        await _context.SaveChangesAsync();

        return user;
    }

    public string CreateToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.JobLevel?.Name ?? "Employee") // Modified
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _configuration.GetSection("AppSettings:Token").Value!));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }
}
