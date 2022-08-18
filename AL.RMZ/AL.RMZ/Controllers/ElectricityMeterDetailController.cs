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
                TotalUnits = (electricityMeterDetailRequest.EndReading - electricityMeterDetailRequest.StartReading)
            };
            await dbContext.ElectricityMeterDetails.AddAsync(electricityMeterDetail);
            await dbContext.SaveChangesAsync();

            return Ok(electricityMeterDetail);
        }
    }
}
