using CarRent.Source.CarManagement.Domain;
using CarRent.Source.CarManagement.Services;
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
            services.AddScoped<IBrandService, BrandService>();
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
