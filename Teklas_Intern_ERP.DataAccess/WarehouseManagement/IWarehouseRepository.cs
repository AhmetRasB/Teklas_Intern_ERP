using System.Collections.Generic;
using System.Threading.Tasks;
using Teklas_Intern_ERP.Entities.WarehouseManagement;
using Teklas_Intern_ERP.DataAccess.Repositories;

namespace Teklas_Intern_ERP.DataAccess.WarehouseManagement
{
    public interface IWarehouseRepository : IRepository<Warehouse>
    {
        Task<List<Warehouse>> GetWarehousesByTypeAsync(string warehouseType);
        Task<List<Warehouse>> SearchWarehousesAsync(string searchTerm);
        Task<bool> IsWarehouseCodeUniqueAsync(string code, long? excludeId = null);
    }
} 