using Teklas_Intern_ERP.Entities.SalesManagement;
using Teklas_Intern_ERP.DataAccess.Repositories;

namespace Teklas_Intern_ERP.DataAccess.SalesManagement
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<List<Customer>> GetActiveCustomersAsync();
        Task<bool> ExistsByNameAsync(string name, long excludeId = 0);
        Task<bool> ExistsByTaxNumberAsync(string taxNumber, long excludeId = 0);
    }
} 