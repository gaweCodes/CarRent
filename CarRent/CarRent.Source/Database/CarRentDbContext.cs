using CarRent.Source.CarManagement.Domain;
using Microsoft.EntityFrameworkCore;

namespace CarRent.Source.Database
{
    public class CarRentDbContext : DbContext
    {
        public CarRentDbContext(DbContextOptions<CarRentDbContext> options) : base(options) { }
        public DbSet<Car> Cars { get; set; }
    }
}
