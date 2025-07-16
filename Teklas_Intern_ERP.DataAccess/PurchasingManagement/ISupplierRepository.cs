using Teklas_Intern_ERP.Entities.PurchasingManagement;
using Teklas_Intern_ERP.DataAccess.Repositories;

namespace Teklas_Intern_ERP.DataAccess.PurchasingManagement
{
    public interface ISupplierRepository : IRepository<Supplier>
    {
        Task<List<Supplier>> GetActiveSuppliersAsync();
        Task<List<Supplier>> GetSuppliersByTypeAsync(long supplierTypeId);
        Task<bool> ExistsByNameAsync(string name, long excludeId = 0);
        Task<bool> ExistsByTaxNumberAsync(string taxNumber, long excludeId = 0);
    }
} 