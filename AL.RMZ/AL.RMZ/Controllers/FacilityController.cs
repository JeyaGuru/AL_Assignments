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
    public class FacilityController : Controller
    {
        private readonly RMZAPIDbContext dBContext;
        public FacilityController(RMZAPIDbContext dBContext)
        {
            this.dBContext = dBContext;
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteFacility(int id)
        {
            var Facility = await dBContext.Facilities.FindAsync(id);

            if (Facility != null)
            {
                dBContext.Facilities.Remove(Facility);

                await dBContext.SaveChangesAsync();
            }

            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> GetFacilities(int? Id)
        {
            if (Id == null)
                return Ok(await dBContext.Facilities.ToListAsync());
            return Ok(await dBContext.Facilities.Where(x => x.Id == Id).ToListAsync());
        }

        [HttpGet]
        [Route("{cityid:int}")]
        public async Task<IActionResult> GetFacilitiesByCityId(int? cityid)
        {
            if (cityid == null)
                return NotFound();
            return Ok(await dBContext.Facilities.Where(x => x.CityId == cityid).ToListAsync());
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateFacility(int id, FacilityRequest updateFacilityRequest)
        {
            var Facility = await dBContext.Facilities.FindAsync(id);

            if (Facility != null)
            {
                Facility.Name = updateFacilityRequest.Name;
                Facility.CityId = updateFacilityRequest.CityId;
                Facility.UpdatedDate = DateTime.Now;

                await dBContext.SaveChangesAsync();
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> AddFacility(FacilityRequest addFacilityRequest)
        {
            var Facility = new Models.Facility
            {
                Name = addFacilityRequest.Name,
                CityId = addFacilityRequest.CityId
            };
            await dBContext.Facilities.AddAsync(Facility);
            await dBContext.SaveChangesAsync();

            return Ok(Facility);
        }
    }
}
