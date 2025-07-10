using Microsoft.EntityFrameworkCore;
using Teklas_Intern_ERP.Entities.ProductionManagement;
using Teklas_Intern_ERP.DataAccess.Repositories;

namespace Teklas_Intern_ERP.DataAccess.ProductionManagement
{
    /// <summary>
    /// Work Order Repository Implementation
    /// </summary>
    public sealed class WorkOrderRepository : BaseRepository<WorkOrder>, IWorkOrderRepository
    {
        public WorkOrderRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<WorkOrder?> GetByNumberAsync(string workOrderNumber)
        {
            return await _dbSet
                .Include(wo => wo.BillOfMaterial)
                    .ThenInclude(bom => bom.ProductMaterialCard)
                .Include(wo => wo.ProductMaterialCard)
                .Include(wo => wo.SupervisorUser)
                .Include(wo => wo.ReleasedByUser)
                .FirstOrDefaultAsync(wo => wo.WorkOrderNumber == workOrderNumber);
        }

        public async Task<IEnumerable<WorkOrder>> GetByBOMAsync(long billOfMaterialId)
        {
            return await _dbSet
                .Include(wo => wo.BillOfMaterial)
                .Include(wo => wo.ProductMaterialCard)
                .Include(wo => wo.SupervisorUser)
                .Where(wo => wo.BillOfMaterialId == billOfMaterialId)
                .OrderByDescending(wo => wo.CreateDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkOrder>> GetByProductMaterialCardAsync(long productMaterialCardId)
        {
            return await _dbSet
                .Include(wo => wo.BillOfMaterial)
                .Include(wo => wo.ProductMaterialCard)
                .Include(wo => wo.SupervisorUser)
                .Where(wo => wo.ProductMaterialCardId == productMaterialCardId)
                .OrderByDescending(wo => wo.CreateDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkOrder>> GetByStatusAsync(string status)
        {
            return await _dbSet
                .Include(wo => wo.BillOfMaterial)
                .Include(wo => wo.ProductMaterialCard)
                .Include(wo => wo.SupervisorUser)
                .Where(wo => wo.Status == status)
                .OrderBy(wo => wo.Priority)
                .ThenBy(wo => wo.PlannedStartDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkOrder>> GetByPriorityAsync(int priority)
        {
            return await _dbSet
                .Include(wo => wo.BillOfMaterial)
                .Include(wo => wo.ProductMaterialCard)
                .Include(wo => wo.SupervisorUser)
                .Where(wo => wo.Priority == priority)
                .OrderBy(wo => wo.PlannedStartDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkOrder>> GetByWorkCenterAsync(string workCenter)
        {
            return await _dbSet
                .Include(wo => wo.BillOfMaterial)
                .Include(wo => wo.ProductMaterialCard)
                .Include(wo => wo.SupervisorUser)
                .Where(wo => wo.WorkCenter == workCenter)
                .OrderBy(wo => wo.PlannedStartDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkOrder>> GetBySupervisorAsync(long supervisorUserId)
        {
            return await _dbSet
                .Include(wo => wo.BillOfMaterial)
                .Include(wo => wo.ProductMaterialCard)
                .Include(wo => wo.SupervisorUser)
                .Where(wo => wo.SupervisorUserId == supervisorUserId)
                .OrderBy(wo => wo.Priority)
                .ThenBy(wo => wo.PlannedStartDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkOrder>> GetByPlannedDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Include(wo => wo.BillOfMaterial)
                .Include(wo => wo.ProductMaterialCard)
                .Include(wo => wo.SupervisorUser)
                .Where(wo => wo.PlannedStartDate >= startDate && wo.PlannedStartDate <= endDate)
                .OrderBy(wo => wo.PlannedStartDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkOrder>> GetByDueDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Include(wo => wo.BillOfMaterial)
                .Include(wo => wo.ProductMaterialCard)
                .Include(wo => wo.SupervisorUser)
                .Where(wo => wo.DueDate.HasValue && wo.DueDate.Value >= startDate && wo.DueDate.Value <= endDate)
                .OrderBy(wo => wo.DueDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkOrder>> GetOverdueWorkOrdersAsync()
        {
            var today = DateTime.Today;
            
            return await _dbSet
                .Include(wo => wo.BillOfMaterial)
                .Include(wo => wo.ProductMaterialCard)
                .Include(wo => wo.SupervisorUser)
                .Where(wo => 
                    wo.DueDate.HasValue && 
                    wo.DueDate.Value < today && 
                    wo.Status != "COMPLETED" && 
                    wo.Status != "CANCELLED")
                .OrderBy(wo => wo.DueDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkOrder>> GetDueSoonAsync(int daysAhead = 7)
        {
            var today = DateTime.Today;
            var futureDate = today.AddDays(daysAhead);
            
            return await _dbSet
                .Include(wo => wo.BillOfMaterial)
                .Include(wo => wo.ProductMaterialCard)
                .Include(wo => wo.SupervisorUser)
                .Where(wo => 
                    wo.DueDate.HasValue && 
                    wo.DueDate.Value >= today && 
                    wo.DueDate.Value <= futureDate &&
                    wo.Status != "COMPLETED" && 
                    wo.Status != "CANCELLED")
                .OrderBy(wo => wo.DueDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkOrder>> GetActiveWorkOrdersAsync()
        {
            return await _dbSet
                .Include(wo => wo.BillOfMaterial)
                .Include(wo => wo.ProductMaterialCard)
                .Include(wo => wo.SupervisorUser)
                .Where(wo => 
                    wo.Status == "RELEASED" || 
                    wo.Status == "IN_PROGRESS")
                .OrderBy(wo => wo.Priority)
                .ThenBy(wo => wo.PlannedStartDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkOrder>> GetCompletedWorkOrdersAsync()
        {
            return await _dbSet
                .Include(wo => wo.BillOfMaterial)
                .Include(wo => wo.ProductMaterialCard)
                .Include(wo => wo.SupervisorUser)
                .Where(wo => wo.Status == "COMPLETED")
                .OrderByDescending(wo => wo.ActualEndDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkOrder>> GetReadyToStartAsync()
        {
            return await _dbSet
                .Include(wo => wo.BillOfMaterial)
                .Include(wo => wo.ProductMaterialCard)
                .Include(wo => wo.SupervisorUser)
                .Where(wo => wo.Status == "RELEASED")
                .OrderBy(wo => wo.Priority)
                .ThenBy(wo => wo.PlannedStartDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkOrder>> GetInProgressAsync()
        {
            return await _dbSet
                .Include(wo => wo.BillOfMaterial)
                .Include(wo => wo.ProductMaterialCard)
                .Include(wo => wo.SupervisorUser)
                .Where(wo => wo.Status == "IN_PROGRESS")
                .OrderBy(wo => wo.Priority)
                .ThenBy(wo => wo.PlannedStartDate)
                .ToListAsync();
        }

        public async Task<WorkOrder?> GetWithConfirmationsAsync(long id)
        {
            return await _dbSet
                .Include(wo => wo.BillOfMaterial)
                .Include(wo => wo.ProductMaterialCard)
                .Include(wo => wo.SupervisorUser)
                .Include(wo => wo.ProductionConfirmations.OrderByDescending(pc => pc.ConfirmationDate))
                    .ThenInclude(pc => pc.OperatorUser)
                .FirstOrDefaultAsync(wo => wo.Id == id);
        }

        public async Task<WorkOrder?> GetWithBOMDetailsAsync(long id)
        {
            return await _dbSet
                .Include(wo => wo.BillOfMaterial)
                    .ThenInclude(bom => bom.BOMItems.OrderBy(bi => bi.LineNumber))
                        .ThenInclude(bi => bi.MaterialCard)
                .Include(wo => wo.ProductMaterialCard)
                .Include(wo => wo.SupervisorUser)
                .FirstOrDefaultAsync(wo => wo.Id == id);
        }

        public async Task<IEnumerable<WorkOrder>> GetRequiringQualityCheckAsync()
        {
            return await _dbSet
                .Include(wo => wo.BillOfMaterial)
                .Include(wo => wo.ProductMaterialCard)
                .Include(wo => wo.SupervisorUser)
                .Where(wo => 
                    wo.RequiresQualityCheck && 
                    (wo.QualityStatus == "PENDING" || wo.QualityStatus == "IN_PROGRESS"))
                .OrderBy(wo => wo.Priority)
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkOrder>> GetBySourceTypeAsync(string sourceType)
        {
            return await _dbSet
                .Include(wo => wo.BillOfMaterial)
                .Include(wo => wo.ProductMaterialCard)
                .Include(wo => wo.SupervisorUser)
                .Where(wo => wo.SourceType == sourceType)
                .OrderByDescending(wo => wo.CreateDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkOrder>> GetByCustomerOrderReferenceAsync(string customerOrderReference)
        {
            return await _dbSet
                .Include(wo => wo.BillOfMaterial)
                .Include(wo => wo.ProductMaterialCard)
                .Include(wo => wo.SupervisorUser)
                .Where(wo => wo.CustomerOrderReference == customerOrderReference)
                .OrderBy(wo => wo.PlannedStartDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkOrder>> SearchAsync(string searchTerm)
        {
            var searchLower = searchTerm.ToLower();
            
            return await _dbSet
                .Include(wo => wo.BillOfMaterial)
                .Include(wo => wo.ProductMaterialCard)
                .Include(wo => wo.SupervisorUser)
                .Where(wo => 
                    wo.WorkOrderNumber.ToLower().Contains(searchLower) ||
                    wo.BillOfMaterial.BOMCode.ToLower().Contains(searchLower) ||
                    wo.BillOfMaterial.BOMName.ToLower().Contains(searchLower) ||
                    wo.ProductMaterialCard.CardName.ToLower().Contains(searchLower) ||
                    wo.ProductMaterialCard.CardCode.ToLower().Contains(searchLower) ||
                    (wo.Description != null && wo.Description.ToLower().Contains(searchLower)) ||
                    (wo.CustomerOrderReference != null && wo.CustomerOrderReference.ToLower().Contains(searchLower)))
                .OrderBy(wo => wo.WorkOrderNumber)
                .ToListAsync();
        }

        public async Task<Dictionary<string, int>> GetStatusStatisticsAsync()
        {
            return await _dbSet
                .GroupBy(wo => wo.Status)
                .ToDictionaryAsync(g => g.Key, g => g.Count());
        }

        public async Task<IEnumerable<WorkOrder>> GetForProductionPlanningAsync()
        {
            return await _dbSet
                .Include(wo => wo.BillOfMaterial)
                .Include(wo => wo.ProductMaterialCard)
                .Include(wo => wo.SupervisorUser)
                .Where(wo => 
                    wo.Status == "CREATED" || 
                    wo.Status == "RELEASED" || 
                    wo.Status == "IN_PROGRESS")
                .OrderBy(wo => wo.Priority)
                .ThenBy(wo => wo.PlannedStartDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkOrder>> GetForSchedulingAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Include(wo => wo.BillOfMaterial)
                .Include(wo => wo.ProductMaterialCard)
                .Include(wo => wo.SupervisorUser)
                .Where(wo => 
                    (wo.PlannedStartDate >= startDate && wo.PlannedStartDate <= endDate) ||
                    (wo.PlannedEndDate >= startDate && wo.PlannedEndDate <= endDate) ||
                    (wo.PlannedStartDate <= startDate && wo.PlannedEndDate >= endDate))
                .OrderBy(wo => wo.PlannedStartDate)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(string workOrderNumber)
        {
            return await _dbSet
                .AnyAsync(wo => wo.WorkOrderNumber == workOrderNumber);
        }

        public async Task<string> GetNextWorkOrderNumberAsync()
        {
            var today = DateTime.Today;
            var yearMonth = today.ToString("yyyyMM");
            var prefix = $"WO-{yearMonth}-";
            
            var lastWorkOrder = await _dbSet
                .Where(wo => wo.WorkOrderNumber.StartsWith(prefix))
                .OrderByDescending(wo => wo.WorkOrderNumber)
                .FirstOrDefaultAsync();

            if (lastWorkOrder == null)
            {
                return $"{prefix}0001";
            }

            var lastNumberPart = lastWorkOrder.WorkOrderNumber.Substring(prefix.Length);
            if (int.TryParse(lastNumberPart, out int lastNumber))
            {
                return $"{prefix}{(lastNumber + 1):D4}";
            }

            return $"{prefix}0001";
        }

        public override async Task<List<WorkOrder>> GetAllAsync()
        {
            return await _dbSet
                .Include(wo => wo.BillOfMaterial)
                .Include(wo => wo.ProductMaterialCard)
                .Include(wo => wo.SupervisorUser)
                .OrderByDescending(wo => wo.CreateDate)
                .ToListAsync();
        }

        public override async Task<WorkOrder?> GetByIdAsync(long id)
        {
            return await _dbSet
                .Include(wo => wo.BillOfMaterial)
                    .ThenInclude(bom => bom.ProductMaterialCard)
                .Include(wo => wo.ProductMaterialCard)
                .Include(wo => wo.SupervisorUser)
                .Include(wo => wo.ReleasedByUser)
                .FirstOrDefaultAsync(wo => wo.Id == id);
        }
    }
} 