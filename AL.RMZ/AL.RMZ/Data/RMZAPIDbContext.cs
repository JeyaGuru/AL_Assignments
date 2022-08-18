using AL.RMZ.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace AL.RMZ.Data
{
    public class RMZAPIDbContext : DbContext
    {
        public RMZAPIDbContext([NotNullAttribute] DbContextOptions options) : base(options)
        {
        }

        public DbSet<City> Cities { get; set; }
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<Floor> Floors { get; set; }
        public DbSet<Zone> Zones { get; set; }
        public DbSet<ElectricityMeter> ElectricityMeters { get; set; }
        public DbSet<WaterMeter> WaterMeters { get; set; }
        public DbSet<ElectricityMeterDetail> ElectricityMeterDetails { get; set; }
        public DbSet<WaterMeterDetail> WaterMeterDetails { get; set; }
    }
}
