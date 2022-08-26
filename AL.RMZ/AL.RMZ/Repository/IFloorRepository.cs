using AL.RMZ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AL.RMZ.Repository
{
    public interface IFloorRepository
    {
        Task<int> DeleteFloor(int? id);

        Task<List<Floor>> GetFloors();

        Task<Floor> GetFloor(int? FloorId);

        Task<List<Floor>> GetFloorsByBuilding(int? BuildingId);

        Task<int> AddFloor(FloorRequest addFloorRequest);

        Task UpdateFloor(int floorId, FloorRequest updateFloorRequest);

    }
}
