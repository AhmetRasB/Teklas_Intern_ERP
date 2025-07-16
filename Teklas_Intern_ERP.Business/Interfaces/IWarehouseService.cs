using System.Collections.Generic;
using System.Threading.Tasks;
using Teklas_Intern_ERP.DTOs.WarehouseManagement;

namespace Teklas_Intern_ERP.Business.Interfaces
{
    public interface IWarehouseService
    {
        #region Basic CRUD Operations
        Task<List<WarehouseDto>> GetAllAsync();
        Task<WarehouseDto?> GetByIdAsync(long id);
        Task<WarehouseDto> AddAsync(WarehouseDto dto);
        Task<WarehouseDto> UpdateAsync(WarehouseDto dto);
        Task<bool> DeleteAsync(long id);
        Task<List<WarehouseDto>> GetDeletedAsync();
        Task<bool> RestoreAsync(long id);
        Task<bool> PermanentDeleteAsync(long id);
        #endregion

        #region Search Operations
        Task<List<WarehouseDto>> SearchWarehousesAsync(string searchTerm);
        Task<bool> IsWarehouseCodeUniqueAsync(string code, long? excludeId = null);
        Task<List<WarehouseDto>> GetWarehousesByTypeAsync(string warehouseType);
        #endregion
    }
} 