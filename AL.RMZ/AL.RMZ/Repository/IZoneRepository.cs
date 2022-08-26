using AL.RMZ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AL.RMZ.Repository
{
    public interface IZoneRepository
    {
        Task<int> DeleteZone(int? id);

        Task<List<Zone>> GetZones();

        Task<Zone> GetZone(int? ZoneId);

        Task<List<Zone>> GetZonesByFloor(int? FloorId);

        Task<int> AddZone(ZoneRequest addZoneRequest);

        Task UpdateZone(int zoneId, ZoneRequest updateZoneRequest);

    }
}
