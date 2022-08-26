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
    public class ElectricityMeterController : Controller
    {
        private readonly IElectricityMeterRepository _electricityMeterRepository;
        public ElectricityMeterController(IElectricityMeterRepository electricityMeterRepository)
        {
            this._electricityMeterRepository = electricityMeterRepository;
        }

        [HttpDelete]
        [Route("DeleteElectricityMeter")]
        public async Task<IActionResult> DeleteElectricityMeter(int? electricityMeterid)
        {

            if (electricityMeterid == default)
            {
                return BadRequest();
            }

            try
            {
                var result = await _electricityMeterRepository.DeleteElectricityMeter(electricityMeterid);
                if (result != default)
                {
                    return Ok();
                }
                return NotFound();
            }
            catch (Exception) { return BadRequest(); }
        }

        [HttpGet]
        [Route("GetElectricityMeters")]
        public async Task<IActionResult> GetElectricityMeters()
        {
            try
            {
                var electricityMeters = await _electricityMeterRepository.GetElectricityMeters();

                return Ok(electricityMeters);
            }
            catch (Exception) { return BadRequest(); }
        }

        [HttpGet]
        [Route("GetElectricityMetersByZone")]
        public async Task<IActionResult> GetElectricityMetersByZone(int? ZoneId)
        {
            if (ZoneId == default)
            {
                return BadRequest();
            }
            try
            {
                var electricityMeters = await _electricityMeterRepository.GetElectricityMetersByZone(ZoneId);
                if (electricityMeters?.Any() == true)
                    return Ok(electricityMeters);
                else
                    return NotFound();
            }
            catch (Exception) { return BadRequest(); }
        }

        [HttpGet]
        [Route("GetElectricityMeter")]
        public async Task<IActionResult> GetElectricityMeter(int? electricityMeterId)
        {
            if (electricityMeterId == default)
            {
                return BadRequest();
            }

            try
            {
                var ElectricityMeter = await _electricityMeterRepository.GetElectricityMeter(electricityMeterId);

                if (ElectricityMeter?.Id != default)
                    return Ok(ElectricityMeter);
                else
                    return NotFound();
            }
            catch (Exception) { return BadRequest(); }
        }

        [HttpPost]
        [Route("AddElectricityMeter")]
        public async Task<IActionResult> AddElectricityMeter([FromBody] ElectricityMeterRequest model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var ElectricityMeterId = await _electricityMeterRepository.AddElectricityMeter(model);
                    if (ElectricityMeterId > 0)
                        return Ok(ElectricityMeterId);
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
        [Route("UpdateElectricityMeter/{id:int}")]
        public async Task<IActionResult> UpdateElectricityMeter(int id, [FromBody] ElectricityMeterRequest model)
        {
            if (ModelState.IsValid && id > 0)
            {
                try
                {
                    await _electricityMeterRepository.UpdateElectricityMeter(id, model);

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
