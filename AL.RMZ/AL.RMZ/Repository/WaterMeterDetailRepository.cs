using AL.RMZ.Data;
using AL.RMZ.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AL.RMZ.Repository
{
    public class WaterMeterDetailRepository : IWaterMeterDetailRepository
    {

        private readonly RMZAPIDbContext dBContext;
        public WaterMeterDetailRepository(RMZAPIDbContext dbContext)
        {
            this.dBContext = dbContext;
        }

        public async Task<int> DeleteWaterMeterDetail(int? id)
        {
            int result = 0;

            var WaterMeterDetail = await dBContext.WaterMeterDetails.FindAsync(id);

            if (WaterMeterDetail != null)
            {
                dBContext.WaterMeterDetails.Remove(WaterMeterDetail);

                result = await dBContext.SaveChangesAsync();
            }

            return result;
        }


        public async Task<List<WaterMeterDetail>> GetWaterMeterDetails()
        {
            return await dBContext.WaterMeterDetails.ToListAsync();
        }

        public async Task<WaterMeterDetail> GetWaterMeterDetail(int? WaterMeterDetailId)
        {
            return await dBContext.WaterMeterDetails.Where(x => x.Id == WaterMeterDetailId).FirstOrDefaultAsync();
        }

        public async Task<List<WaterMeterDetail>> GetWaterMeterDetailsByWaterMeter(int? WaterMeterId)
        {
            return await dBContext.WaterMeterDetails.Where(x => x.WaterMeterId == WaterMeterId).ToListAsync();
        }

        public async Task<List<DisplayWaterMeterDetail>> GetWaterMeterDetailByParams(int? FacilityId, int? BuildingId, int? FloorId, int? ZoneId, int? WaterMeterId, DateTime? ReadingStartDate, DateTime? ReadingEndDate)
        {
            return await
                  (from facility in dBContext.Facilities
                   join building in dBContext.Buildings on facility.Id equals building.FacilityId
                   join floor in dBContext.Floors on building.Id equals floor.BuildingId
                   join zone in dBContext.Zones on floor.Id equals zone.FloorId
                   join waterMeter in dBContext.WaterMeters on zone.Id equals waterMeter.ZoneId
                   join waterMeterDetail in dBContext.WaterMeterDetails on waterMeter.Id equals waterMeterDetail.WaterMeterId
                   where ((ReadingStartDate == null || waterMeterDetail.ReadingDate.Date >= ReadingStartDate.Value.Date) && (ReadingEndDate == null || waterMeterDetail.ReadingDate.Date <= ReadingEndDate.Value.Date) && (WaterMeterId == default || waterMeterDetail.WaterMeterId == WaterMeterId) && (ZoneId == default || zone.Id == ZoneId) && (FloorId == default || floor.Id == FloorId) && (BuildingId == default || building.Id == BuildingId) && (FacilityId == default || facility.Id == FacilityId))
                   select new DisplayWaterMeterDetail
                   {
                       watermeter = waterMeter.Number,
                       buildingname = building.Name,
                       facilityname = facility.Name,
                       startunit = waterMeterDetail.StartReading,
                       endunit = waterMeterDetail.EndReading,
                       zonename = zone.Name,
                       readingdate = waterMeterDetail.ReadingDate,
                       totalunits = waterMeterDetail.TotalUnits,
                       id = waterMeterDetail.Id
                   }).ToListAsync();
        }

        public async Task<int> AddWaterMeterDetail(WaterMeterDetailRequest addWaterMeterDetailRequest)
        {
            if (addWaterMeterDetailRequest.ReadingDate != default && addWaterMeterDetailRequest.StartReading < addWaterMeterDetailRequest.EndReading && addWaterMeterDetailRequest.WaterMeterId != default)
            {
                var existingWaterMeterDetail = await dBContext.WaterMeterDetails.
                                                     Where(x => x.ReadingDate.ToShortDateString() == addWaterMeterDetailRequest.ReadingDate.ToShortDateString() && addWaterMeterDetailRequest.WaterMeterId == addWaterMeterDetailRequest.WaterMeterId && addWaterMeterDetailRequest.StartReading == addWaterMeterDetailRequest.StartReading && x.EndReading == addWaterMeterDetailRequest.EndReading).FirstOrDefaultAsync();

                if (existingWaterMeterDetail == default)
                {
                    WaterMeterDetail WaterMeterDetail = new WaterMeterDetail()
                    {
                        ReadingDate = addWaterMeterDetailRequest.ReadingDate,
                        WaterMeterId = addWaterMeterDetailRequest.WaterMeterId,
                        StartReading = addWaterMeterDetailRequest.StartReading,
                        EndReading = addWaterMeterDetailRequest.EndReading,
                        CreatedById = 1

                    };
                    await dBContext.WaterMeterDetails.AddAsync(WaterMeterDetail);
                    await dBContext.SaveChangesAsync();
                    return WaterMeterDetail.Id;
                }
                return existingWaterMeterDetail.Id;
            }
            return 0;
        }

        public async Task UpdateWaterMeterDetail(int waterMeterDetailId, WaterMeterDetailRequest updateWaterMeterDetailRequest)
        {
            if (updateWaterMeterDetailRequest.ReadingDate != default && updateWaterMeterDetailRequest.StartReading < updateWaterMeterDetailRequest.EndReading && updateWaterMeterDetailRequest.WaterMeterId != default)
            {
                var WaterMeterDetail = dBContext.WaterMeterDetails.FindAsync(waterMeterDetailId);
                if (WaterMeterDetail.Result != null)
                {
                    WaterMeterDetail.Result.ReadingDate = updateWaterMeterDetailRequest.ReadingDate;
                    WaterMeterDetail.Result.WaterMeterId = updateWaterMeterDetailRequest.WaterMeterId;
                    WaterMeterDetail.Result.StartReading = updateWaterMeterDetailRequest.StartReading;
                    WaterMeterDetail.Result.EndReading = updateWaterMeterDetailRequest.EndReading;
                    WaterMeterDetail.Result.UpdatedById = 1;
                    WaterMeterDetail.Result.UpdatedDate = DateTime.Now;
                    await dBContext.SaveChangesAsync();
                }
                else
                    throw new ArgumentNullException(Convert.ToString(waterMeterDetailId));
            }
            else
                throw new NullReferenceException(Convert.ToString(waterMeterDetailId));
        }
    }
}
