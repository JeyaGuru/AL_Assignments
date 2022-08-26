using AL.RMZ.Data;
using AL.RMZ.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AL.RMZ.Repository
{
    public class WaterMeterRepository : IWaterMeterRepository
    {

        private readonly RMZAPIDbContext dBContext;
        public WaterMeterRepository(RMZAPIDbContext dbContext)
        {
            this.dBContext = dbContext;
        }

        public async Task<int> DeleteWaterMeter(int? id)
        {
            int result = 0;

            var WaterMeter = await dBContext.WaterMeters.FindAsync(id);

            if (WaterMeter != null)
            {
                dBContext.WaterMeters.Remove(WaterMeter);

                result = await dBContext.SaveChangesAsync();
            }

            return result;
        }


        public async Task<List<WaterMeter>> GetWaterMeters()
        {
            return await dBContext.WaterMeters.ToListAsync();
        }

        public async Task<WaterMeter> GetWaterMeter(int? WaterMeterId)
        {
            return await dBContext.WaterMeters.Where(x => x.Id == WaterMeterId).FirstOrDefaultAsync();
        }

        public async Task<List<WaterMeter>> GetWaterMetersByZone(int? ZoneId)
        {
            return await dBContext.WaterMeters.Where(x => x.ZoneId == ZoneId).ToListAsync();
        }

        public async Task<int> AddWaterMeter(WaterMeterRequest addWaterMeterRequest)
        {
            if (addWaterMeterRequest.Number != default && addWaterMeterRequest.ZoneId != default)
            {
                var existingWaterMeter = await dBContext.WaterMeters.
                                                     Where(x => x.ZoneId == addWaterMeterRequest.ZoneId && x.Number == addWaterMeterRequest.Number).FirstOrDefaultAsync();

                if (existingWaterMeter == default)
                {
                    WaterMeter WaterMeter = new WaterMeter()
                    {
                        Number = addWaterMeterRequest.Number,
                        ZoneId = addWaterMeterRequest.ZoneId,
                        CreatedById = 1

                    };
                    await dBContext.WaterMeters.AddAsync(WaterMeter);
                    await dBContext.SaveChangesAsync();
                    return WaterMeter.Id;
                }
                return existingWaterMeter.Id;
            }
            return 0;
        }

        public async Task UpdateWaterMeter(int waterMeterId, WaterMeterRequest updateWaterMeterRequest)
        {
            if (updateWaterMeterRequest.Number != default && updateWaterMeterRequest.ZoneId != default)
            {
                var WaterMeter = dBContext.WaterMeters.FindAsync(waterMeterId);
                if (WaterMeter.Result != null)
                {
                    WaterMeter.Result.Number = updateWaterMeterRequest.Number;
                    WaterMeter.Result.ZoneId = updateWaterMeterRequest.ZoneId;
                    WaterMeter.Result.UpdatedById = 1;
                    WaterMeter.Result.UpdatedDate = DateTime.Now;
                    await dBContext.SaveChangesAsync();
                }
                else
                    throw new ArgumentNullException(Convert.ToString(waterMeterId));
            }
            else
                throw new NullReferenceException(Convert.ToString(waterMeterId));
        }
    }
}
