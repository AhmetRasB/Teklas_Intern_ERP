using Teklas_Intern_ERP.Entities.PurchasingManagement;
using Teklas_Intern_ERP.DataAccess.Repositories;

namespace Teklas_Intern_ERP.DataAccess.PurchasingManagement
{
    public interface ISupplierTypeRepository : IRepository<SupplierType>
    {
        Task<List<SupplierType>> GetActiveSupplierTypesAsync();
        Task<bool> ExistsByNameAsync(string name, long excludeId = 0);
    }
} 