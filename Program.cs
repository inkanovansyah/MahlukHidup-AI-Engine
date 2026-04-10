using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MahlukHidup.Backend.Data;
using MahlukHidup.Backend.Services;
using MahlukHidup.Backend.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// 1. Database Configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// 2. Dependency Injection
builder.Services.AddScoped<IAuthService, AuthService>();

// 3. JWT Authentication Setup
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value!)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

// OpenAPI/Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

// CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowViteFrontend",
        policy => policy.WithOrigins("http://localhost:5173").AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();

// Pipeline Configuration
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowViteFrontend");
app.UseAuthentication();
app.UseAuthorization();

// 4. MAP ENDPOINTS (Modular & Clean)
app.MapAuthEndpoints();
app.MapSpeciesEndpoints();
app.MapCompanyEndpoints();
app.MapBranchEndpoints();

app.MapGet("/", () => "Mahluk Hidup API (Modular Architecture) is running!")
   .WithName("GetRoot");

app.Run("http://localhost:4000");
