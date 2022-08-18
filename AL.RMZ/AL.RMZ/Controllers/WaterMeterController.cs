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
    public class WaterMeterController : Controller
    {
        private readonly RMZAPIDbContext dBContext;
        public WaterMeterController(RMZAPIDbContext dBContext)
        {
            this.dBContext = dBContext;
        }
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteWaterMeter(int id)
        {
            var WaterMeter = await dBContext.WaterMeters.FindAsync(id);

            if (WaterMeter != null)
            {
                dBContext.WaterMeters.Remove(WaterMeter);

                await dBContext.SaveChangesAsync();
            }

            return NotFound();
        }

        [HttpGet]      
        public async Task<IActionResult> GetWaterMeters(int? Id)
        {
            if (Id == null)
                return Ok(await dBContext.WaterMeters.ToListAsync());
            return Ok(await dBContext.WaterMeters.Where(x => x.Id == Id).ToListAsync());
        }

        [HttpGet]
        [Route("{zoneid:int}")]
        public async Task<IActionResult> GetWaterMetersByZoneId(int? zoneid)
        {
            if (zoneid == null)
                return NotFound();
            return Ok(await dBContext.WaterMeters.Where(x => x.ZoneId == zoneid).ToListAsync());
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateWaterMeter(int id, WaterMeterRequest updateWaterMeterRequest)
        {
            var WaterMeter = await dBContext.WaterMeters.FindAsync(id);

            if (WaterMeter != null)
            {
                WaterMeter.Number = updateWaterMeterRequest.Number;
                WaterMeter.ZoneId = updateWaterMeterRequest.ZoneId;
                WaterMeter.UpdatedDate = DateTime.Now;

                await dBContext.SaveChangesAsync();
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> AddWaterMeter(WaterMeterRequest addWaterMeterRequest)
        {
            var WaterMeter = new Models.WaterMeter
            {
                Number = addWaterMeterRequest.Number,
                //ReadingDate = addWaterMeterRequest.ReadingDate,
                //StartReading = addWaterMeterRequest.StartReading,
                //EndReading = addWaterMeterRequest.EndReading,
                //CityId = addWaterMeterRequest.CityId,
                //FacilityId = addWaterMeterRequest.FacilityId,
                //BuildingId = addWaterMeterRequest.BuildingId,
                //FloorId = addWaterMeterRequest.FloorId,
                //ZoneId = dBContext.Zones.Where(x => x.Name == addWaterMeterRequest.ZoneName).FirstOrDefault().Id
                ZoneId = addWaterMeterRequest.ZoneId
            };
            await dBContext.WaterMeters.AddAsync(WaterMeter);
            await dBContext.SaveChangesAsync();

            return Ok(WaterMeter);
        }
    }
}
