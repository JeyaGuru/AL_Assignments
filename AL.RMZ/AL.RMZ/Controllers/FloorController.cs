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
    public class FloorController : Controller
    {
        private readonly IFloorRepository _floorRepository;
        public FloorController(IFloorRepository floorRepository)
        {
            this._floorRepository = floorRepository;
        }

        [HttpDelete]
        [Route("DeleteFloor")]
        public async Task<IActionResult> DeleteFloor(int? floorid)
        {

            if (floorid == default)
            {
                return BadRequest();
            }

            try
            {
                var result = await _floorRepository.DeleteFloor(floorid);
                if (result != default)
                {
                    return Ok();
                }
                return NotFound();
            }
            catch (Exception) { return BadRequest(); }
        }

        [HttpGet]
        [Route("GetFloors")]
        public async Task<IActionResult> GetFloors()
        {
            try
            {
                var floors = await _floorRepository.GetFloors();

                return Ok(floors);
            }
            catch (Exception) { return BadRequest(); }
        }

        [HttpGet]
        [Route("GetFloorsByBuilding")]
        public async Task<IActionResult> GetFloorsByBuilding(int? BuildingId)
        {
            if (BuildingId == default)
            {
                return BadRequest();
            }
            try
            {
                var floors = await _floorRepository.GetFloorsByBuilding(BuildingId);
                if (floors?.Any() == true)
                    return Ok(floors);
                else
                    return NotFound();
            }
            catch (Exception) { return BadRequest(); }
        }

        [HttpGet]
        [Route("GetFloor")]
        public async Task<IActionResult> GetFloor(int? floorId)
        {
            if (floorId == default)
            {
                return BadRequest();
            }

            try
            {
                var Floor = await _floorRepository.GetFloor(floorId);

                if (Floor?.Id != default)
                    return Ok(Floor);
                else
                    return NotFound();
            }
            catch (Exception) { return BadRequest(); }
        }

        [HttpPost]
        [Route("AddFloor")]
        public async Task<IActionResult> AddFloor([FromBody] FloorRequest model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var FloorId = await _floorRepository.AddFloor(model);
                    if (FloorId > 0)
                        return Ok(FloorId);
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
        [Route("UpdateFloor/{id:int}")]
        public async Task<IActionResult> UpdateFloor(int id, [FromBody] FloorRequest model)
        {
            if (ModelState.IsValid && id > 0)
            {
                try
                {
                    await _floorRepository.UpdateFloor(id, model);

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
