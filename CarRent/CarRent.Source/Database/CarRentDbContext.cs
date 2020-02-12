using CarRent.Source.CarManagement.Domain;
using CarRent.Source.ContractManagement.Domain;
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
        public DbSet<RentalContract> RentalContracts { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Car>(c =>
            {
                c.HasOne<CarModel>().WithMany().OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<Brand>(b => { b.HasMany<CarModel>().WithOne().OnDelete(DeleteBehavior.Restrict); });
            modelBuilder.Entity<CarModel>(c =>
            {
                c.HasOne<CarCategory>().WithOne().HasForeignKey<CarModel>(x => x.CategoryId).OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<RentalContract>(rc =>
            {
                rc.HasOne<Reservation>().WithOne().OnDelete(DeleteBehavior.Cascade);
                rc.HasOne<Customer>().WithMany().OnDelete(DeleteBehavior.Restrict);
                rc.HasOne<Car>().WithMany().OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Reservation>(rc =>
            {
                rc.HasOne<Customer>().WithMany().OnDelete(DeleteBehavior.Restrict);
                rc.HasOne<Car>().WithMany().OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
