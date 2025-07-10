using Teklas_Intern_ERP.Entities.ProductionManagement;
using Teklas_Intern_ERP.DataAccess.Repositories;
using Teklas_Intern_ERP.DTOs;

namespace Teklas_Intern_ERP.DataAccess.ProductionManagement
{
    /// <summary>
    /// Production Confirmation Repository Interface
    /// </summary>
    public interface IProductionConfirmationRepository : IRepository<ProductionConfirmation>
    {
        /// <summary>
        /// Get confirmation by number
        /// </summary>
        Task<ProductionConfirmation?> GetByNumberAsync(string confirmationNumber);

        /// <summary>
        /// Get confirmations by work order
        /// </summary>
        Task<IEnumerable<ProductionConfirmation>> GetByWorkOrderAsync(long workOrderId);

        /// <summary>
        /// Get confirmations by operator
        /// </summary>
        Task<IEnumerable<ProductionConfirmation>> GetByOperatorAsync(long operatorUserId);

        /// <summary>
        /// Get confirmations by status
        /// </summary>
        Task<IEnumerable<ProductionConfirmation>> GetByStatusAsync(string status);

        /// <summary>
        /// Get confirmations by confirmation type
        /// </summary>
        Task<IEnumerable<ProductionConfirmation>> GetByTypeAsync(string confirmationType);

        /// <summary>
        /// Get confirmations by date range
        /// </summary>
        Task<IEnumerable<ProductionConfirmation>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Get confirmations by work center
        /// </summary>
        Task<IEnumerable<ProductionConfirmation>> GetByWorkCenterAsync(string workCenter);

        /// <summary>
        /// Get confirmations by shift
        /// </summary>
        Task<IEnumerable<ProductionConfirmation>> GetByShiftAsync(string shift);

        /// <summary>
        /// Get draft confirmations
        /// </summary>
        Task<IEnumerable<ProductionConfirmation>> GetDraftConfirmationsAsync();

        /// <summary>
        /// Get confirmed productions
        /// </summary>
        Task<IEnumerable<ProductionConfirmation>> GetConfirmedProductionsAsync();

        /// <summary>
        /// Get confirmations requiring stock posting
        /// </summary>
        Task<IEnumerable<ProductionConfirmation>> GetRequiringStockPostingAsync();

        /// <summary>
        /// Get confirmations with quality issues
        /// </summary>
        Task<IEnumerable<ProductionConfirmation>> GetWithQualityIssuesAsync();

        /// <summary>
        /// Get confirmations by batch number
        /// </summary>
        Task<IEnumerable<ProductionConfirmation>> GetByBatchNumberAsync(string batchNumber);

        /// <summary>
        /// Get confirmations by quality status
        /// </summary>
        Task<IEnumerable<ProductionConfirmation>> GetByQualityStatusAsync(string qualityStatus);

        /// <summary>
        /// Get production summary by work order
        /// </summary>
        Task<ProductionSummaryDto> GetProductionSummaryAsync(long workOrderId);

        /// <summary>
        /// Get efficiency report by operator
        /// </summary>
        Task<IEnumerable<OperatorEfficiencyDto>> GetOperatorEfficiencyAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Get scrap analysis
        /// </summary>
        Task<IEnumerable<ScrapAnalysisDto>> GetScrapAnalysisAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Get confirmations for posting
        /// </summary>
        Task<IEnumerable<ProductionConfirmation>> GetForPostingAsync();

        /// <summary>
        /// Search confirmations
        /// </summary>
        Task<IEnumerable<ProductionConfirmation>> SearchAsync(string searchTerm);

        /// <summary>
        /// Check if confirmation number exists
        /// </summary>
        Task<bool> ExistsAsync(string confirmationNumber);

        /// <summary>
        /// Get next confirmation number
        /// </summary>
        Task<string> GetNextConfirmationNumberAsync();
    }
} 