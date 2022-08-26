using AL.RMZ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AL.RMZ.Repository
{
    public interface IBuildingRepository
    {
        Task<int> DeleteBuilding(int? id);

        Task<List<Building>> GetBuildings();

        Task<Building> GetBuilding(int? BuildingId);

        Task<List<Building>> GetBuildingsByFacility(int? FacilityId);

        Task<int> AddBuilding(BuildingRequest addBuildingRequest);

        Task UpdateBuilding(int buildingId, BuildingRequest updateBuildingRequest);

    }
}
