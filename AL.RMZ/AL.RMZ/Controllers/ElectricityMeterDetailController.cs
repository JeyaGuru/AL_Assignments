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
    public class ElectricityMeterDetailController : Controller
    {
        private readonly IElectricityMeterDetailRepository _electricityMeterDetailRepository;
        public ElectricityMeterDetailController(IElectricityMeterDetailRepository electricityMeterDetailRepository)
        {
            this._electricityMeterDetailRepository = electricityMeterDetailRepository;
        }

        [HttpDelete]
        [Route("DeleteElectricityMeterDetail")]
        public async Task<IActionResult> DeleteElectricityMeterDetail(int? electricityMeterDetailid)
        {

            if (electricityMeterDetailid == default)
            {
                return BadRequest();
            }

            try
            {
                var result = await _electricityMeterDetailRepository.DeleteElectricityMeterDetail(electricityMeterDetailid);
                if (result != default)
                {
                    return Ok();
                }
                return NotFound();
            }
            catch (Exception) { return BadRequest(); }
        }

        [HttpGet]
        [Route("GetElectricityMeterDetails")]
        public async Task<IActionResult> GetElectricityMeterDetails()
        {
            try
            {
                var electricityMeterDetails = await _electricityMeterDetailRepository.GetElectricityMeterDetails();

                return Ok(electricityMeterDetails);
            }
            catch (Exception) { return BadRequest(); }
        }

        [HttpGet]
        [Route("GetElectricityMeterDetailsByElectricityMeter")]
        public async Task<IActionResult> GetElectricityMeterDetailsByElectricityMeter(int? ElectricityMeterId)
        {
            if (ElectricityMeterId == default)
            {
                return BadRequest();
            }
            try
            {
                var electricityMeterDetails = await _electricityMeterDetailRepository.GetElectricityMeterDetailsByElectricityMeter(ElectricityMeterId);
                if (electricityMeterDetails?.Any() == true)
                    return Ok(electricityMeterDetails);
                else
                    return NotFound();
            }
            catch (Exception) { return BadRequest(); }
        }

        [HttpGet]
        [Route("GetElectricityMeterDetail")]
        public async Task<IActionResult> GetElectricityMeterDetail(int? electricityMeterDetailId)
        {
            if (electricityMeterDetailId == default)
            {
                return BadRequest();
            }

            try
            {
                var ElectricityMeterDetail = await _electricityMeterDetailRepository.GetElectricityMeterDetail(electricityMeterDetailId);

                if (ElectricityMeterDetail?.Id != default)
                    return Ok(ElectricityMeterDetail);
                else
                    return NotFound();
            }
            catch (Exception) { return BadRequest(); }
        }

        [HttpPost]
        [Route("AddElectricityMeterDetail")]
        public async Task<IActionResult> AddElectricityMeterDetail([FromBody] ElectricityMeterDetailRequest model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var ElectricityMeterDetailId = await _electricityMeterDetailRepository.AddElectricityMeterDetail(model);
                    if (ElectricityMeterDetailId > 0)
                        return Ok(ElectricityMeterDetailId);
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
        [Route("UpdateElectricityMeterDetail/{id:int}")]
        public async Task<IActionResult> UpdateElectricityMeterDetail(int id, [FromBody] ElectricityMeterDetailRequest model)
        {
            if (ModelState.IsValid && id > 0)
            {
                try
                {
                    await _electricityMeterDetailRepository.UpdateElectricityMeterDetail(id, model);

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

        [HttpGet]
        [Route("GetElectricityMeterDetailByParams")]
        public async Task<IActionResult> GetElectricityMeterDetailByParams(int? facilityid, int? buildingid, int? floorid, int? zoneid, int? electricitymeterid, DateTime? readingstartdate, DateTime? readingenddate)
        {
            try
            {
                var electricityMeterDetails = await _electricityMeterDetailRepository.GetElectricityMeterDetailByParams(facilityid, buildingid, floorid, zoneid, electricitymeterid, readingstartdate, readingenddate);
                return Ok(electricityMeterDetails);
            }
            catch (Exception) { return BadRequest(); }
        }
    }
}
