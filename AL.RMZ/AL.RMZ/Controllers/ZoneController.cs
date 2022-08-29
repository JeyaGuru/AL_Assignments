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
    public class ZoneController : Controller
    {
        private readonly IZoneRepository _zoneRepository;
        public ZoneController(IZoneRepository zoneRepository)
        {
            this._zoneRepository = zoneRepository;
        }

        [HttpDelete]
        [Route("DeleteZone")]
        public async Task<IActionResult> DeleteZone(int? zoneid)
        {

            if (zoneid == default)
            {
                return BadRequest();
            }

            try
            {
                var result = await _zoneRepository.DeleteZone(zoneid);
                if (result != default)
                {
                    return Ok();
                }
                return NotFound();
            }
            catch (Exception) { return BadRequest(); }
        }

        [HttpGet]
        [Route("GetZones")]
        public async Task<IActionResult> GetZones()
        {
            try
            {
                var zones = await _zoneRepository.GetZones();

                return Ok(zones);
            }
            catch (Exception) { return BadRequest(); }
        }

        [HttpGet]
        [Route("GetZonesByFloor")]
        public async Task<IActionResult> GetZonesByFloor(int? FloorId)
        {
            if (FloorId == default)
            {
                return BadRequest();
            }
            try
            {
                var zones = await _zoneRepository.GetZonesByFloor(FloorId);
                if (zones?.Any() == true)
                    return Ok(zones);
                else
                    return NotFound();
            }
            catch (Exception) { return BadRequest(); }
        }

        [HttpGet]
        [Route("GetZone")]
        public async Task<IActionResult> GetZone(int? zoneId)
        {
            if (zoneId == default)
            {
                return BadRequest();
            }

            try
            {
                var Zone = await _zoneRepository.GetZone(zoneId);

                if (Zone?.Id != default)
                    return Ok(Zone);
                else
                    return NotFound();
            }
            catch (Exception) { return BadRequest(); }
        }

        [HttpPost]
        [Route("AddZone")]
        public async Task<IActionResult> AddZone([FromBody] ZoneRequest model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var ZoneId = await _zoneRepository.AddZone(model);
                    if (ZoneId > 0)
                        return Ok(ZoneId);
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
        [Route("UpdateZone/{id:int}")]
        public async Task<IActionResult> UpdateZone(int id, [FromBody] ZoneRequest model)
        {
            if (ModelState.IsValid && id > 0)
            {
                try
                {
                    await _zoneRepository.UpdateZone(id, model);

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
