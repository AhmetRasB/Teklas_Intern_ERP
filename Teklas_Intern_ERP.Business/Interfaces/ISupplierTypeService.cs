using Teklas_Intern_ERP.DTOs.PurchasingManagement;

namespace Teklas_Intern_ERP.Business.Interfaces
{
    public interface ISupplierTypeService
    {
        Task<IEnumerable<SupplierTypeDto>> GetAllAsync();
        Task<SupplierTypeDto?> GetByIdAsync(long id);
        Task<SupplierTypeDto> CreateAsync(SupplierTypeDto dto);
        Task<SupplierTypeDto> UpdateAsync(long id, SupplierTypeDto dto);
        Task<bool> DeleteAsync(long id);
        Task<bool> RestoreAsync(long id);
        Task<IEnumerable<SupplierTypeDto>> SearchAsync(string searchTerm);
        Task<IEnumerable<SupplierTypeDto>> GetDeletedAsync();
        Task<bool> PermanentDeleteAsync(long id);
    }
} 