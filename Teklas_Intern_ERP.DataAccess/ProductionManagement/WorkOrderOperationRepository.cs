using Teklas_Intern_ERP.Entities.ProductionManagment;
using Teklas_Intern_ERP.DataAccess.Repositories;

namespace Teklas_Intern_ERP.DataAccess.ProductionManagement;

public class WorkOrderOperationRepository : BaseRepository<WorkOrderOperation>, IWorkOrderOperationRepository
{
    public WorkOrderOperationRepository(AppDbContext context) : base(context) { }
} 