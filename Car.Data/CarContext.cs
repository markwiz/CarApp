using Microsoft.EntityFrameworkCore;

namespace Car.Data;

public sealed class CarContext : DbContext
{
    public CarContext(DbContextOptions<CarContext> options)
        : base(options)
    {
    }

    public DbSet<Car.Core.Domain.Car> Cars { get; set; } = null!;
}
