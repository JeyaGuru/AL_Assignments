using AL.RMZ.Data;
using AL.RMZ.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AL.RMZ.Repository
{
    public class FacilityRepository : IFacilityRepository
    {

        private readonly RMZAPIDbContext dBContext;
        public FacilityRepository(RMZAPIDbContext dbContext)
        {
            this.dBContext = dbContext;
        }

        public async Task<int> DeleteFacility(int? id)
        {
            int result = 0;

            var Facility = await dBContext.Facilities.FindAsync(id);

            if (Facility != null)
            {
                dBContext.Facilities.Remove(Facility);

                result = await dBContext.SaveChangesAsync();
            }

            return result;
        }


        public async Task<List<Facility>> GetFacilities()
        {
            return await dBContext.Facilities.ToListAsync();
        }

        public async Task<Facility> GetFacility(int? FacilityId)
        {
            return await dBContext.Facilities.Where(x => x.Id == FacilityId).FirstOrDefaultAsync();
        }

        public async Task<List<Facility>> GetFacilitiesByCity(int? CityId)
        {
            return await dBContext.Facilities.Where(x => x.CityId == CityId).ToListAsync();
        }

        public async Task<int> AddFacility(FacilityRequest addFacilityRequest)
        {
            if (addFacilityRequest.Name != default && addFacilityRequest.CityId != default)
            {
                var existingFacility = await dBContext.Facilities.
                                                     Where(x => x.CityId == addFacilityRequest.CityId && x.Name == addFacilityRequest.Name).FirstOrDefaultAsync();

                if (existingFacility == default)
                {
                    Facility Facility = new Facility()
                    {
                        Name = addFacilityRequest.Name,
                        CityId = addFacilityRequest.CityId,
                        CreatedById = 1

                    };
                    await dBContext.Facilities.AddAsync(Facility);
                    await dBContext.SaveChangesAsync();
                    return Facility.Id;
                }
                return existingFacility.Id;
            }
            return 0;
        }

        public async Task UpdateFacility(int FacilityId, FacilityRequest updateFacilityRequest)
        {
            if (updateFacilityRequest.Name != default && updateFacilityRequest.CityId != default)
            {
                var Facility = dBContext.Facilities.FindAsync(FacilityId);
                if (Facility.Result != null)
                {
                    Facility.Result.Name = updateFacilityRequest.Name;
                    Facility.Result.CityId = updateFacilityRequest.CityId;
                    Facility.Result.UpdatedById = 1;
                    Facility.Result.UpdatedDate = DateTime.Now;
                    await dBContext.SaveChangesAsync();
                }
                else
                    throw new ArgumentNullException(Convert.ToString(FacilityId));
            }
            else
                throw new NullReferenceException(Convert.ToString(FacilityId));
        }
    }
}
