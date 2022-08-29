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
    public class WaterMeterDetailController : Controller
    {
        private readonly IWaterMeterDetailRepository _waterMeterDetailRepository;
        public WaterMeterDetailController(IWaterMeterDetailRepository waterMeterDetailRepository)
        {
            this._waterMeterDetailRepository = waterMeterDetailRepository;
        }

        [HttpDelete]
        [Route("DeleteWaterMeterDetail")]
        public async Task<IActionResult> DeleteWaterMeterDetail(int? waterMeterDetailid)
        {

            if (waterMeterDetailid == default)
            {
                return BadRequest();
            }

            try
            {
                var result = await _waterMeterDetailRepository.DeleteWaterMeterDetail(waterMeterDetailid);
                if (result != default)
                {
                    return Ok();
                }
                return NotFound();
            }
            catch (Exception) { return BadRequest(); }
        }

        [HttpGet]
        [Route("GetWaterMeterDetails")]
        public async Task<IActionResult> GetWaterMeterDetails()
        {
            try
            {
                var waterMeterDetails = await _waterMeterDetailRepository.GetWaterMeterDetails();

                return Ok(waterMeterDetails);
            }
            catch (Exception) { return BadRequest(); }
        }

        [HttpGet]
        [Route("GetWaterMeterDetailsByWaterMeter")]
        public async Task<IActionResult> GetWaterMeterDetailsByWaterMeter(int? WaterMeterId)
        {
            if (WaterMeterId == default)
            {
                return BadRequest();
            }
            try
            {
                var waterMeterDetails = await _waterMeterDetailRepository.GetWaterMeterDetailsByWaterMeter(WaterMeterId);
                if (waterMeterDetails?.Any() == true)
                    return Ok(waterMeterDetails);
                else
                    return NotFound();
            }
            catch (Exception) { return BadRequest(); }
        }

        [HttpGet]
        [Route("GetWaterMeterDetail")]
        public async Task<IActionResult> GetWaterMeterDetail(int? waterMeterDetailId)
        {
            if (waterMeterDetailId == default)
            {
                return BadRequest();
            }

            try
            {
                var WaterMeterDetail = await _waterMeterDetailRepository.GetWaterMeterDetail(waterMeterDetailId);

                if (WaterMeterDetail?.Id != default)
                    return Ok(WaterMeterDetail);
                else
                    return NotFound();
            }
            catch (Exception) { return BadRequest(); }
        }

        [HttpPost]
        [Route("AddWaterMeterDetail")]
        public async Task<IActionResult> AddWaterMeterDetail([FromBody] WaterMeterDetailRequest model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var WaterMeterDetailId = await _waterMeterDetailRepository.AddWaterMeterDetail(model);
                    if (WaterMeterDetailId > 0)
                        return Ok(WaterMeterDetailId);
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
        [Route("UpdateWaterMeterDetail:{id:int}")]
        public async Task<IActionResult> UpdateWaterMeterDetail(int waterMeterId, [FromBody] WaterMeterDetailRequest model)
        {
            if (ModelState.IsValid && waterMeterId > 0)
            {
                try
                {
                    await _waterMeterDetailRepository.UpdateWaterMeterDetail(waterMeterId, model);

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
        [Route("GetWaterMeterDetailByParams")]
        public async Task<IActionResult> GetWaterMeterDetailByParams(int? facilityid, int? buildingid, int? floorid, int? zoneid, int? watermeterid, DateTime? readingstartdate, DateTime? readingenddate)
        {
            try
            {
                var waterMeterDetails = await _waterMeterDetailRepository.GetWaterMeterDetailByParams(facilityid, buildingid, floorid, zoneid, watermeterid, readingstartdate, readingenddate);
                return Ok(waterMeterDetails);

            }
            catch (Exception) { return BadRequest(); }
        }
    }
}
