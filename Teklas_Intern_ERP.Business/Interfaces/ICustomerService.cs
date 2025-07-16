using Teklas_Intern_ERP.DTOs.SalesManagement;

namespace Teklas_Intern_ERP.Business.Interfaces
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerDto>> GetAllAsync();
        Task<CustomerDto?> GetByIdAsync(long id);
        Task<CustomerDto> CreateAsync(CustomerDto dto);
        Task<CustomerDto> UpdateAsync(long id, CustomerDto dto);
        Task<bool> DeleteAsync(long id);
        Task<bool> RestoreAsync(long id);
        Task<IEnumerable<CustomerDto>> GetDeletedAsync();
        Task<bool> PermanentDeleteAsync(long id);
        Task<IEnumerable<CustomerDto>> SearchAsync(string searchTerm);
    }
} 