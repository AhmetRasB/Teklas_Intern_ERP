using Teklas_Intern_ERP.Entities.ProductionManagment;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Teklas_Intern_ERP.DataAccess.Repositories;

public interface IWorkOrderRepository : IRepository<WorkOrder>
{
    Task<WorkOrder?> GetWithOperationsAsync(long id);
    Task<List<WorkOrder>> GetAllWithOperationsAsync();
    Task<WorkOrder?> GetByIdForDeleteAsync(long workOrderId);
    Task<List<WorkOrder>> GetDeletedAsync();
} 