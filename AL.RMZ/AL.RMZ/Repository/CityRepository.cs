using AL.RMZ.Data;
using AL.RMZ.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AL.RMZ.Repository
{
    public class CityRepository : ICityRepository
    {

        private readonly RMZAPIDbContext dBContext;
        public CityRepository(RMZAPIDbContext dbContext)
        {
            this.dBContext = dbContext;
        }

        public async Task<int> DeleteCity(int? id)
        {
            int result = 0;

            var city = await dBContext.Cities.FindAsync(id);

            if (city != null)
            {
                dBContext.Cities.Remove(city);

                result = await dBContext.SaveChangesAsync();
            }

            return result;
        }


        public async Task<List<City>> GetCities()
        {
            return await dBContext.Cities.ToListAsync();
        }

        public async Task<City> GetCity(int? CityId)
        {
            return await dBContext.Cities.Where(x => x.Id == CityId).FirstOrDefaultAsync();
        }


        public async Task<int> AddCity(CityRequest addCityRequest)
        {
            City city = new City()
            {
                Name = addCityRequest.Name,
                CreatedById = 1

            };
            var existingCity = await dBContext.Cities.Where(x => x.Name == addCityRequest.Name).FirstOrDefaultAsync();
            if (existingCity == default)
            {
                await dBContext.Cities.AddAsync(city);
                await dBContext.SaveChangesAsync();
                return city.Id;
            }
            return existingCity.Id;
        }

        public async Task UpdateCity(int cityId, CityRequest updateCityRequest)
        {
            if (updateCityRequest.Name != default)
            {
                var city = dBContext.Cities.FindAsync(cityId);
                if (city.Result != null)
                {
                    city.Result.Name = updateCityRequest.Name;
                    city.Result.UpdatedById = 1;
                    city.Result.UpdatedDate = DateTime.Now;
                    await dBContext.SaveChangesAsync();
                }
                else
                    throw new ArgumentNullException(Convert.ToString(cityId));
            }
            else
                throw new NullReferenceException(Convert.ToString(cityId));
        }
    }
}
