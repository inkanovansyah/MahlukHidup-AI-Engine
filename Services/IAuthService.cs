using MahlukHidup.Backend.DTOs;
using MahlukHidup.Backend.Models;

namespace MahlukHidup.Backend.Services;

public interface IAuthService
{
    Task<AuthResponse?> Login(LoginRequest request);
    Task<User?> Register(RegisterRequest request);
    string CreateToken(User user);
}
