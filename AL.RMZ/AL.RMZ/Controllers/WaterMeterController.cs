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
    public class WaterMeterController : Controller
    {
        private readonly IWaterMeterRepository _waterMeterRepository;
        public WaterMeterController(IWaterMeterRepository waterMeterRepository)
        {
            this._waterMeterRepository = waterMeterRepository;
        }

        [HttpDelete]
        [Route("DeleteWaterMeter")]
        public async Task<IActionResult> DeleteWaterMeter(int? waterMeterid)
        {

            if (waterMeterid == default)
            {
                return BadRequest();
            }

            try
            {
                var result = await _waterMeterRepository.DeleteWaterMeter(waterMeterid);
                if (result != default)
                {
                    return Ok();
                }
                return NotFound();
            }
            catch (Exception) { return BadRequest(); }
        }

        [HttpGet]
        [Route("GetWaterMeters")]
        public async Task<IActionResult> GetWaterMeters()
        {
            try
            {
                var waterMeters = await _waterMeterRepository.GetWaterMeters();

                return Ok(waterMeters);
            }
            catch (Exception) { return BadRequest(); }
        }

        [HttpGet]
        [Route("GetWaterMetersByZone")]
        public async Task<IActionResult> GetWaterMetersByZone(int? ZoneId)
        {
            if (ZoneId == default)
            {
                return BadRequest();
            }
            try
            {
                var waterMeters = await _waterMeterRepository.GetWaterMetersByZone(ZoneId);
                if (waterMeters?.Any() == true)
                    return Ok(waterMeters);
                else
                    return NotFound();
            }
            catch (Exception) { return BadRequest(); }
        }

        [HttpGet]
        [Route("GetWaterMeter")]
        public async Task<IActionResult> GetWaterMeter(int? waterMeterId)
        {
            if (waterMeterId == default)
            {
                return BadRequest();
            }

            try
            {
                var WaterMeter = await _waterMeterRepository.GetWaterMeter(waterMeterId);

                if (WaterMeter?.Id != default)
                    return Ok(WaterMeter);
                else
                    return NotFound();
            }
            catch (Exception) { return BadRequest(); }
        }

        [HttpPost]
        [Route("AddWaterMeter")]
        public async Task<IActionResult> AddWaterMeter([FromBody] WaterMeterRequest model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var WaterMeterId = await _waterMeterRepository.AddWaterMeter(model);
                    if (WaterMeterId > 0)
                        return Ok(WaterMeterId);
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
        [Route("UpdateWaterMeter/{id:int}")]
        public async Task<IActionResult> UpdateWaterMeter(int id, [FromBody] WaterMeterRequest model)
        {
            if (ModelState.IsValid && id > 0)
            {
                try
                {
                    await _waterMeterRepository.UpdateWaterMeter(id, model);

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
