using Teklas_Intern_ERP.Entities.ProductionManagement;
using Teklas_Intern_ERP.DataAccess.Repositories;

namespace Teklas_Intern_ERP.DataAccess.ProductionManagement
{
    /// <summary>
    /// Bill of Material Repository Interface
    /// </summary>
    public interface IBillOfMaterialRepository : IRepository<BillOfMaterial>
    {
        /// <summary>
        /// Get BOM by code
        /// </summary>
        Task<BillOfMaterial?> GetByCodeAsync(string bomCode);

        /// <summary>
        /// Get BOMs by product material card
        /// </summary>
        Task<IEnumerable<BillOfMaterial>> GetByProductMaterialCardAsync(long productMaterialCardId);

        /// <summary>
        /// Get active BOMs
        /// </summary>
        Task<IEnumerable<BillOfMaterial>> GetActiveBOMsAsync();

        /// <summary>
        /// Get approved BOMs
        /// </summary>
        Task<IEnumerable<BillOfMaterial>> GetApprovedBOMsAsync();

        /// <summary>
        /// Get BOMs by type
        /// </summary>
        Task<IEnumerable<BillOfMaterial>> GetByTypeAsync(string bomType);

        /// <summary>
        /// Get BOMs by approval status
        /// </summary>
        Task<IEnumerable<BillOfMaterial>> GetByApprovalStatusAsync(string approvalStatus);

        /// <summary>
        /// Get effective BOMs for a specific date
        /// </summary>
        Task<IEnumerable<BillOfMaterial>> GetEffectiveBOMsAsync(DateTime effectiveDate);

        /// <summary>
        /// Get BOMs with items
        /// </summary>
        Task<BillOfMaterial?> GetWithItemsAsync(long id);

        /// <summary>
        /// Get BOMs with work orders
        /// </summary>
        Task<BillOfMaterial?> GetWithWorkOrdersAsync(long id);

        /// <summary>
        /// Get BOM versions
        /// </summary>
        Task<IEnumerable<BillOfMaterial>> GetVersionsAsync(string bomCode);

        /// <summary>
        /// Get latest version of BOM
        /// </summary>
        Task<BillOfMaterial?> GetLatestVersionAsync(string bomCode);

        /// <summary>
        /// Check if BOM code exists
        /// </summary>
        Task<bool> ExistsAsync(string bomCode);

        /// <summary>
        /// Search BOMs
        /// </summary>
        Task<IEnumerable<BillOfMaterial>> SearchAsync(string searchTerm);

        /// <summary>
        /// Get BOMs for export
        /// </summary>
        Task<IEnumerable<BillOfMaterial>> GetForExportAsync();

        /// <summary>
        /// Get BOMs requiring approval
        /// </summary>
        Task<IEnumerable<BillOfMaterial>> GetPendingApprovalAsync();

        /// <summary>
        /// Get expired BOMs
        /// </summary>
        Task<IEnumerable<BillOfMaterial>> GetExpiredBOMsAsync();

        /// <summary>
        /// Get BOMs expiring soon
        /// </summary>
        Task<IEnumerable<BillOfMaterial>> GetExpiringSoonAsync(int daysAhead = 30);
    }
} 