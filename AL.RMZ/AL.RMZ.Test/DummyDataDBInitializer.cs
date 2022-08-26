using AL.RMZ.Data;
using System;
using System.Linq;
using Xunit;

namespace AL.RMZ.Test
{
    public class DummyDataDBInitializer
    {
        public DummyDataDBInitializer()
        {
        }

        public void Seed(RMZAPIDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.Cities.AddRange(
                new Models.City { Name = "Bangalore" },
                  new Models.City { Name = "Mumbai" },
                    new Models.City { Name = "Chennai" }
                );
            context.SaveChanges();

            context.Facilities.AddRange(
                new Models.Facility { Name = "Majestic", City = context.Cities.Where(x => x.Name == "Bangalore").FirstOrDefault() },
                new Models.Facility { Name = "Carmelram", City = context.Cities.Where(x => x.Name == "Bangalore").FirstOrDefault() }
                );

            context.SaveChanges();

            context.Buildings.AddRange(
               new Models.Building { Name = "Building001", Facility = context.Facilities.Where(x => x.Name == "Carmelram").FirstOrDefault() },
               new Models.Building { Name = "Building002", Facility = context.Facilities.Where(x => x.Name == "Majestic").FirstOrDefault() }
               );

            context.SaveChanges();

            context.Floors.AddRange(
              new Models.Floor { Name = "Floor001", Building = context.Buildings.Where(x => x.Name == "Building001").FirstOrDefault() },
              new Models.Floor { Name = "Floor002", Building = context.Buildings.Where(x => x.Name == "Building002").FirstOrDefault() }
              );

            context.SaveChanges();

            context.Zones.AddRange(
              new Models.Zone { Name = "Zone001", Floor = context.Floors.Where(x => x.Name == "Floor001").FirstOrDefault() },
              new Models.Zone { Name = "Zone002", Floor = context.Floors.Where(x => x.Name == "Floor001").FirstOrDefault() },
              new Models.Zone { Name = "Zone003", Floor = context.Floors.Where(x => x.Name == "Floor002").FirstOrDefault() },
              new Models.Zone { Name = "Zone004", Floor = context.Floors.Where(x => x.Name == "Floor002").FirstOrDefault() }
              );

            context.SaveChanges();

            context.ElectricityMeters.AddRange(
             new Models.ElectricityMeter { Number = "ElectricityMeter001", Zone = context.Zones.Where(x => x.Name == "Zone001").FirstOrDefault() },
             new Models.ElectricityMeter { Number = "ElectricityMeter002", Zone = context.Zones.Where(x => x.Name == "Zone002").FirstOrDefault() },
             new Models.ElectricityMeter { Number = "ElectricityMeter003", Zone = context.Zones.Where(x => x.Name == "Zone003").FirstOrDefault() },
             new Models.ElectricityMeter { Number = "ElectricityMeter004", Zone = context.Zones.Where(x => x.Name == "Zone004").FirstOrDefault() }
             );

            context.SaveChanges();

            context.WaterMeters.AddRange(
             new Models.WaterMeter { Number = "WaterMeter001", Zone = context.Zones.Where(x => x.Name == "Zone001").FirstOrDefault() },
             new Models.WaterMeter { Number = "WaterMeter002", Zone = context.Zones.Where(x => x.Name == "Zone002").FirstOrDefault() },
             new Models.WaterMeter { Number = "WaterMeter003", Zone = context.Zones.Where(x => x.Name == "Zone003").FirstOrDefault() },
             new Models.WaterMeter { Number = "WaterMeter004", Zone = context.Zones.Where(x => x.Name == "Zone004").FirstOrDefault() }
             );

            context.SaveChanges();

            context.ElectricityMeterDetails.AddRange(
            new Models.ElectricityMeterDetail { ReadingDate = DateTime.Now.AddDays(-30), StartReading = 0, EndReading = 100, ElectricityMeter = context.ElectricityMeters.Where(x => x.Number == "ElectricityMeter001").FirstOrDefault() },
            new Models.ElectricityMeterDetail { ReadingDate = DateTime.Now.AddDays(-30), StartReading = 10, EndReading = 100, ElectricityMeter = context.ElectricityMeters.Where(x => x.Number == "ElectricityMeter002").FirstOrDefault() },
            new Models.ElectricityMeterDetail { ReadingDate = DateTime.Now.AddDays(-30), StartReading = 0, EndReading = 100, ElectricityMeter = context.ElectricityMeters.Where(x => x.Number == "ElectricityMeter003").FirstOrDefault() },
            new Models.ElectricityMeterDetail { ReadingDate = DateTime.Now.AddDays(-30), StartReading = 10, EndReading = 100, ElectricityMeter = context.ElectricityMeters.Where(x => x.Number == "ElectricityMeter004").FirstOrDefault() }
            );

            context.SaveChanges();

            context.WaterMeterDetails.AddRange(
            new Models.WaterMeterDetail { ReadingDate = DateTime.Now.AddDays(-30), StartReading = 0, EndReading = 100, WaterMeter = context.WaterMeters.Where(x => x.Number == "WaterMeter001").FirstOrDefault() },
            new Models.WaterMeterDetail { ReadingDate = DateTime.Now.AddDays(-30), StartReading = 10, EndReading = 100, WaterMeter = context.WaterMeters.Where(x => x.Number == "WaterMeter002").FirstOrDefault() },
            new Models.WaterMeterDetail { ReadingDate = DateTime.Now.AddDays(-30), StartReading = 0, EndReading = 100, WaterMeter = context.WaterMeters.Where(x => x.Number == "WaterMeter003").FirstOrDefault() },
            new Models.WaterMeterDetail { ReadingDate = DateTime.Now.AddDays(-30), StartReading = 10, EndReading = 100, WaterMeter = context.WaterMeters.Where(x => x.Number == "WaterMeter004").FirstOrDefault() }
            );

            context.SaveChanges();
        }
    }
}
