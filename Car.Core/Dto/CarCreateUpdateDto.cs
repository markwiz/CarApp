using System.ComponentModel.DataAnnotations;

namespace Car.Core.Dto;

public sealed class CarCreateUpdateDto
{
    [Required]
    [StringLength(80)]
    public string Make { get; set; } = string.Empty;

    [Required]
    [StringLength(80)]
    public string Model { get; set; } = string.Empty;

    [Range(1886, 3000)]
    public int Year { get; set; }

    [Required]
    [StringLength(17, MinimumLength = 11)]
    public string Vin { get; set; } = string.Empty;

    [Range(0, int.MaxValue)]
    public int MileageKm { get; set; }
}
