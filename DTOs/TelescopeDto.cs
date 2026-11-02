namespace TelescopeShop.DTOs;

public class TelescopeDto
{
    public int Id { get; set; }

    public string Manufacturer { get; set; } = string.Empty;

    public int TripodHeight { get; set; }

    public int Aperture { get; set; }

    public string Seeker { get; set; } = string.Empty;

    public bool BarlowLensIncluded { get; set; }

    public int MaximumUsefulMagnification { get; set; }

    public int FocalLength { get; set; }

    public string Description { get; set; } = string.Empty;
}
