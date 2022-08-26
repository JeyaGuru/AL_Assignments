using AL.RMZ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AL.RMZ.Repository
{
    public interface IElectricityMeterDetailRepository
    {
        Task<int> DeleteElectricityMeterDetail(int? id);

        Task<List<ElectricityMeterDetail>> GetElectricityMeterDetails();

        Task<ElectricityMeterDetail> GetElectricityMeterDetail(int? ElectricityMeterDetailId);

        Task<List<ElectricityMeterDetail>> GetElectricityMeterDetailsByElectricityMeter(int? ElectricityMeterId);

        Task<int> AddElectricityMeterDetail(ElectricityMeterDetailRequest addElectricityMeterDetailRequest);

        Task UpdateElectricityMeterDetail(int electricityMeterDetailId, ElectricityMeterDetailRequest updateElectricityMeterDetailRequest);

        Task<List<DisplayElectricityMeterDetail>> GetElectricityMeterDetailByParams(int? FacilityId, int? BuildingId, int? FloorId, int? ZoneId, int? ElectricityMeterId, DateTime? ReadingStartDate, DateTime? ReadingEndDate);
    }
}
