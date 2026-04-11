using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace MahlukHidup.Backend.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? "Server=localhost;Port=3306;Database=mahlukhidup;User=root;Password=;";

        var builder = new DbContextOptionsBuilder<AppDbContext>();
        builder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

        return new AppDbContext(builder.Options);
    }
}
