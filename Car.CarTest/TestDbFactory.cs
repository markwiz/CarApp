using Car.Data;
using Microsoft.EntityFrameworkCore;

namespace Car.CarTest;

public static class TestDbFactory
{
    public static CarContext CreateContext(string databaseName)
    {
        var options = new DbContextOptionsBuilder<CarContext>()
            .UseInMemoryDatabase(databaseName)
            .Options;

        return new CarContext(options);
    }
}
