using Teklas_Intern_ERP.Entities.ProductionManagment;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Teklas_Intern_ERP.DataAccess.Repositories;

public interface IBillOfMaterialRepository : IRepository<BOMHeader>
{
    Task<BOMHeader?> GetWithItemsAsync(long id);
    Task<List<BOMHeader>> GetAllWithItemsAsync();
    Task<BOMHeader?> GetByIdForDeleteAsync(long bomHeaderId);
    Task<List<BOMHeader>> GetDeletedAsync();
} 