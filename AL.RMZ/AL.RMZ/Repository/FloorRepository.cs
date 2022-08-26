using AL.RMZ.Data;
using AL.RMZ.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AL.RMZ.Repository
{
    public class FloorRepository : IFloorRepository
    {

        private readonly RMZAPIDbContext dBContext;
        public FloorRepository(RMZAPIDbContext dbContext)
        {
            this.dBContext = dbContext;
        }

        public async Task<int> DeleteFloor(int? id)
        {
            int result = 0;

            var Floor = await dBContext.Floors.FindAsync(id);

            if (Floor != null)
            {
                dBContext.Floors.Remove(Floor);

                result = await dBContext.SaveChangesAsync();
            }

            return result;
        }


        public async Task<List<Floor>> GetFloors()
        {
            return await dBContext.Floors.ToListAsync();
        }

        public async Task<Floor> GetFloor(int? FloorId)
        {
            return await dBContext.Floors.Where(x => x.Id == FloorId).FirstOrDefaultAsync();
        }

        public async Task<List<Floor>> GetFloorsByBuilding(int? BuildingId)
        {
            return await dBContext.Floors.Where(x => x.BuildingId == BuildingId).ToListAsync();
        }

        public async Task<int> AddFloor(FloorRequest addFloorRequest)
        {
            if (addFloorRequest.Name != default && addFloorRequest.BuildingId != default)
            {
                var existingFloor = await dBContext.Floors.
                                                    Where(x => x.BuildingId == addFloorRequest.BuildingId && x.Name == addFloorRequest.Name).FirstOrDefaultAsync();

                if (existingFloor == default)
                {
                    Floor Floor = new Floor()
                    {
                        Name = addFloorRequest.Name,
                        BuildingId = addFloorRequest.BuildingId,
                        CreatedById = 1

                    };
                    await dBContext.Floors.AddAsync(Floor);
                    await dBContext.SaveChangesAsync();
                    return Floor.Id;
                }
                return existingFloor.Id;
            }
            return 0;
        }

        public async Task UpdateFloor(int floorId, FloorRequest updateFloorRequest)
        {
            if (updateFloorRequest.Name != default && updateFloorRequest.BuildingId != default)
            {
                var Floor = dBContext.Floors.FindAsync(floorId);
                if (Floor.Result != null)
                {
                    Floor.Result.Name = updateFloorRequest.Name;
                    Floor.Result.BuildingId = updateFloorRequest.BuildingId;
                    Floor.Result.UpdatedById = 1;
                    Floor.Result.UpdatedDate = DateTime.Now;
                    await dBContext.SaveChangesAsync();
                }
                else
                    throw new ArgumentNullException(Convert.ToString(floorId));
            }
            else
                throw new NullReferenceException(Convert.ToString(floorId));
        }
    }
}
