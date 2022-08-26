using AL.RMZ.Data;
using AL.RMZ.Models;
using AL.RMZ.Repository;
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
    public class BuildingController : Controller
    {
        private readonly IBuildingRepository _buildingRepository;
        public BuildingController(IBuildingRepository buildingRepository)
        {
            this._buildingRepository = buildingRepository;
        }

        [HttpDelete]
        [Route("DeleteBuilding")]
        public async Task<IActionResult> DeleteBuilding(int? buildingid)
        {

            if (buildingid == default)
            {
                return BadRequest();
            }

            try
            {
                var result = await _buildingRepository.DeleteBuilding(buildingid);
                if (result != default)
                {
                    return Ok();
                }
                return NotFound();
            }
            catch (Exception) { return BadRequest(); }
        }

        [HttpGet]
        [Route("GetBuildings")]
        public async Task<IActionResult> GetBuildings()
        {
            try
            {
                var buildings = await _buildingRepository.GetBuildings();

                return Ok(buildings);
            }
            catch (Exception) { return BadRequest(); }
        }

        [HttpGet]
        [Route("GetBuildingsByFacility")]
        public async Task<IActionResult> GetBuildingsByFacility(int? FacilityId)
        {
            if (FacilityId == default)
            {
                return BadRequest();
            }
            try
            {
                var buildings = await _buildingRepository.GetBuildingsByFacility(FacilityId);
                if (buildings?.Any() == true)
                    return Ok(buildings);
                else
                    return NotFound();
            }
            catch (Exception) { return BadRequest(); }
        }

        [HttpGet]
        [Route("GetBuilding")]
        public async Task<IActionResult> GetBuilding(int? buildingId)
        {
            if (buildingId == default)
            {
                return BadRequest();
            }

            try
            {
                var Building = await _buildingRepository.GetBuilding(buildingId);

                if (Building?.Id != default)
                    return Ok(Building);
                else
                    return NotFound();
            }
            catch (Exception) { return BadRequest(); }
        }

        [HttpPost]
        [Route("AddBuilding")]
        public async Task<IActionResult> AddBuilding([FromBody] BuildingRequest model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var BuildingId = await _buildingRepository.AddBuilding(model);
                    if (BuildingId > 0)
                        return Ok(BuildingId);
                    else
                        return BadRequest();
                }
                catch (Exception)
                {

                    return BadRequest();
                }

            }
            return BadRequest();
        }

        [HttpPost]
        [Route("UpdateBuilding/{id:int}")]
        public async Task<IActionResult> UpdateBuilding(int id, [FromBody] BuildingRequest model)
        {
            if (ModelState.IsValid && id > 0)
            {
                try
                {
                    await _buildingRepository.UpdateBuilding(id, model);

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
            return BadRequest();
        }
    }
}
