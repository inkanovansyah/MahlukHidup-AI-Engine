using MahlukHidup.Backend.DTOs;
using MahlukHidup.Backend.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MahlukHidup.Backend.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth").WithOpenApi();

        group.MapPost("/register", async (RegisterRequest request, IAuthService authService) =>
        {
            var user = await authService.Register(request);
            if (user == null) return Results.BadRequest(new { error = "Email sudah digunakan." });
            return Results.Ok(new { message = "Registrasi berhasil." });
        })
        .WithName("Register");

        group.MapPost("/login", async (LoginRequest request, IAuthService authService) =>
        {
            var response = await authService.Login(request);
            if (response == null) return Results.Json(new { error = "Email atau password salah." }, statusCode: StatusCodes.Status401Unauthorized);
            return Results.Ok(new { message = "Login berhasil.", data = response });
        })
        .WithName("Login");
    }
}
