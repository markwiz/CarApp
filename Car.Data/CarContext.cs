using Microsoft.EntityFrameworkCore;

namespace Car.Data;

public sealed class CarContext : DbContext
{
    public CarContext(DbContextOptions<CarContext> options)
        : base(options)
    {
    }

    public DbSet<Car.Core.Domain.Car> Cars { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)

    {
        modelBuilder.Entity<Car.Core.Domain.Car>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Make).IsRequired().HasMaxLength(80);
            entity.Property(x => x.Model).IsRequired().HasMaxLength(80);
            entity.Property(x => x.Vin).IsRequired().HasMaxLength(17);

            entity.HasIndex(x => x.Vin).IsUnique();

            entity.Property(x => x.CreatedAt).IsRequired();
            entity.Property(x => x.ModifiedAt).IsRequired();
        });
    }
}
