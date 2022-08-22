using AL.RMZ.Data;
using AL.RMZ.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AL.RMZ.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ElectricityMeterDetailController : Controller
    {
        private readonly RMZAPIDbContext dbContext;
        public ElectricityMeterDetailController(RMZAPIDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetElectricityMeters(int? id)
        {
            if (id == null)
                return Ok(await dbContext.ElectricityMeterDetails.ToListAsync());
            return Ok(await dbContext.ElectricityMeterDetails.Where(x => x.Id == id).ToListAsync());
        }

        [HttpGet]
        [Route("{electricitymeterid:int}")]
        public async Task<IActionResult> GetElectricityMeterByElectricityMeterId(int? electricitymeterid)
        {
            if (electricitymeterid == null)
                return NotFound();
            return Ok(await dbContext.ElectricityMeterDetails.Where(x => x.ElectricityMeterId == electricitymeterid).ToListAsync());
        }

        [HttpGet]
        [Route("Search/")]
        public async Task<IActionResult> GetElectricityMeterDetail(int? facilityid, int? buildingid, int? zoneid, int? electricitymeterid, DateTime? readingstartdate, DateTime? readingenddate)
        {

            return Ok(await
                (from facility in dbContext.Facilities
                 join building in dbContext.Buildings on facility.Id equals building.FacilityId
                 join floor in dbContext.Floors on building.Id equals floor.BuildingId
                 join zone in dbContext.Zones on floor.Id equals zone.FloorId
                 join electricityMeter in dbContext.ElectricityMeters on zone.Id equals electricityMeter.ZoneId
                 join electricityMeterDetail in dbContext.ElectricityMeterDetails on electricityMeter.Id equals electricityMeterDetail.ElectricityMeterId
                 where ((readingstartdate == null || electricityMeterDetail.ReadingDate.Date >= readingstartdate.Value.Date) && (readingenddate == null || electricityMeterDetail.ReadingDate.Date <= readingenddate.Value.Date) && (electricitymeterid == default || electricityMeterDetail.ElectricityMeterId == electricitymeterid) && (zoneid == default || zone.Id == zoneid) && (buildingid == default || building.Id == buildingid) && (facilityid == default || facility.Id == facilityid))
                 select new DisplayElectricityMeterDetail { electricitymeter = electricityMeter.Number, buildingname = building.Name, facilityname = facility.Name, startunit = electricityMeterDetail.StartReading, endunit = electricityMeterDetail.EndReading, zonename = zone.Name, readingdate = electricityMeterDetail.ReadingDate, totalunits = electricityMeterDetail.TotalUnits, id = electricityMeterDetail.Id }).ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> AddElectricityMeterDetail(ElectricityMeterDetailRequest electricityMeterDetailRequest)
        {
            if (electricityMeterDetailRequest == null)
                return NotFound();

            var electricityMeterDetail = new Models.ElectricityMeterDetail
            {
                ReadingDate = electricityMeterDetailRequest.ReadingDate,
                ElectricityMeterId = electricityMeterDetailRequest.ElectricityMeterId,
                StartReading = electricityMeterDetailRequest.StartReading,
                EndReading = electricityMeterDetailRequest.EndReading,
                CreatedById = 1
            };
            await dbContext.ElectricityMeterDetails.AddAsync(electricityMeterDetail);
            await dbContext.SaveChangesAsync();

            return Ok(electricityMeterDetail);
        }
    }
}
