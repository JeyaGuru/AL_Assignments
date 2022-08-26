using AL.RMZ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AL.RMZ.Repository
{
    public interface IWaterMeterDetailRepository
    {
        Task<int> DeleteWaterMeterDetail(int? id);

        Task<List<WaterMeterDetail>> GetWaterMeterDetails();

        Task<WaterMeterDetail> GetWaterMeterDetail(int? WaterMeterDetailId);

        Task<List<WaterMeterDetail>> GetWaterMeterDetailsByWaterMeter(int? WaterMeterId);

        Task<int> AddWaterMeterDetail(WaterMeterDetailRequest addWaterMeterDetailRequest);

        Task UpdateWaterMeterDetail(int waterMeterDetailId, WaterMeterDetailRequest updateWaterMeterDetailRequest);

        Task<List<DisplayWaterMeterDetail>> GetWaterMeterDetailByParams(int? FacilityId, int? BuildingId, int? FloorId, int? ZoneId, int? WaterMeterId, DateTime? ReadingStartDate, DateTime? ReadingEndDate);
    }
}
