using System;
using AutoMapper;
using CarRent.Source.CarManagement.Domain;
using CarRent.Source.CarManagement.Dtos;
using CarRent.Source.Common;
using CarRent.Source.Database;
using CarRent.Source.Extensions;
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
            services.AddScoped<IRepository<Brand>, DatabaseRepository<Brand>>();
            services.AddScoped<ICrudService<Brand>, CrudService<Brand>>();
            services.AddScoped<IRepository<CarCategory>, DatabaseRepository<CarCategory>>();
            services.AddScoped<ICrudService<CarCategory>, CrudService<CarCategory>>();
            services.AddScoped<IRepository<CarModel>, DatabaseRepository<CarModel>>();
            services.AddScoped<ICrudService<CarModel>, CrudService<CarModel>>();
            services.AddAutoMapper(config =>
            {
                config.CreateMap<Brand, BrandDto>();
                config.CreateMap<BrandDto, Brand>();
                config.CreateMap<CarCategory, CarCategoryDto>();
                config.CreateMap<CarCategoryDto, CarCategory>();
                config.CreateMap<CarModel, CarModelDto>();
                config.CreateMap<CarModelDto, CarModel>();
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
