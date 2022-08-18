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
                TotalUnits = ( WaterMeterDetailRequest.EndReading - WaterMeterDetailRequest.StartReading)
            };
            await dbContext.WaterMeterDetails.AddAsync(WaterMeterDetail);
            await dbContext.SaveChangesAsync();

            return Ok(WaterMeterDetail);
        }
    }
}
