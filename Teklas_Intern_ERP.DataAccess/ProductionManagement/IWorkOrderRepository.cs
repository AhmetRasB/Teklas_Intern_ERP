using Teklas_Intern_ERP.Entities.ProductionManagement;
using Teklas_Intern_ERP.DataAccess.Repositories;

namespace Teklas_Intern_ERP.DataAccess.ProductionManagement
{
    /// <summary>
    /// Work Order Repository Interface
    /// </summary>
    public interface IWorkOrderRepository : IRepository<WorkOrder>
    {
        /// <summary>
        /// Get work order by number
        /// </summary>
        Task<WorkOrder?> GetByNumberAsync(string workOrderNumber);

        /// <summary>
        /// Get work orders by BOM
        /// </summary>
        Task<IEnumerable<WorkOrder>> GetByBOMAsync(long billOfMaterialId);

        /// <summary>
        /// Get work orders by product material card
        /// </summary>
        Task<IEnumerable<WorkOrder>> GetByProductMaterialCardAsync(long productMaterialCardId);

        /// <summary>
        /// Get work orders by status
        /// </summary>
        Task<IEnumerable<WorkOrder>> GetByStatusAsync(string status);

        /// <summary>
        /// Get work orders by priority
        /// </summary>
        Task<IEnumerable<WorkOrder>> GetByPriorityAsync(int priority);

        /// <summary>
        /// Get work orders by work center
        /// </summary>
        Task<IEnumerable<WorkOrder>> GetByWorkCenterAsync(string workCenter);

        /// <summary>
        /// Get work orders by supervisor
        /// </summary>
        Task<IEnumerable<WorkOrder>> GetBySupervisorAsync(long supervisorUserId);

        /// <summary>
        /// Get work orders by planned date range
        /// </summary>
        Task<IEnumerable<WorkOrder>> GetByPlannedDateRangeAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Get work orders by due date range
        /// </summary>
        Task<IEnumerable<WorkOrder>> GetByDueDateRangeAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Get overdue work orders
        /// </summary>
        Task<IEnumerable<WorkOrder>> GetOverdueWorkOrdersAsync();

        /// <summary>
        /// Get work orders due soon
        /// </summary>
        Task<IEnumerable<WorkOrder>> GetDueSoonAsync(int daysAhead = 7);

        /// <summary>
        /// Get active work orders
        /// </summary>
        Task<IEnumerable<WorkOrder>> GetActiveWorkOrdersAsync();

        /// <summary>
        /// Get completed work orders
        /// </summary>
        Task<IEnumerable<WorkOrder>> GetCompletedWorkOrdersAsync();

        /// <summary>
        /// Get work orders ready to start
        /// </summary>
        Task<IEnumerable<WorkOrder>> GetReadyToStartAsync();

        /// <summary>
        /// Get work orders in progress
        /// </summary>
        Task<IEnumerable<WorkOrder>> GetInProgressAsync();

        /// <summary>
        /// Get work order with confirmations
        /// </summary>
        Task<WorkOrder?> GetWithConfirmationsAsync(long id);

        /// <summary>
        /// Get work order with BOM details
        /// </summary>
        Task<WorkOrder?> GetWithBOMDetailsAsync(long id);

        /// <summary>
        /// Get work orders requiring quality check
        /// </summary>
        Task<IEnumerable<WorkOrder>> GetRequiringQualityCheckAsync();

        /// <summary>
        /// Get work orders by source type
        /// </summary>
        Task<IEnumerable<WorkOrder>> GetBySourceTypeAsync(string sourceType);

        /// <summary>
        /// Get work orders by customer order reference
        /// </summary>
        Task<IEnumerable<WorkOrder>> GetByCustomerOrderReferenceAsync(string customerOrderReference);

        /// <summary>
        /// Search work orders
        /// </summary>
        Task<IEnumerable<WorkOrder>> SearchAsync(string searchTerm);

        /// <summary>
        /// Get work order statistics
        /// </summary>
        Task<Dictionary<string, int>> GetStatusStatisticsAsync();

        /// <summary>
        /// Get work orders for production planning
        /// </summary>
        Task<IEnumerable<WorkOrder>> GetForProductionPlanningAsync();

        /// <summary>
        /// Get work orders for scheduling
        /// </summary>
        Task<IEnumerable<WorkOrder>> GetForSchedulingAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Check if work order number exists
        /// </summary>
        Task<bool> ExistsAsync(string workOrderNumber);

        /// <summary>
        /// Get next work order number
        /// </summary>
        Task<string> GetNextWorkOrderNumberAsync();
    }
} 