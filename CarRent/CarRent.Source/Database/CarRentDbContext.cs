using CarRent.Source.CarManagement.Domain;
using CarRent.Source.CustomerManagement.Domain;
using CarRent.Source.ReservationManagement.Domain;
using Microsoft.EntityFrameworkCore;

namespace CarRent.Source.Database
{
    public class CarRentDbContext : DbContext
    {
        public CarRentDbContext(DbContextOptions options) : base(options) { }
        public DbSet<Car> Cars { get; set; }
        public DbSet<CarModel> CarModels { get; set; }
        public DbSet<CarCategory> CarCategories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Car>(c =>
            {
                c.HasOne<CarModel>().WithMany().OnDelete(DeleteBehavior.Restrict);
            });
            builder.Entity<Brand>(b => { b.HasMany<CarModel>().WithOne().OnDelete(DeleteBehavior.Restrict); });
            builder.Entity<CarModel>(c =>
            {
                c.HasOne<CarCategory>().WithOne().HasForeignKey<CarModel>(x => x.CategoryId).OnDelete(DeleteBehavior.Restrict);
            });
            builder.Entity<Customer>(c =>
            {
                c.HasMany<Reservation>().WithOne().HasForeignKey(r => r.CustomerId).OnDelete(DeleteBehavior.Restrict);
                // c.HasMany<RentalContract>().WithOne().HasForeignKey(rc => rc.CustomerId).OnDelete(DeleteBehavior.Restrict);
            });

            /*builder.Entity<RentalContract>(rc =>
            {
                rc.HasKey(x => x.Id);
                rc.HasOne<Reservation>().WithOne().OnDelete(DeleteBehavior.Cascade);
                rc.HasOne<Customer>().WithMany().OnDelete(DeleteBehavior.SetNull).IsRequired(false);
                rc.HasOne<Car>().WithMany().OnDelete(DeleteBehavior.SetNull).IsRequired(false);
            });*/

            builder.Entity<Reservation>(rc =>
            {
                rc.HasOne<Customer>().WithMany().OnDelete(DeleteBehavior.Restrict);
                rc.HasOne<Car>().WithMany().OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
