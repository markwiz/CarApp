namespace Car.Core.Dto;

public sealed class CarCreateUpdateDto
{
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;

    public int Year { get; set; }

    public string Vin { get; set; } = string.Empty;

    public int MileageKm { get; set; }
}
