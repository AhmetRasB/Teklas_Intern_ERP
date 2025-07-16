using Teklas_Intern_ERP.Entities.SalesManagement;
using Teklas_Intern_ERP.DataAccess.Repositories;

namespace Teklas_Intern_ERP.DataAccess.SalesManagement
{
    public interface ICustomerOrderRepository : IRepository<CustomerOrder>
    {
        Task<List<CustomerOrder>> GetOrdersByCustomerAsync(long customerId);
        Task<List<CustomerOrder>> GetOrdersByStatusAsync(string status);
        Task<List<CustomerOrder>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<bool> ExistsByOrderNumberAsync(string orderNumber, long excludeId = 0);
    }
} 