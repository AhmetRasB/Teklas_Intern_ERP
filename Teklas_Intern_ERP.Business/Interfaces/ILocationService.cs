using System.Collections.Generic;
using System.Threading.Tasks;
using Teklas_Intern_ERP.DTOs.WarehouseManagement;

namespace Teklas_Intern_ERP.Business.Interfaces
{
    public interface ILocationService
    {
        #region Basic CRUD Operations
        Task<List<LocationDto>> GetAllAsync();
        Task<LocationDto?> GetByIdAsync(long id);
        Task<LocationDto> AddAsync(LocationDto dto);
        Task<LocationDto> UpdateAsync(LocationDto dto);
        Task<bool> DeleteAsync(long id);
        Task<List<LocationDto>> GetDeletedAsync();
        Task<bool> RestoreAsync(long id);
        Task<bool> PermanentDeleteAsync(long id);
        #endregion

        #region Search Operations
        Task<List<LocationDto>> GetLocationsByWarehouseAsync(long warehouseId);
        Task<List<LocationDto>> SearchLocationsAsync(string searchTerm);
        Task<bool> IsLocationCodeUniqueAsync(string code, long? excludeId = null);
        Task<List<LocationDto>> GetLocationsByTypeAsync(string locationType);
        #endregion
    }
} 