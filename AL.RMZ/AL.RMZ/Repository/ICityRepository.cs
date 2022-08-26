using AL.RMZ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AL.RMZ.Repository
{
    public interface ICityRepository
    {
        Task<int> DeleteCity(int? id);

        Task<List<City>> GetCities();

        Task<City> GetCity(int? CityId);

        Task<int> AddCity(CityRequest addCityRequest);

        Task UpdateCity(int cityId, CityRequest updateCityRequest);

    }
}
