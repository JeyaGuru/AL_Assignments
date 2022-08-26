using AL.RMZ.Data;
using AL.RMZ.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AL.RMZ.Repository
{
    public class ElectricityMeterDetailRepository : IElectricityMeterDetailRepository
    {

        private readonly RMZAPIDbContext dBContext;
        public ElectricityMeterDetailRepository(RMZAPIDbContext dbContext)
        {
            this.dBContext = dbContext;
        }

        public async Task<int> DeleteElectricityMeterDetail(int? id)
        {
            int result = 0;

            var ElectricityMeterDetail = await dBContext.ElectricityMeterDetails.FindAsync(id);

            if (ElectricityMeterDetail != null)
            {
                dBContext.ElectricityMeterDetails.Remove(ElectricityMeterDetail);

                result = await dBContext.SaveChangesAsync();
            }

            return result;
        }


        public async Task<List<ElectricityMeterDetail>> GetElectricityMeterDetails()
        {
            return await dBContext.ElectricityMeterDetails.ToListAsync();
        }

        public async Task<ElectricityMeterDetail> GetElectricityMeterDetail(int? ElectricityMeterDetailId)
        {
            return await dBContext.ElectricityMeterDetails.Where(x => x.Id == ElectricityMeterDetailId).FirstOrDefaultAsync();
        }

        public async Task<List<ElectricityMeterDetail>> GetElectricityMeterDetailsByElectricityMeter(int? ElectricityMeterId)
        {
            return await dBContext.ElectricityMeterDetails.Where(x => x.ElectricityMeterId == ElectricityMeterId).ToListAsync();
        }

        public async Task<List<DisplayElectricityMeterDetail>> GetElectricityMeterDetailByParams(int? FacilityId, int? BuildingId, int? FloorId, int? ZoneId, int? ElectricityMeterId, DateTime? ReadingStartDate, DateTime? ReadingEndDate)
        {
            return await
                  (from facility in dBContext.Facilities
                   join building in dBContext.Buildings on facility.Id equals building.FacilityId
                   join floor in dBContext.Floors on building.Id equals floor.BuildingId
                   join zone in dBContext.Zones on floor.Id equals zone.FloorId
                   join electricityMeter in dBContext.ElectricityMeters on zone.Id equals electricityMeter.ZoneId
                   join electricityMeterDetail in dBContext.ElectricityMeterDetails on electricityMeter.Id equals electricityMeterDetail.ElectricityMeterId
                   where ((ReadingStartDate == null || electricityMeterDetail.ReadingDate.Date >= ReadingStartDate.Value.Date) && (ReadingEndDate == null || electricityMeterDetail.ReadingDate.Date <= ReadingEndDate.Value.Date) && (ElectricityMeterId == default || electricityMeterDetail.ElectricityMeterId == ElectricityMeterId) && (ZoneId == default || zone.Id == ZoneId) && (FloorId == default || floor.Id == FloorId) && (BuildingId == default || building.Id == BuildingId) && (FacilityId == default || facility.Id == FacilityId))
                   select new DisplayElectricityMeterDetail
                   {
                       electricitymeter = electricityMeter.Number,
                       buildingname = building.Name,
                       facilityname = facility.Name,
                       startunit = electricityMeterDetail.StartReading,
                       endunit = electricityMeterDetail.EndReading,
                       zonename = zone.Name,
                       readingdate = electricityMeterDetail.ReadingDate,
                       totalunits = electricityMeterDetail.TotalUnits,
                       id = electricityMeterDetail.Id
                   }).ToListAsync();
        }

        public async Task<int> AddElectricityMeterDetail(ElectricityMeterDetailRequest addElectricityMeterDetailRequest)
        {
            if (addElectricityMeterDetailRequest.ReadingDate != default && addElectricityMeterDetailRequest.StartReading < addElectricityMeterDetailRequest.EndReading && addElectricityMeterDetailRequest.ElectricityMeterId != default)
            {
                var existingElectricityMeterDetail = await dBContext.ElectricityMeterDetails.
                                                     Where(x => x.ReadingDate.ToShortDateString() == addElectricityMeterDetailRequest.ReadingDate.ToShortDateString() && addElectricityMeterDetailRequest.ElectricityMeterId == addElectricityMeterDetailRequest.ElectricityMeterId && addElectricityMeterDetailRequest.StartReading == addElectricityMeterDetailRequest.StartReading && x.EndReading == addElectricityMeterDetailRequest.EndReading).FirstOrDefaultAsync();

                if (existingElectricityMeterDetail == default)
                {
                    ElectricityMeterDetail ElectricityMeterDetail = new ElectricityMeterDetail()
                    {
                        ReadingDate = addElectricityMeterDetailRequest.ReadingDate,
                        ElectricityMeterId = addElectricityMeterDetailRequest.ElectricityMeterId,
                        StartReading = addElectricityMeterDetailRequest.StartReading,
                        EndReading = addElectricityMeterDetailRequest.EndReading,
                        CreatedById = 1

                    };
                    await dBContext.ElectricityMeterDetails.AddAsync(ElectricityMeterDetail);
                    await dBContext.SaveChangesAsync();
                    return ElectricityMeterDetail.Id;
                }
                return existingElectricityMeterDetail.Id;
            }
            return 0;
        }

        public async Task UpdateElectricityMeterDetail(int ElectricityMeterDetailRequestId, ElectricityMeterDetailRequest updateElectricityMeterDetailRequest)
        {
            if (updateElectricityMeterDetailRequest.ReadingDate != default && updateElectricityMeterDetailRequest.StartReading < updateElectricityMeterDetailRequest.EndReading && updateElectricityMeterDetailRequest.ElectricityMeterId != default)
            {
                var ElectricityMeterDetail = dBContext.ElectricityMeterDetails.FindAsync(ElectricityMeterDetailRequestId);
                if (ElectricityMeterDetail.Result != null)
                {
                    ElectricityMeterDetail.Result.ReadingDate = updateElectricityMeterDetailRequest.ReadingDate;
                    ElectricityMeterDetail.Result.ElectricityMeterId = updateElectricityMeterDetailRequest.ElectricityMeterId;
                    ElectricityMeterDetail.Result.StartReading = updateElectricityMeterDetailRequest.StartReading;
                    ElectricityMeterDetail.Result.EndReading = updateElectricityMeterDetailRequest.EndReading;
                    ElectricityMeterDetail.Result.UpdatedById = 1;
                    ElectricityMeterDetail.Result.UpdatedDate = DateTime.Now;
                    await dBContext.SaveChangesAsync();
                }
                else
                    throw new ArgumentNullException(Convert.ToString(ElectricityMeterDetailRequestId));
            }
            else
                throw new NullReferenceException(Convert.ToString(ElectricityMeterDetailRequestId));
        }
    }
}
