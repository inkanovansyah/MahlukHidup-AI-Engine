# Mahluk Hidup Backend

A .NET 9 backend API for the **Mahluk Hidup** (Living Creatures) application, built with ASP.NET Core Minimal APIs. This system manages agricultural sectors, drone devices, companies, branches, and user authentication with JWT.

## 🚀 Tech Stack

- **Framework**: ASP.NET Core 9.0 (Minimal APIs)
- **Language**: C# with Nullable Reference Types
- **Database**: MySQL (via Entity Framework Core)
- **Authentication**: JWT Bearer Tokens with BCrypt password hashing
- **API Documentation**: OpenAPI/Swagger
- **Architecture**: Clean Architecture with Minimal APIs pattern

## 📁 Project Structure

```
backend/
├── Data/
│   └── AppDbContext.cs          # EF Core database context
├── DTOs/
│   ├── AuthDto.cs               # Authentication data transfer objects
│   ├── BranchDto.cs             # Branch-related DTOs
│   └── CompanyDto.cs            # Company-related DTOs
├── Endpoints/
│   ├── AuthEndpoints.cs         # Authentication endpoints (login, register)
│   ├── BranchEndpoints.cs       # Branch CRUD operations
│   ├── CompanyEndpoints.cs      # Company CRUD operations
│   └── SpeciesEndpoints.cs      # Species/agricultural sector endpoints
├── Models/
│   ├── AgriculturalSector.cs    # Agricultural sector entity
│   ├── BaseEntity.cs            # Base entity with common fields
│   ├── Branch.cs                # Branch entity
│   ├── Company.cs               # Company entity
│   ├── DroneDevice.cs           # Drone device entity
│   ├── DroneTask.cs             # Drone task entity
│   ├── MonthlyRevenue.cs        # Monthly revenue tracking
│   ├── RecordActivity.cs        # Activity records
│   ├── SectorClimate.cs         # Climate data for sectors
│   ├── SectorFinance.cs         # Financial data for sectors
│   ├── SectorPhoto.cs           # Sector photo management
│   ├── SectorSoil.cs            # Soil data for sectors
│   └── User.cs                  # User entity with roles
├── Services/
│   ├── AuthService.cs           # Authentication service implementation
│   └── IAuthService.cs          # Authentication service interface
├── Program.cs                   # Application entry point & configuration
├── appsettings.json             # Application configuration
└── MahlukHidup.Backend.csproj   # Project file with dependencies
```

## 📋 Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) or later
- [MySQL Server](https://dev.mysql.com/downloads/mysql/) (8.0+)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/) with C# Dev Kit

## 🛠️ Setup & Installation

### 1. Clone the Repository

```bash
git clone <repository-url>
cd backend
```

### 2. Configure Database

Update the connection string in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "server=localhost;database=db_mahlukhidup;user=root;password=your_password"
  }
}
```

### 3. Restore Dependencies

```bash
dotnet restore
```

### 4. Create Database & Apply Migrations

```bash
# Create the database in MySQL
# Then run migrations (if migration files exist)
dotnet ef database update
```

> **Note**: If no migrations exist, you'll need to create them first:
> ```bash
> dotnet ef migrations add InitialCreate
> ```

### 5. Run the Application

```bash
dotnet run
```

The API will start on **http://localhost:4000**

## 🔑 API Endpoints

### Authentication
- `POST /api/auth/register` - Register a new user
- `POST /api/auth/login` - Login and receive JWT token

### Companies
- `GET /api/companies` - Get all companies
- `GET /api/companies/{id}` - Get company by ID
- `POST /api/companies` - Create a new company
- `PUT /api/companies/{id}` - Update a company
- `DELETE /api/companies/{id}` - Delete a company

### Branches
- `GET /api/branches` - Get all branches
- `GET /api/branches/{id}` - Get branch by ID
- `POST /api/branches` - Create a new branch
- `PUT /api/branches/{id}` - Update a branch
- `DELETE /api/branches/{id}` - Delete a branch

### Species/Agricultural Sectors
- Additional species and sector-related endpoints (see `SpeciesEndpoints.cs`)

> Check individual endpoint files in `/Endpoints` for detailed route definitions and request/response schemas.

## 🔐 Authentication

The API uses JWT Bearer token authentication. To access protected endpoints:

1. Register a user via `/api/auth/register`
2. Login via `/api/auth/login` to receive a token
3. Include the token in the Authorization header:
   ```
   Authorization: Bearer <your-token-here>
   ```

## ⚙️ Configuration

### appsettings.json

| Setting | Description | Default |
|---------|-------------|---------|
| `ConnectionStrings:DefaultConnection` | MySQL connection string | `server=localhost;database=db_mahlukhidup;user=root;password=root` |
| `AppSettings:Token` | JWT signing secret key | `my_secret_key_1234567890_very_long_and_secure_key` |

> **⚠️ Security Warning**: Never commit the `Token` value to version control. Use environment variables or User Secrets in production.

## 🗄️ Database Models

### Core Entities

- **User** - Application users with role-based access (Admin, Operator, etc.)
- **Company** - Company/organization entities
- **Branch** - Company branches/locations
- **AgriculturalSector** - Agricultural land sectors
- **DroneDevice** - Drone devices for sector monitoring
- **DroneTask** - Tasks assigned to drones
- **SectorClimate** - Climate data for agricultural sectors
- **SectorSoil** - Soil composition and quality data
- **SectorFinance** - Financial tracking for sectors
- **SectorPhoto** - Photo documentation
- **MonthlyRevenue** - Revenue tracking by month
- **RecordActivity** - Activity logging

## 📝 Development

### Run in Development Mode

```bash
dotnet run --environment Development
```

This enables OpenAPI/Swagger documentation at `/openapi/v1.json`.

### Build the Project

```bash
dotnet build
```

### Run Tests

```bash
dotnet test
```

### Add New Migration

```bash
dotnet ef migrations add <MigrationName>
```

### Update Database

```bash
dotnet ef database update
```

## 🐳 Docker (Optional)

If you want to run with Docker:

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 4000

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["MahlukHidup.Backend.csproj", "."]
RUN dotnet restore
COPY . .
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MahlukHidup.Backend.dll"]
```

Build and run:
```bash
docker build -t mahlukhidup-backend .
docker run -p 4000:4000 --env-file .env mahlukhidup-backend
```

## 🔧 CORS Configuration

The API is configured to accept requests from `http://localhost:5173` (default Vite dev server). Update the CORS policy in `Program.cs` if your frontend runs on a different origin.

## 📦 Dependencies

| Package | Version | Purpose |
|---------|---------|---------|
| Microsoft.EntityFrameworkCore | 9.0.0 | ORM for database operations |
| Pomelo.EntityFrameworkCore.MySql | 9.0.0-preview.2 | MySQL provider for EF Core |
| Microsoft.AspNetCore.Authentication.JwtBearer | 9.0.0 | JWT authentication |
| BCrypt.Net-Next | 4.0.3 | Password hashing |
| Microsoft.AspNetCore.OpenApi | 9.0.0 | OpenAPI/Swagger support |

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📄 License

This project is private and proprietary.

## 📞 Support

For questions or issues, please contact the development team.

---

**Built with** ❤️ **using ASP.NET Core Minimal APIs**
