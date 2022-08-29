using AL.RMZ.Data;
using AL.RMZ.Models;
using AL.RMZ.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
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
    [Authorize]
    public class FacilityController : Controller
    {
        private readonly IFacilityRepository _facilityRepository;
        public FacilityController(IFacilityRepository facilityRepository)
        {
            this._facilityRepository = facilityRepository;
        }

        [HttpDelete]
        [Route("DeleteFacility")]
        public async Task<IActionResult> DeleteFacility(int? facilityid)
        {

            if (facilityid == default)
            {
                return BadRequest();
            }

            try
            {
                var result = await _facilityRepository.DeleteFacility(facilityid);
                if (result != default)
                {
                    return Ok();
                }
                return NotFound();
            }
            catch (Exception) { return BadRequest(); }
        }

        [HttpGet]
        [Route("GetFacilities")]
        public async Task<IActionResult> GetFacilities()
        {
            try
            {
                var facilities = await _facilityRepository.GetFacilities();

                return Ok(facilities);
            }
            catch (Exception) { return BadRequest(); }
        }

        [HttpGet]
        [Route("GetFacilitiesByCity")]
        public async Task<IActionResult> GetFacilitiesByCity(int? CityId)
        {
            if (CityId == default)
            {
                return BadRequest();
            }
            try
            {
                var facilities = await _facilityRepository.GetFacilitiesByCity(CityId);
                if (facilities?.Any() == true)
                    return Ok(facilities);
                else
                    return NotFound();
            }
            catch (Exception) { return BadRequest(); }
        }

        [HttpGet]
        [Route("GetFacility")]
        public async Task<IActionResult> GetFacility(int? facilityId)
        {
            if (facilityId == default)
            {
                return BadRequest();
            }

            try
            {
                var Facility = await _facilityRepository.GetFacility(facilityId);

                if (Facility?.Id != default)
                    return Ok(Facility);
                else
                    return NotFound();
            }
            catch (Exception) { return BadRequest(); }
        }

        [HttpPost]
        [Route("AddFacility")]
        public async Task<IActionResult> AddFacility([FromBody] FacilityRequest model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var FacilityId = await _facilityRepository.AddFacility(model);
                    if (FacilityId > 0)
                        return Ok(FacilityId);
                    else
                        return BadRequest();
                }
                catch (Exception)
                {

                    return BadRequest();
                }

            }
            return BadRequest(ModelState);
        }

        [HttpPost]
        [Route("UpdateFacility/{id:int}")]
        public async Task<IActionResult> UpdateFacility(int id, [FromBody] FacilityRequest model)
        {
            if (ModelState.IsValid && id > 0)
            {
                try
                {
                    await _facilityRepository.UpdateFacility(id, model);

                    return Ok();
                }
                catch (ArgumentNullException ex)
                {
                    return NotFound(ex.Message);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest(ModelState);
        }
    }
}
