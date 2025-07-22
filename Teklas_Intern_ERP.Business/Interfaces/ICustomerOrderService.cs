using Teklas_Intern_ERP.DTOs.SalesManagement;

namespace Teklas_Intern_ERP.Business.Interfaces
{
    public interface ICustomerOrderService
    {
        Task<IEnumerable<CustomerOrderDto>> GetAllAsync();
        Task<CustomerOrderDto?> GetByIdAsync(long id);
        Task<CustomerOrderDto> CreateAsync(CustomerOrderDto dto);
        Task<CustomerOrderDto> UpdateAsync(long id, CustomerOrderDto dto);
        Task<bool> DeleteAsync(long id);
        Task<bool> RestoreAsync(long id);
        Task<IEnumerable<CustomerOrderDto>> SearchAsync(string searchTerm);
        Task<IEnumerable<CustomerOrderDto>> GetDeletedAsync();
        Task<bool> PermanentDeleteAsync(long id);
    }
} 