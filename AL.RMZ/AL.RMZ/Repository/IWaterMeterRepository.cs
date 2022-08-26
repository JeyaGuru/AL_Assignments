using AL.RMZ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AL.RMZ.Repository
{
    public interface IWaterMeterRepository
    {
        Task<int> DeleteWaterMeter(int? id);

        Task<List<WaterMeter>> GetWaterMeters();

        Task<WaterMeter> GetWaterMeter(int? WaterMeterId);

        Task<List<WaterMeter>> GetWaterMetersByZone(int? ZoneId);

        Task<int> AddWaterMeter(WaterMeterRequest addWaterMeterRequest);

        Task UpdateWaterMeter(int WaterMeterId, WaterMeterRequest updateWaterMeterRequest);

    }
}
