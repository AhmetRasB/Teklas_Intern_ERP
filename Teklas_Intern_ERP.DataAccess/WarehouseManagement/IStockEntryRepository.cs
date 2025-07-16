using System.Collections.Generic;
using System.Threading.Tasks;
using Teklas_Intern_ERP.Entities.WarehouseManagement;
using Teklas_Intern_ERP.DataAccess.Repositories;

namespace Teklas_Intern_ERP.DataAccess.WarehouseManagement
{
    public interface IStockEntryRepository : IRepository<StockEntry>
    {
        Task<List<StockEntry>> GetStockEntriesByWarehouseAsync(long warehouseId);
        Task<List<StockEntry>> GetStockEntriesByLocationAsync(long locationId);
        Task<List<StockEntry>> GetStockEntriesByMaterialAsync(long materialId);
        Task<List<StockEntry>> GetStockEntriesWithDetailsAsync();
        Task<List<StockEntry>> GetStockEntriesByTypeAsync(string entryType);
        Task<List<StockEntry>> SearchStockEntriesAsync(string searchTerm);
        Task<bool> IsEntryNumberUniqueAsync(string entryNumber, long? excludeId = null);
    }
} 