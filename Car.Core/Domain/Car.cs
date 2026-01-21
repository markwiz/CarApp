namespace Car.Core.Domain;

public sealed class Car
{
    public Guid Id { get; set; }

    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;

    public int Year { get; set; }

    public string Vin { get; set; } = string.Empty;

    public int MileageKm { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset ModifiedAt { get; set; }
}
