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
    public class ElectricityMeterController : Controller
    {
        private readonly RMZAPIDbContext dBContext;
        public ElectricityMeterController(RMZAPIDbContext dBContext)
        {
            this.dBContext = dBContext;
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteElectricityMeter(int id)
        {
            var ElectricityMeter = await dBContext.ElectricityMeters.FindAsync(id);

            if (ElectricityMeter != null)
            {
                dBContext.ElectricityMeters.Remove(ElectricityMeter);

                await dBContext.SaveChangesAsync();
            }

            return NotFound();
        }

        [HttpGet]        
        public async Task<IActionResult> GetElectricityMeters(int? Id)
        {
            if (Id == null)
                return Ok(await dBContext.ElectricityMeters.ToListAsync());
            return Ok(await dBContext.ElectricityMeters.Where(x => x.Id == Id).ToListAsync());
        }

        [HttpGet]
        [Route("{zoneid:int}")]
        public async Task<IActionResult> GetElectricityMetersByZoneId(int? zoneid)
        {
            if (zoneid == null)
                return NotFound();
            return Ok(await dBContext.ElectricityMeters.Where(x => x.ZoneId == zoneid).ToListAsync());
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateElectricityMeter(int id, ElectricityMeterRequest updateElectricityMeterRequest)
        {
            var ElectricityMeter = await dBContext.ElectricityMeters.FindAsync(id);

            if (ElectricityMeter != null)
            {
                ElectricityMeter.Number = updateElectricityMeterRequest.Number;
                ElectricityMeter.ZoneId = updateElectricityMeterRequest.ZoneId;
                ElectricityMeter.UpdatedDate = DateTime.Now;

                await dBContext.SaveChangesAsync();
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> AddElectricityMeter(ElectricityMeterRequest addElectricityMeterRequest)
        {
            var electricityMeter = new Models.ElectricityMeter
            {
                Number = addElectricityMeterRequest.Number,                
                ZoneId = addElectricityMeterRequest.ZoneId,
                CreatedById = 1
            };
            await dBContext.ElectricityMeters.AddAsync(electricityMeter);
            await dBContext.SaveChangesAsync();

            return Ok(electricityMeter);
        }
    }
}
