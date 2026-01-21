using Car.Core.Dto;
using Car.Core.Exceptions;
using Car.Core.ServiceInterface;
using Car.Data;
using Microsoft.EntityFrameworkCore;

namespace Car.ApplicationServices.Services;

public sealed class CarService : ICarService
{
    private readonly CarContext _context;

    public CarService(CarContext context)
    {
        _context = context;
    }

    public async Task<CarDto> AddAsync(CarCreateUpdateDto dto)
    {
        Validate(dto);

        var now = DateTimeOffset.UtcNow;

        var car = new Car.Core.Domain.Car
        {
            Id = Guid.NewGuid(),
            Make = dto.Make.Trim(),
            Model = dto.Model.Trim(),
            Year = dto.Year,
            Vin = dto.Vin.Trim(),
            MileageKm = dto.MileageKm,
            CreatedAt = now,
            ModifiedAt = now
        };

        _context.Cars.Add(car);
        await _context.SaveChangesAsync();

        return Map(car);
    }

    public async Task<CarDto> UpdateAsync(Guid id, CarCreateUpdateDto dto)
    {
        Validate(dto);

        var car = await _context.Cars.FirstOrDefaultAsync(x => x.Id == id);
        if (car == null)
            throw new ValidationException("Car not found");

        car.Make = dto.Make.Trim();
        car.Model = dto.Model.Trim();
        car.Year = dto.Year;
        car.Vin = dto.Vin.Trim();
        car.MileageKm = dto.MileageKm;
        car.ModifiedAt = DateTimeOffset.UtcNow;

        await _context.SaveChangesAsync();

        return Map(car);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var car = await _context.Cars.FirstOrDefaultAsync(x => x.Id == id);
        if (car == null)
            return false;

        _context.Cars.Remove(car);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<CarDto?> GetAsync(Guid id)
    {
        var car = await _context.Cars.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return car == null ? null : Map(car);
    }

    public async Task<List<CarDto>> GetAllAsync()
    {
        return await _context.Cars
            .AsNoTracking()
            .OrderBy(x => x.Make)
            .Select(x => Map(x))
            .ToListAsync();
    }

    private static void Validate(CarCreateUpdateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Make))
            throw new ValidationException("Make is required");

        if (string.IsNullOrWhiteSpace(dto.Model))
            throw new ValidationException("Model is required");

        if (dto.Year < 1886 || dto.Year > DateTime.UtcNow.Year + 1)
            throw new ValidationException("Invalid year");

        if (string.IsNullOrWhiteSpace(dto.Vin))
            throw new ValidationException("Vin is required");

        if (dto.MileageKm < 0)
            throw new ValidationException("MileageKm must be zero or greater");
    }

    private static CarDto Map(Car.Core.Domain.Car car)
    {
        return new CarDto
        {
            Id = car.Id,
            Make = car.Make,
            Model = car.Model,
            Year = car.Year,
            Vin = car.Vin,
            MileageKm = car.MileageKm,
            CreatedAt = car.CreatedAt,
            ModifiedAt = car.ModifiedAt
        };
    }
}
