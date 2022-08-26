using AL.RMZ.Data;
using AL.RMZ.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AL.RMZ.Repository
{
    public class ElectricityMeterRepository : IElectricityMeterRepository
    {

        private readonly RMZAPIDbContext dBContext;
        public ElectricityMeterRepository(RMZAPIDbContext dbContext)
        {
            this.dBContext = dbContext;
        }

        public async Task<int> DeleteElectricityMeter(int? id)
        {
            int result = 0;

            var ElectricityMeter = await dBContext.ElectricityMeters.FindAsync(id);

            if (ElectricityMeter != null)
            {
                dBContext.ElectricityMeters.Remove(ElectricityMeter);

                result = await dBContext.SaveChangesAsync();
            }

            return result;
        }


        public async Task<List<ElectricityMeter>> GetElectricityMeters()
        {
            return await dBContext.ElectricityMeters.ToListAsync();
        }

        public async Task<ElectricityMeter> GetElectricityMeter(int? ElectricityMeterId)
        {
            return await dBContext.ElectricityMeters.Where(x => x.Id == ElectricityMeterId).FirstOrDefaultAsync();
        }

        public async Task<List<ElectricityMeter>> GetElectricityMetersByZone(int? ZoneId)
        {
            return await dBContext.ElectricityMeters.Where(x => x.ZoneId == ZoneId).ToListAsync();
        }

        public async Task<int> AddElectricityMeter(ElectricityMeterRequest addElectricityMeterRequest)
        {
            if (addElectricityMeterRequest.Number != default && addElectricityMeterRequest.ZoneId != default)
            {
                var existingElectricityMeter = await dBContext.ElectricityMeters.
                                                     Where(x => x.ZoneId == addElectricityMeterRequest.ZoneId && x.Number == addElectricityMeterRequest.Number).FirstOrDefaultAsync();

                if (existingElectricityMeter == default)
                {
                    ElectricityMeter ElectricityMeter = new ElectricityMeter()
                    {
                        Number = addElectricityMeterRequest.Number,
                        ZoneId = addElectricityMeterRequest.ZoneId,
                        CreatedById = 1

                    };
                    await dBContext.ElectricityMeters.AddAsync(ElectricityMeter);
                    await dBContext.SaveChangesAsync();
                    return ElectricityMeter.Id;
                }
                return existingElectricityMeter.Id;
            }
            return 0;
        }

        public async Task UpdateElectricityMeter(int ElectricityMeterId, ElectricityMeterRequest updateElectricityMeterRequest)
        {
            if (updateElectricityMeterRequest.Number != default && updateElectricityMeterRequest.ZoneId != default)
            {
                var ElectricityMeter = dBContext.ElectricityMeters.FindAsync(ElectricityMeterId);
                if (ElectricityMeter.Result != null)
                {
                    ElectricityMeter.Result.Number = updateElectricityMeterRequest.Number;
                    ElectricityMeter.Result.ZoneId = updateElectricityMeterRequest.ZoneId;
                    ElectricityMeter.Result.UpdatedById = 1;
                    ElectricityMeter.Result.UpdatedDate = DateTime.Now;
                    await dBContext.SaveChangesAsync();
                }
                else
                    throw new ArgumentNullException(Convert.ToString(ElectricityMeterId));
            }
            else
                throw new NullReferenceException(Convert.ToString(ElectricityMeterId));
        }
    }
}
