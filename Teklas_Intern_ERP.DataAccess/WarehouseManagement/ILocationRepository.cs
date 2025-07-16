using System.Collections.Generic;
using System.Threading.Tasks;
using Teklas_Intern_ERP.Entities.WarehouseManagement;
using Teklas_Intern_ERP.DataAccess.Repositories;

namespace Teklas_Intern_ERP.DataAccess.WarehouseManagement
{
    public interface ILocationRepository : IRepository<Location>
    {
        Task<List<Location>> GetLocationsByWarehouseAsync(long warehouseId);
        Task<List<Location>> GetLocationsWithWarehouseAsync();
        Task<List<Location>> SearchLocationsAsync(string searchTerm);
        Task<bool> IsLocationCodeUniqueAsync(string code, long? excludeId = null);
        Task<List<Location>> GetLocationsByTypeAsync(string locationType);
    }
} 