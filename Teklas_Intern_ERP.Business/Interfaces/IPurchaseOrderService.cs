using Teklas_Intern_ERP.DTOs.PurchasingManagement;

namespace Teklas_Intern_ERP.Business.Interfaces
{
    public interface IPurchaseOrderService
    {
        Task<IEnumerable<PurchaseOrderDto>> GetAllAsync();
        Task<PurchaseOrderDto?> GetByIdAsync(long id);
        Task<PurchaseOrderDto> CreateAsync(PurchaseOrderDto dto);
        Task<PurchaseOrderDto> UpdateAsync(long id, PurchaseOrderDto dto);
        Task<bool> DeleteAsync(long id);
        Task<bool> RestoreAsync(long id);
        Task<IEnumerable<PurchaseOrderDto>> SearchAsync(string searchTerm);
        Task<IEnumerable<PurchaseOrderDto>> GetDeletedAsync();
        Task<bool> PermanentDeleteAsync(long id);
    }
} 