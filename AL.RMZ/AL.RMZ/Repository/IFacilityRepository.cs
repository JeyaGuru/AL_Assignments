using AL.RMZ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AL.RMZ.Repository
{
    public interface IFacilityRepository
    {
        Task<int> DeleteFacility(int? id);

        Task<List<Facility>> GetFacilities();

        Task<Facility> GetFacility(int? FacilityId);

        Task<List<Facility>> GetFacilitiesByCity(int? CityId);

        Task<int> AddFacility(FacilityRequest addFacilityRequest);

        Task UpdateFacility(int facilityId, FacilityRequest updateFacilityRequest);

    }
}
