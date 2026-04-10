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
            if (user == null) return Results.BadRequest("Email already exists.");
            return Results.Ok("User registered successfully.");
        })
        .WithName("Register");

        group.MapPost("/login", async (LoginRequest request, IAuthService authService) =>
        {
            var response = await authService.Login(request);
            if (response == null) return Results.Unauthorized();
            return Results.Ok(response);
        })
        .WithName("Login");
    }
}
