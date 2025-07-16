using Teklas_Intern_ERP.Entities.ProductionManagment;
using Teklas_Intern_ERP.DataAccess.Repositories;

namespace Teklas_Intern_ERP.DataAccess.ProductionManagement;

public class MaterialConsumptionRepository : BaseRepository<MaterialConsumption>, IMaterialConsumptionRepository
{
    public MaterialConsumptionRepository(AppDbContext context) : base(context) { }
} 