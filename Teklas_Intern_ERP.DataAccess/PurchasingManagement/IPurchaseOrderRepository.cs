using Teklas_Intern_ERP.Entities.PurchasingManagement;
using Teklas_Intern_ERP.DataAccess.Repositories;

namespace Teklas_Intern_ERP.DataAccess.PurchasingManagement
{
    public interface IPurchaseOrderRepository : IRepository<PurchaseOrder>
    {
        Task<List<PurchaseOrder>> GetOrdersBySupplierAsync(long supplierId);
        Task<List<PurchaseOrder>> GetOrdersByStatusAsync(string status);
        Task<List<PurchaseOrder>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<bool> ExistsByOrderNumberAsync(string orderNumber, long excludeId = 0);
    }
} 