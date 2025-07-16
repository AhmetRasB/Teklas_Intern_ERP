using System.Collections.Generic;
using System.Threading.Tasks;
using Teklas_Intern_ERP.DTOs.WarehouseManagement;

namespace Teklas_Intern_ERP.Business.Interfaces
{
    public interface IStockEntryService
    {
        #region Basic CRUD Operations
        Task<List<StockEntryDto>> GetAllAsync();
        Task<StockEntryDto?> GetByIdAsync(long id);
        Task<StockEntryDto> AddAsync(StockEntryDto dto);
        Task<StockEntryDto> UpdateAsync(StockEntryDto dto);
        Task<bool> DeleteAsync(long id);
        Task<List<StockEntryDto>> GetDeletedAsync();
        Task<bool> RestoreAsync(long id);
        Task<bool> PermanentDeleteAsync(long id);
        #endregion

        #region Search Operations
        Task<List<StockEntryDto>> GetStockEntriesByWarehouseAsync(long warehouseId);
        Task<List<StockEntryDto>> GetStockEntriesByLocationAsync(long locationId);
        Task<List<StockEntryDto>> GetStockEntriesByMaterialAsync(long materialId);
        Task<List<StockEntryDto>> GetStockEntriesByTypeAsync(string entryType);
        Task<List<StockEntryDto>> SearchStockEntriesAsync(string searchTerm);
        Task<bool> IsEntryNumberUniqueAsync(string entryNumber, long? excludeId = null);
        #endregion
    }
} 