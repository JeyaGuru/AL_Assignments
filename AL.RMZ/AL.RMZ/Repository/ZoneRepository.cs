using AL.RMZ.Data;
using AL.RMZ.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AL.RMZ.Repository
{
    public class ZoneRepository : IZoneRepository
    {

        private readonly RMZAPIDbContext dBContext;
        public ZoneRepository(RMZAPIDbContext dbContext)
        {
            this.dBContext = dbContext;
        }

        public async Task<int> DeleteZone(int? id)
        {
            int result = 0;

            var Zone = await dBContext.Zones.FindAsync(id);

            if (Zone != null)
            {
                dBContext.Zones.Remove(Zone);

                result = await dBContext.SaveChangesAsync();
            }

            return result;
        }

        public async Task<List<Zone>> GetZones()
        {
            return await dBContext.Zones.ToListAsync();
        }

        public async Task<Zone> GetZone(int? ZoneId)
        {
            return await dBContext.Zones.Where(x => x.Id == ZoneId).FirstOrDefaultAsync();
        }

        public async Task<List<Zone>> GetZonesByFloor(int? FloorId)
        {
            return await dBContext.Zones.Where(x => x.FloorId == FloorId).ToListAsync();
        }

        public async Task<int> AddZone(ZoneRequest addZoneRequest)
        {
            if (addZoneRequest.Name != default && addZoneRequest.FloorId != default)
            {
                var existingZone = await dBContext.Zones.
                                                    Where(x => x.FloorId == addZoneRequest.FloorId && x.Name == addZoneRequest.Name).FirstOrDefaultAsync();

                if (existingZone == default)
                {
                    Zone Zone = new Zone()
                    {
                        Name = addZoneRequest.Name,
                        FloorId = addZoneRequest.FloorId,
                        CreatedById = 1

                    };
                    await dBContext.Zones.AddAsync(Zone);
                    await dBContext.SaveChangesAsync();
                    return Zone.Id;
                }
                return existingZone.Id;
            }
            return 0;
        }

        public async Task UpdateZone(int ZoneId, ZoneRequest updateZoneRequest)
        {
            if (ZoneId != default && updateZoneRequest.Name != default && updateZoneRequest.FloorId != default)
            {
                var Zone = dBContext.Zones.FindAsync(ZoneId);
                if (Zone.Result != null)
                {
                    Zone.Result.Name = updateZoneRequest.Name;
                    Zone.Result.FloorId = updateZoneRequest.FloorId;
                    Zone.Result.UpdatedById = 1;
                    Zone.Result.UpdatedDate = DateTime.Now;
                    await dBContext.SaveChangesAsync();
                }
                else
                    throw new ArgumentNullException(Convert.ToString(ZoneId));
            }
            else
                throw new NullReferenceException(Convert.ToString(ZoneId));
        }
    }
}
