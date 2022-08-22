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
    public class ZoneController : Controller
    {
        private readonly RMZAPIDbContext dBContext;
        public ZoneController(RMZAPIDbContext dBContext)
        {
            this.dBContext = dBContext;
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteZone(int id)
        {
            var Zone = await dBContext.Zones.FindAsync(id);

            if (Zone != null)
            {
                dBContext.Zones.Remove(Zone);

                await dBContext.SaveChangesAsync();
            }

            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> GetZones(int? Id)
        {
            if (Id == null)
                return Ok(await dBContext.Zones.ToListAsync());
            return Ok(await dBContext.Zones.Where(x => x.Id == Id).ToListAsync());
        }

        [HttpGet]
        [Route("{floorId:int}")]
        public async Task<IActionResult> GetZonesByFloorId(int? floorid)
        {
            if (floorid == null)
                return NotFound();
            return Ok(await dBContext.Zones.Where(x => x.FloorId == floorid).ToListAsync());
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateZone(int id, ZoneRequest updateZoneRequest)
        {
            var Zone = await dBContext.Zones.FindAsync(id);

            if (Zone != null)
            {
                Zone.Name = updateZoneRequest.Name;
                Zone.FloorId = updateZoneRequest.FloorId;
                Zone.UpdatedDate = DateTime.Now;

                await dBContext.SaveChangesAsync();
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> AddZone(ZoneRequest addZoneRequest)
        {
            var Zone = new Models.Zone
            {
                Name = addZoneRequest.Name,
                FloorId = addZoneRequest.FloorId,
                CreatedById = 1
            };
            await dBContext.Zones.AddAsync(Zone);
            await dBContext.SaveChangesAsync();

            return Ok(Zone);
        }
    }
}
