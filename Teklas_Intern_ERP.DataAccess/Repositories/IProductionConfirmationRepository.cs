using Teklas_Intern_ERP.Entities.ProductionManagment;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Teklas_Intern_ERP.DataAccess.Repositories;

public interface IProductionConfirmationRepository : IRepository<ProductionConfirmation>
{
    Task<ProductionConfirmation?> GetWithConsumptionsAsync(long id);
    Task<List<ProductionConfirmation>> GetAllWithConsumptionsAsync();
    Task<ProductionConfirmation?> GetByIdForDeleteAsync(long id);
    Task<ProductionConfirmation?> GetDeletedByIdAsync(long id);
    Task<List<ProductionConfirmation>> GetAllDeletedAsync();
} 