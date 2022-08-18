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
    public class FloorController : Controller
    {
        private readonly RMZAPIDbContext dBContext;
        public FloorController(RMZAPIDbContext dBContext)
        {
            this.dBContext = dBContext;
        }
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteFloor(int id)
        {
            var Floor = await dBContext.Floors.FindAsync(id);

            if (Floor != null)
            {
                dBContext.Floors.Remove(Floor);

                await dBContext.SaveChangesAsync();
            }

            return NotFound();
        }

        [HttpGet]        
        public async Task<IActionResult> GetFloors(int? Id)
        {
            if (Id == null)
                return Ok(await dBContext.Floors.ToListAsync());
            return Ok(await dBContext.Floors.Where(x => x.Id == Id).ToListAsync());
        }

        [HttpGet]
        [Route("{buildingId:int}")]
        public async Task<IActionResult> GetFloorsByBuildingId(int? buildingid)
        {
            if (buildingid == null)
                return NotFound();
            return Ok(await dBContext.Floors.Where(x => x.BuildingId == buildingid).ToListAsync());
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateFloor(int id, FloorRequest updateFloorRequest)
        {
            var Floor = await dBContext.Floors.FindAsync(id);

            if (Floor != null)
            {
                Floor.Name = updateFloorRequest.Name;
                Floor.BuildingId = updateFloorRequest.BuildingId;
                Floor.UpdatedDate = DateTime.Now;

                await dBContext.SaveChangesAsync();
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> AddFloor(FloorRequest addFloorRequest)
        {
            var Floor = new Models.Floor
            {
                BuildingId = addFloorRequest.BuildingId,
                Name = addFloorRequest.Name
            };
            await dBContext.Floors.AddAsync(Floor);
            await dBContext.SaveChangesAsync();

            return Ok(Floor);
        }
    }
}
