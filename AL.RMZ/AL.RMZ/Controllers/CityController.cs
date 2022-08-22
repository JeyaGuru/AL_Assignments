using AL.RMZ.Data;
using AL.RMZ.Models;
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
        private readonly RMZAPIDbContext dBContext;
        public CityController(RMZAPIDbContext dbContext)
        {
            this.dBContext = dbContext;
        }
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteCity(int id)
        {
            var city = await dBContext.Cities.FindAsync(id);

            if (city != null)
            {
                dBContext.Cities.Remove(city);

                await dBContext.SaveChangesAsync();
            }

            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> GetCities(int? Id)
        {
            dBContext.Cities.Add(new City { Id = 1, Name = "Bangalore" });
            if (Id == null)
                return Ok(await dBContext.Cities.ToListAsync());
            return Ok(await dBContext.Cities.Where(x => x.Id == Id).ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> AddCity(CityRequest addCityRequest)
        {
            City city = new City()
            {
                Name = addCityRequest.Name,
                CreatedById = 1

            };
            await dBContext.Cities.AddAsync(city);
            await dBContext.SaveChangesAsync();

            return Ok(city);
        }
    }
}
