using CarRent.Source.CarManagement.Domain;
using Microsoft.EntityFrameworkCore;

namespace CarRent.Source.Database
{
    public class CarRentDbContext : DbContext
    {
        public CarRentDbContext(DbContextOptions options) : base(options) { }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Type> Types { get; set; }
        public DbSet<Brand> Brands { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Car>(c =>
            {
                c.HasKey(x => x.Id);
                c.HasOne<CarClass>().WithMany();
                // c.HasMany<Reservation>().WithOne().HasForeignKey(r => r.CarId).OnDelete(DeleteBehavior.Cascade);
                // c.HasMany<RentalContract>().WithOne().HasForeignKey(rc => rc.CarId).OnDelete(DeleteBehavior.Cascade);
                c.HasOne<Type>().WithMany();
                c.HasOne<Brand>().WithMany();
            });

            /*builder.Entity<Customer>(c =>
            {
                c.HasKey(x => x.Id);
                c.HasMany<Reservation>().WithOne().HasForeignKey(r => r.CustomerId).OnDelete(DeleteBehavior.Restrict);
                c.HasMany<RentalContract>().WithOne().HasForeignKey(rc => rc.CustomerId).OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<RentalContract>(rc =>
            {
                rc.HasKey(x => x.Id);
                rc.HasOne<Reservation>().WithOne().OnDelete(DeleteBehavior.Cascade);
                rc.HasOne<Customer>().WithMany().OnDelete(DeleteBehavior.SetNull).IsRequired(false);
                rc.HasOne<Car>().WithMany().OnDelete(DeleteBehavior.SetNull).IsRequired(false);
            });

            builder.Entity<Reservation>(rc =>
            {
                rc.HasKey(x => x.Id);
                rc.HasOne<Customer>().WithMany().OnDelete(DeleteBehavior.SetNull).IsRequired(false);
                rc.HasOne<Car>().WithMany().OnDelete(DeleteBehavior.SetNull).IsRequired(false);
            });*/
        }
    }
}
