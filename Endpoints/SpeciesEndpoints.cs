using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MahlukHidup.Backend.Endpoints;

public static class SpeciesEndpoints
{
    public static void MapSpeciesEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/species").RequireAuthorization().WithOpenApi();

        var speciesList = new List<Species>
        {
            new Species { Id = 1, Name = "Komodo Dragon", ScientificName = "Varanus komodoensis", Description = "Kadal terbesar di dunia." },
            new Species { Id = 2, Name = "Rafflesia Arnoldii", ScientificName = "Rafflesia arnoldii", Description = "Bunga terbesar di dunia." }
        };

        group.MapGet("/", () => Results.Ok(speciesList))
             .WithName("GetSpecies");
    }

    public class Species
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ScientificName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
