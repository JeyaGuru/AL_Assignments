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
    public class BuildingController : Controller
    {
        private readonly RMZAPIDbContext dBContext;
        public BuildingController(RMZAPIDbContext dBContext)
        {
            this.dBContext = dBContext;
        }

        [HttpPost]
        public async Task<IActionResult> AddBuilding(BuildingRequest addBuildingRequest)
        {
            var building = new Models.Building
            {
                Name = addBuildingRequest.Name,
                FacilityId = addBuildingRequest.FacilityId,
                CreatedById=1
                //FacilityId = dBContext.Facilities.Where(x=>x.Name == addBuildingRequest.FacilityName).FirstOrDefault().Id              

            };
            await dBContext.Buildings.AddAsync(building);
            await dBContext.SaveChangesAsync();

            return Ok(building);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateBuilding(int id, BuildingRequest updateBuildingRequest)
        {
            var building = await dBContext.Buildings.FindAsync(id);

            if (building != null)
            {
                building.Name = updateBuildingRequest.Name;
                building.FacilityId = updateBuildingRequest.FacilityId;
                building.UpdatedDate = DateTime.Now;

                await dBContext.SaveChangesAsync();
            }

            return NotFound();
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteBuilding(int id)
        {
            var building = await dBContext.Buildings.FindAsync(id);

            if (building != null)
            {
                dBContext.Buildings.Remove(building);

                await dBContext.SaveChangesAsync();
            }

            return NotFound();
        }

        [HttpGet]     
        public async Task<IActionResult> GetBuildings(int? id)
        {
            if (id == null)
                return Ok(await dBContext.Buildings.ToListAsync());
            return Ok(await dBContext.Buildings.Where(x => x.Id == id).ToListAsync());
        }

        [HttpGet]
        [Route("{facilityid:int}")]
        public async Task<IActionResult> GetBuildingsByFacilityId(int? facilityid)
        {
            if (facilityid == null)
                return NotFound();
            return Ok(await dBContext.Buildings.Where(x => x.FacilityId == facilityid).ToListAsync());
        }
    }
}
