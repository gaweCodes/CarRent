using System;
using AutoMapper;
using CarRent.Source.CarManagement.Domain;
using CarRent.Source.CarManagement.Dtos;
using CarRent.Source.CarManagement.Repositories;
using CarRent.Source.CarManagement.Repositories.Interfaces;
using CarRent.Source.CarManagement.Services;
using CarRent.Source.CarManagement.Services.Interfaces;
using CarRent.Source.ContractManagement.Domain;
using CarRent.Source.ContractManagement.Dtos;
using CarRent.Source.ContractManagement.Repositories;
using CarRent.Source.ContractManagement.Repositories.Interfaces;
using CarRent.Source.ContractManagement.Services;
using CarRent.Source.ContractManagement.Services.Interfaces;
using CarRent.Source.CustomerManagement.Domain;
using CarRent.Source.CustomerManagement.Dtos;
using CarRent.Source.CustomerManagement.Repositories;
using CarRent.Source.CustomerManagement.Repositories.Interfaces;
using CarRent.Source.CustomerManagement.Services;
using CarRent.Source.CustomerManagement.Services.Interfaces;
using CarRent.Source.Database;
using CarRent.Source.Extensions;
using CarRent.Source.ReservationManagement.Repositories;
using CarRent.Source.ReservationManagement.Repositories.Interfaces;
using CarRent.Source.ReservationManagement.Services;
using CarRent.Source.ReservationManagement.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CarRent.Source
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CarRentDbContext>(options =>
            {
                options.UseMySql(Configuration.GetConnectionString("CarRentDb"));
            }, ServiceLifetime.Transient);
            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<IBrandService, BrandService>();
            services.AddScoped<ICarCategoryRepository, CarCategoryRepository>();
            services.AddScoped<ICarCategoryService, CarCategoryService>();
            services.AddScoped<ICarModelRepository, CarModelRepository>();
            services.AddScoped<ICarModelService, CarModelService>();
            services.AddScoped<ICarRepository, CarRepository>();
            services.AddScoped<ICarService, CarService>();
            
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICustomerService, CustomerService>();

            services.AddScoped<IReservationService, ReservationService>();
            services.AddScoped<IReservationRepository, ReservationRepository>();

            services.AddScoped<IRentalContractService, RentalContractService>();
            services.AddScoped<IRentalContractRepository, RentalContractRepository>();

            services.AddAutoMapper(config =>
            {
                config.CreateMap<Brand, BrandDto>();
                config.CreateMap<BrandDto, Brand>();
                config.CreateMap<CarCategory, CarCategoryDto>();
                config.CreateMap<CarCategoryDto, CarCategory>();
                config.CreateMap<CarModel, CarModelDto>();
                config.CreateMap<CarModelDto, CarModel>();
                config.CreateMap<Car, CarDto>();
                config.CreateMap<CarDto, Car>();

                config.CreateMap<Customer, CustomerDto>();
                config.CreateMap<CustomerDto, Customer>();

                config.CreateMap<RentalContract, RentalContractDto>();
                config.CreateMap<RentalContractDto, RentalContract>();

            }, AppDomain.CurrentDomain.GetAssemblies());
            services.AddControllers();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            app.UseSpaRouting();
        }
    }
}
