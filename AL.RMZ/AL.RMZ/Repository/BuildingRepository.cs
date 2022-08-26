using AL.RMZ.Data;
using AL.RMZ.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AL.RMZ.Repository
{
    public class BuildingRepository : IBuildingRepository
    {

        private readonly RMZAPIDbContext dBContext;
        public BuildingRepository(RMZAPIDbContext dbContext)
        {
            this.dBContext = dbContext;
        }

        public async Task<int> DeleteBuilding(int? id)
        {
            int result = 0;

            var Building = await dBContext.Buildings.FindAsync(id);

            if (Building != null)
            {
                dBContext.Buildings.Remove(Building);

                result = await dBContext.SaveChangesAsync();
            }

            return result;
        }


        public async Task<List<Building>> GetBuildings()
        {
            return await dBContext.Buildings.ToListAsync();
        }

        public async Task<Building> GetBuilding(int? BuildingId)
        {
            return await dBContext.Buildings.Where(x => x.Id == BuildingId).FirstOrDefaultAsync();
        }

        public async Task<List<Building>> GetBuildingsByFacility(int? FacilityId)
        {
            return await dBContext.Buildings.Where(x => x.FacilityId == FacilityId).ToListAsync();
        }

        public async Task<int> AddBuilding(BuildingRequest addBuildingRequest)
        {
            if (addBuildingRequest.Name != default && addBuildingRequest.FacilityId != default)
            {
                var existingBuilding = await dBContext.Buildings.Where(x => x.Name == addBuildingRequest.Name && x.FacilityId == addBuildingRequest.FacilityId).FirstOrDefaultAsync();
                if (existingBuilding == default)
                {

                    Building Building = new Building()
                    {
                        Name = addBuildingRequest.Name,
                        FacilityId = addBuildingRequest.FacilityId,
                        CreatedById = 1

                    };
                    await dBContext.Buildings.AddAsync(Building);
                    await dBContext.SaveChangesAsync();
                    return Building.Id;
                }
                return existingBuilding.Id;
            }
            return 0;
        }

        public async Task UpdateBuilding(int buildingId, BuildingRequest updateBuildingRequest)
        {
            if (updateBuildingRequest.Name != default && updateBuildingRequest.FacilityId != default)
            {
                var Building = dBContext.Buildings.FindAsync(buildingId);
                if (Building.Result != null)
                {
                    Building.Result.Name = updateBuildingRequest.Name;
                    Building.Result.FacilityId = updateBuildingRequest.FacilityId;
                    Building.Result.UpdatedById = 1;
                    Building.Result.UpdatedDate = DateTime.Now;
                    await dBContext.SaveChangesAsync();
                }
                else
                    throw new ArgumentNullException(Convert.ToString(buildingId));
            }
            else
                throw new NullReferenceException(Convert.ToString(buildingId));
        }
    }
}
