using Car.ApplicationServices.Services;
using Car.Core.Dto;
using Car.Core.Exceptions;
using Xunit;

namespace Car.CarTest;

public sealed class CarServiceTests
{
    [Fact]
    public async Task AddAsync_CreatesCar_WithTimestamps()
    {
        var dbName = Guid.NewGuid().ToString();
        await using var context = TestDbFactory.CreateContext(dbName);
        var service = new CarService(context);

        var before = DateTimeOffset.UtcNow;

        var dto = new CarCreateUpdateDto
        {
            Make = "Toyota",
            Model = "Corolla",
            Year = 2020,
            Vin = "JT123456789012345",
            MileageKm = 120000
        };

        var created = await service.AddAsync(dto);

        var after = DateTimeOffset.UtcNow;

        Assert.NotEqual(Guid.Empty, created.Id);
        Assert.Equal("Toyota", created.Make);
        Assert.Equal("Corolla", created.Model);
        Assert.Equal(2020, created.Year);
        Assert.Equal("JT123456789012345", created.Vin);
        Assert.Equal(120000, created.MileageKm);

        Assert.True(created.CreatedAt >= before && created.CreatedAt <= after);
        Assert.True(created.ModifiedAt >= created.CreatedAt);
    }
    [Fact]
    public async Task UpdateAsync_ChangesModifiedAt_AndUpdatesFields()
    {
        var dbName = Guid.NewGuid().ToString();
        await using var context = TestDbFactory.CreateContext(dbName);
        var service = new CarService(context);

        var created = await service.AddAsync(new CarCreateUpdateDto
        {
            Make = "Honda",
            Model = "Civic",
            Year = 2019,
            Vin = "HND12345678901234",
            MileageKm = 50000
        });

        var beforeUpdate = await service.GetAsync(created.Id);
        Assert.NotNull(beforeUpdate);

        await Task.Delay(10);

        var updated = await service.UpdateAsync(created.Id, new CarCreateUpdateDto
        {
            Make = "Honda",
            Model = "Civic Sport",
            Year = 2019,
            Vin = "HND12345678901234",
            MileageKm = 52000
        });

        Assert.Equal(created.Id, updated.Id);
        Assert.Equal("Civic Sport", updated.Model);
        Assert.Equal(52000, updated.MileageKm);
        Assert.True(updated.ModifiedAt > beforeUpdate!.ModifiedAt);
    }
    [Fact]
    public async Task DeleteAsync_RemovesCar()
    {
        var dbName = Guid.NewGuid().ToString();
        await using var context = TestDbFactory.CreateContext(dbName);
        var service = new CarService(context);

        var created = await service.AddAsync(new CarCreateUpdateDto
        {
            Make = "BMW",
            Model = "320i",
            Year = 2018,
            Vin = "BMW12345678901234",
            MileageKm = 88000
        });

        var deleted = await service.DeleteAsync(created.Id);

        var after = await service.GetAsync(created.Id);

        Assert.True(deleted);
        Assert.Null(after);
    }
    [Fact]
    public async Task AddAsync_ThrowsValidationException_WhenVinAlreadyExists()
    {
        var dbName = Guid.NewGuid().ToString();
        await using var context = TestDbFactory.CreateContext(dbName);
        var service = new CarService(context);

        var vin = "DUP12345678901234";

        await service.AddAsync(new CarCreateUpdateDto
        {
            Make = "Audi",
            Model = "A4",
            Year = 2021,
            Vin = vin,
            MileageKm = 10000
        });

        var ex = await Assert.ThrowsAsync<ValidationException>(async () =>
            await service.AddAsync(new CarCreateUpdateDto
            {
                Make = "Audi",
                Model = "A6",
                Year = 2022,
                Vin = vin,
                MileageKm = 9000
            }));

        Assert.Equal("Vin already exists", ex.Message);
    }
}




