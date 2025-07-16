using Teklas_Intern_ERP.Entities.ProductionManagment;
using Teklas_Intern_ERP.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Teklas_Intern_ERP.DataAccess.ProductionManagement;

public class WorkOrderRepository : BaseRepository<WorkOrder>, IWorkOrderRepository
{
    public WorkOrderRepository(AppDbContext context) : base(context) { }

    public async Task<WorkOrder?> GetWithOperationsAsync(long id)
    {
        return await _context.WorkOrders
            .Include(w => w.Operations)
            .FirstOrDefaultAsync(w => w.WorkOrderId == id && !w.IsDeleted);
    }

    public async Task<List<WorkOrder>> GetAllWithOperationsAsync()
    {
        return await _context.WorkOrders
            .Include(w => w.Operations)
            .Where(w => !w.IsDeleted)
            .ToListAsync();
    }

    public async Task<WorkOrder?> GetByIdForDeleteAsync(long workOrderId)
    {
        return await _context.WorkOrders
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(w => w.WorkOrderId == workOrderId);
    }

    public async Task<List<WorkOrder>> GetDeletedAsync()
    {
        return await _context.WorkOrders
            .IgnoreQueryFilters()
            .Include(w => w.Operations)
            .Where(w => w.IsDeleted)
            .ToListAsync();
    }
} 