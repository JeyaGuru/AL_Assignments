using AL.RMZ.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AL.RMZ
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            //services.AddCors(options =>
            //{
            //    options.AddDefaultPolicy(
            //                      policy =>
            //                      {
            //                          policy.WithOrigins("*");
            //                      });
            //});
            services.AddCors(option => option.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();

            }));

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v2", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "RMZ info",
                    Version = "v2",
                    Description = "API Helper",
                });
            });

            // services.AddDbContext<RMZ.Data.RMZAPIDbContext>(options => options.UseInMemoryDatabase("RMZDb"));
            services.AddDbContext<RMZ.Data.RMZAPIDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("RMZAPIConnectionString")));
            
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<IFacilityRepository, FacilityRepository>();
            services.AddScoped<IBuildingRepository, BuildingRepository>();
            services.AddScoped<IFloorRepository, FloorRepository>();
            services.AddScoped<IZoneRepository, ZoneRepository>();
            services.AddScoped<IElectricityMeterRepository, ElectricityMeterRepository>();
            services.AddScoped<IElectricityMeterDetailRepository, ElectricityMeterDetailRepository>();
            services.AddScoped<IWaterMeterRepository, WaterMeterRepository>();
            services.AddScoped<IWaterMeterDetailRepository, WaterMeterDetailRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSwagger();
            app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v2/swagger.json", "RMZ API Services"));

            //app.UseFileServer(new FileServerOptions
            //{
            //    FileProvider = new PhysicalFileProvider(
            //       Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles")),
            //    RequestPath = "/StaticFiles",
            //    EnableDefaultFiles=false
            //});
        }
    }
}
