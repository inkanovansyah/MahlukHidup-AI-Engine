using System;
using System.Collections.Generic;

namespace MahlukHidup.Backend.Models;

public class AgriculturalSector : BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public double LandArea { get; set; } 
    public string CropType { get; set; } = string.Empty;
    public int PlantHealth { get; set; }
    public string PestRisk { get; set; } = string.Empty;
    public string AiRecommendation { get; set; } = string.Empty;
    
    // Navigation Properties
    public SectorSoil? Soil { get; set; }
    public SectorClimate? Climate { get; set; }
    public SectorFinance? Finance { get; set; }
    public ICollection<SectorPhoto> Photos { get; set; } = new List<SectorPhoto>();
}
