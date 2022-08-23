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
    public class WaterMeterDetailController : Controller
    {
        private readonly RMZAPIDbContext dbContext;
        public WaterMeterDetailController(RMZAPIDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetWaterMeters(int? id)
        {
            if (id == null)
                return Ok(await dbContext.WaterMeterDetails.ToListAsync());
            return Ok(await dbContext.WaterMeterDetails.Where(x => x.Id == id).ToListAsync());
        }

        [HttpGet]
        [Route("{watermeterid:int}")]
        public async Task<IActionResult> GetWaterMeterByWaterMeterId(int? watermeterid)
        {
            if (watermeterid == null)
                return NotFound();
            return Ok(await dbContext.WaterMeterDetails.Where(x => x.WaterMeterId == watermeterid).ToListAsync());
        }

        [HttpGet]
        [Route("Search/")]
        public async Task<IActionResult> GetWaterMeterDetail(int? facilityid, int? buildingid, int? floorid, int? zoneid, int? watermeterid, DateTime? readingstartdate, DateTime? readingenddate)
        {

            return Ok(await
                (from facility in dbContext.Facilities
                 join building in dbContext.Buildings on facility.Id equals building.FacilityId
                 join floor in dbContext.Floors on building.Id equals floor.BuildingId
                 join zone in dbContext.Zones on floor.Id equals zone.FloorId
                 join waterMeter in dbContext.WaterMeters on zone.Id equals waterMeter.ZoneId
                 join waterMeterDetail in dbContext.WaterMeterDetails on waterMeter.Id equals waterMeterDetail.WaterMeterId
                 where ((readingstartdate == null || waterMeterDetail.ReadingDate.Date >= readingstartdate.Value.Date) && (readingenddate == null || waterMeterDetail.ReadingDate.Date <= readingenddate.Value.Date) && (watermeterid == default || waterMeterDetail.WaterMeterId == watermeterid) && (zoneid == default || zone.Id == zoneid) && (buildingid == default || building.Id == buildingid) && (floorid == default || floor.Id == floorid) && (facilityid == default || facility.Id == facilityid))
                 select new DisplayWaterMeterDetail { watermeter = waterMeter.Number, buildingname = building.Name, facilityname = facility.Name, startunit = waterMeterDetail.StartReading, endunit = waterMeterDetail.EndReading, zonename = zone.Name, readingdate = waterMeterDetail.ReadingDate, totalunits = waterMeterDetail.TotalUnits, id = waterMeterDetail.Id }).ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> AddWaterMeterDetail(WaterMeterDetailRequest WaterMeterDetailRequest)
        {
            if (WaterMeterDetailRequest == null)
                return NotFound();

            var WaterMeterDetail = new Models.WaterMeterDetail
            {
                ReadingDate = WaterMeterDetailRequest.ReadingDate,
                WaterMeterId = WaterMeterDetailRequest.WaterMeterId,
                StartReading = WaterMeterDetailRequest.StartReading,
                EndReading = WaterMeterDetailRequest.EndReading,
                CreatedById = 1
            };
            await dbContext.WaterMeterDetails.AddAsync(WaterMeterDetail);
            await dbContext.SaveChangesAsync();

            return Ok(WaterMeterDetail);
        }
    }
}
