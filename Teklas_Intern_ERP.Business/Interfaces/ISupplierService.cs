using Teklas_Intern_ERP.DTOs.PurchasingManagement;

namespace Teklas_Intern_ERP.Business.Interfaces
{
    public interface ISupplierService
    {
        Task<IEnumerable<SupplierDto>> GetAllAsync();
        Task<SupplierDto?> GetByIdAsync(long id);
        Task<SupplierDto> CreateAsync(SupplierDto dto);
        Task<SupplierDto> UpdateAsync(long id, SupplierDto dto);
        Task<bool> DeleteAsync(long id);
        Task<bool> RestoreAsync(long id);
        Task<IEnumerable<SupplierDto>> SearchAsync(string searchTerm);
        Task<IEnumerable<SupplierDto>> GetDeletedAsync();
        Task<bool> PermanentDeleteAsync(long id);
    }
} 