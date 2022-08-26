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
    public class CityController : Controller
    {
        private readonly ICityRepository _cityRepository;
        public CityController(ICityRepository cityRepository)
        {
            this._cityRepository = cityRepository;
        }

        [HttpDelete]
        [Route("DeleteCity")]
        public async Task<IActionResult> DeleteCity(int? cityid)
        {
            int result = 0;

            if (cityid == default)
            {
                return BadRequest();
            }

            try
            {
                result = await _cityRepository.DeleteCity(cityid);
                if (result > 0)
                {
                    return Ok();
                }
                return NotFound();
            }
            catch (Exception) { return BadRequest(); }
        }

        [HttpGet]
        [Route("GetCities")]
        public async Task<IActionResult> GetCities()
        {
            try
            {
                var cities = await _cityRepository.GetCities();

                return Ok(cities);
            }
            catch (Exception) { return BadRequest(); }
        }

        [HttpGet]
        [Route("GetCity")]
        public async Task<IActionResult> GetCity(int? cityId)
        {
            if (cityId == default)
            {
                return BadRequest();
            }

            try
            {
                var City = await _cityRepository.GetCity(cityId);

                if (City?.Id != default)
                    return Ok(City);
                else
                    return NotFound();
            }
            catch (Exception) { return BadRequest(); }
        }

        [HttpPost]
        [Route("AddCity")]
        public async Task<IActionResult> AddCity([FromBody] CityRequest model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var CityId = await _cityRepository.AddCity(model);

                    return Ok(CityId);
                }
                catch (Exception)
                {

                    return BadRequest();
                }

            }
            return BadRequest();
        }

        [HttpPost]
        [Route("UpdateCity/{id:int}")]
        public async Task<IActionResult> UpdateCity(int id, [FromBody] CityRequest model)
        {
            if (ModelState.IsValid && id > 0)
            {
                try
                {
                    await _cityRepository.UpdateCity(id, model);

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
