using AL.RMZ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AL.RMZ.Repository
{
    public interface IElectricityMeterRepository
    {
        Task<int> DeleteElectricityMeter(int? id);

        Task<List<ElectricityMeter>> GetElectricityMeters();

        Task<ElectricityMeter> GetElectricityMeter(int? ElectricityMeterId);

        Task<List<ElectricityMeter>> GetElectricityMetersByZone(int? ZoneId);

        Task<int> AddElectricityMeter(ElectricityMeterRequest addElectricityMeterRequest);

        Task UpdateElectricityMeter(int electricityMeterId, ElectricityMeterRequest updateElectricityMeterRequest);

    }
}
