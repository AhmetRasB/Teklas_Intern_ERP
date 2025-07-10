using Microsoft.EntityFrameworkCore;
using Teklas_Intern_ERP.Entities.ProductionManagement;
using Teklas_Intern_ERP.DataAccess.Repositories;
using Teklas_Intern_ERP.DTOs;

namespace Teklas_Intern_ERP.DataAccess.ProductionManagement
{
    /// <summary>
    /// Production Confirmation Repository Implementation
    /// </summary>
    public sealed class ProductionConfirmationRepository : BaseRepository<ProductionConfirmation>, IProductionConfirmationRepository
    {
        public ProductionConfirmationRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<ProductionConfirmation?> GetByNumberAsync(string confirmationNumber)
        {
            return await _dbSet
                .Include(pc => pc.WorkOrder)
                    .ThenInclude(wo => wo.ProductMaterialCard)
                .Include(pc => pc.OperatorUser)
                .Include(pc => pc.ConfirmedByUser)
                .FirstOrDefaultAsync(pc => pc.ConfirmationNumber == confirmationNumber);
        }

        public async Task<IEnumerable<ProductionConfirmation>> GetByWorkOrderAsync(long workOrderId)
        {
            return await _dbSet
                .Include(pc => pc.WorkOrder)
                .Include(pc => pc.OperatorUser)
                .Include(pc => pc.ConfirmedByUser)
                .Where(pc => pc.WorkOrderId == workOrderId)
                .OrderByDescending(pc => pc.ConfirmationDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductionConfirmation>> GetByOperatorAsync(long operatorUserId)
        {
            return await _dbSet
                .Include(pc => pc.WorkOrder)
                    .ThenInclude(wo => wo.ProductMaterialCard)
                .Include(pc => pc.OperatorUser)
                .Where(pc => pc.OperatorUserId == operatorUserId)
                .OrderByDescending(pc => pc.ConfirmationDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductionConfirmation>> GetByStatusAsync(string status)
        {
            return await _dbSet
                .Include(pc => pc.WorkOrder)
                    .ThenInclude(wo => wo.ProductMaterialCard)
                .Include(pc => pc.OperatorUser)
                .Where(pc => pc.Status == status)
                .OrderByDescending(pc => pc.ConfirmationDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductionConfirmation>> GetByTypeAsync(string confirmationType)
        {
            return await _dbSet
                .Include(pc => pc.WorkOrder)
                    .ThenInclude(wo => wo.ProductMaterialCard)
                .Include(pc => pc.OperatorUser)
                .Where(pc => pc.ConfirmationType == confirmationType)
                .OrderByDescending(pc => pc.ConfirmationDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductionConfirmation>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Include(pc => pc.WorkOrder)
                    .ThenInclude(wo => wo.ProductMaterialCard)
                .Include(pc => pc.OperatorUser)
                .Where(pc => pc.ConfirmationDate >= startDate && pc.ConfirmationDate <= endDate)
                .OrderByDescending(pc => pc.ConfirmationDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductionConfirmation>> GetByWorkCenterAsync(string workCenter)
        {
            return await _dbSet
                .Include(pc => pc.WorkOrder)
                    .ThenInclude(wo => wo.ProductMaterialCard)
                .Include(pc => pc.OperatorUser)
                .Where(pc => pc.WorkCenter == workCenter)
                .OrderByDescending(pc => pc.ConfirmationDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductionConfirmation>> GetByShiftAsync(string shift)
        {
            return await _dbSet
                .Include(pc => pc.WorkOrder)
                    .ThenInclude(wo => wo.ProductMaterialCard)
                .Include(pc => pc.OperatorUser)
                .Where(pc => pc.Shift == shift)
                .OrderByDescending(pc => pc.ConfirmationDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductionConfirmation>> GetDraftConfirmationsAsync()
        {
            return await _dbSet
                .Include(pc => pc.WorkOrder)
                    .ThenInclude(wo => wo.ProductMaterialCard)
                .Include(pc => pc.OperatorUser)
                .Where(pc => pc.Status == "DRAFT")
                .OrderBy(pc => pc.ConfirmationDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductionConfirmation>> GetConfirmedProductionsAsync()
        {
            return await _dbSet
                .Include(pc => pc.WorkOrder)
                    .ThenInclude(wo => wo.ProductMaterialCard)
                .Include(pc => pc.OperatorUser)
                .Include(pc => pc.ConfirmedByUser)
                .Where(pc => pc.Status == "CONFIRMED")
                .OrderByDescending(pc => pc.ConfirmedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductionConfirmation>> GetRequiringStockPostingAsync()
        {
            return await _dbSet
                .Include(pc => pc.WorkOrder)
                    .ThenInclude(wo => wo.ProductMaterialCard)
                .Include(pc => pc.OperatorUser)
                .Where(pc => 
                    pc.RequiresStockPosting && 
                    !pc.StockPosted && 
                    pc.Status == "CONFIRMED")
                .OrderBy(pc => pc.ConfirmedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductionConfirmation>> GetWithQualityIssuesAsync()
        {
            return await _dbSet
                .Include(pc => pc.WorkOrder)
                    .ThenInclude(wo => wo.ProductMaterialCard)
                .Include(pc => pc.OperatorUser)
                .Where(pc => 
                    pc.QualityStatus == "FAILED" || 
                    pc.ScrapQuantity > 0)
                .OrderByDescending(pc => pc.ConfirmationDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductionConfirmation>> GetByBatchNumberAsync(string batchNumber)
        {
            return await _dbSet
                .Include(pc => pc.WorkOrder)
                    .ThenInclude(wo => wo.ProductMaterialCard)
                .Include(pc => pc.OperatorUser)
                .Where(pc => pc.BatchNumber == batchNumber)
                .OrderByDescending(pc => pc.ConfirmationDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductionConfirmation>> GetByQualityStatusAsync(string qualityStatus)
        {
            return await _dbSet
                .Include(pc => pc.WorkOrder)
                    .ThenInclude(wo => wo.ProductMaterialCard)
                .Include(pc => pc.OperatorUser)
                .Where(pc => pc.QualityStatus == qualityStatus)
                .OrderByDescending(pc => pc.ConfirmationDate)
                .ToListAsync();
        }

        public async Task<ProductionSummaryDto> GetProductionSummaryAsync(long workOrderId)
        {
            var confirmations = await _dbSet
                .Include(pc => pc.WorkOrder)
                .Where(pc => pc.WorkOrderId == workOrderId && pc.Status == "CONFIRMED")
                .ToListAsync();

            var workOrder = confirmations.FirstOrDefault()?.WorkOrder;
            if (workOrder == null)
            {
                return new ProductionSummaryDto { WorkOrderId = workOrderId };
            }

            var totalConfirmed = confirmations.Sum(pc => pc.ConfirmedQuantity);
            var totalScrap = confirmations.Sum(pc => pc.ScrapQuantity);
            var totalRework = confirmations.Sum(pc => pc.ReworkQuantity);
            var totalProduced = totalConfirmed + totalScrap + totalRework;

            return new ProductionSummaryDto
            {
                WorkOrderId = workOrderId,
                WorkOrderNumber = workOrder.WorkOrderNumber,
                PlannedQuantity = workOrder.PlannedQuantity,
                TotalConfirmed = totalConfirmed,
                TotalScrap = totalScrap,
                TotalRework = totalRework,
                CompletionPercentage = workOrder.PlannedQuantity > 0 ? 
                    (totalProduced / workOrder.PlannedQuantity) * 100 : 0,
                YieldPercentage = totalProduced > 0 ? 
                    (totalConfirmed / totalProduced) * 100 : 0,
                ConfirmationCount = confirmations.Count
            };
        }

        public async Task<IEnumerable<OperatorEfficiencyDto>> GetOperatorEfficiencyAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Include(pc => pc.OperatorUser)
                .Where(pc => 
                    pc.ConfirmationDate >= startDate && 
                    pc.ConfirmationDate <= endDate &&
                    pc.Status == "CONFIRMED" &&
                    pc.OperatorUserId.HasValue)
                .GroupBy(pc => new { pc.OperatorUserId, pc.OperatorUser!.FirstName, pc.OperatorUser.LastName })
                .Select(g => new OperatorEfficiencyDto
                {
                    OperatorUserId = g.Key.OperatorUserId!.Value,
                    OperatorName = $"{g.Key.FirstName} {g.Key.LastName}",
                    TotalQuantity = g.Sum(pc => pc.ConfirmedQuantity),
                    TotalTime = g.Sum(pc => (pc.SetupTime ?? 0) + (pc.RunTime ?? 0) + (pc.DownTime ?? 0)),
                    AverageEfficiency = g.Average(pc => 
                        (pc.SetupTime ?? 0) + (pc.RunTime ?? 0) + (pc.DownTime ?? 0) > 0 ?
                        ((pc.RunTime ?? 0) / ((pc.SetupTime ?? 0) + (pc.RunTime ?? 0) + (pc.DownTime ?? 0))) * 100 : 0),
                    ConfirmationCount = g.Count()
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<ScrapAnalysisDto>> GetScrapAnalysisAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Include(pc => pc.WorkOrder)
                    .ThenInclude(wo => wo.ProductMaterialCard)
                .Where(pc => 
                    pc.ConfirmationDate >= startDate && 
                    pc.ConfirmationDate <= endDate &&
                    pc.Status == "CONFIRMED")
                .GroupBy(pc => new 
                { 
                    pc.WorkOrder.ProductMaterialCard.CardName,
                    WorkCenter = pc.WorkCenter ?? "Unknown"
                })
                .Select(g => new ScrapAnalysisDto
                {
                    ProductName = g.Key.CardName,
                    WorkCenter = g.Key.WorkCenter,
                    TotalProduced = g.Sum(pc => pc.ConfirmedQuantity + pc.ScrapQuantity + pc.ReworkQuantity),
                    TotalScrap = g.Sum(pc => pc.ScrapQuantity),
                    ScrapPercentage = g.Sum(pc => pc.ConfirmedQuantity + pc.ScrapQuantity + pc.ReworkQuantity) > 0 ?
                        (g.Sum(pc => pc.ScrapQuantity) / g.Sum(pc => pc.ConfirmedQuantity + pc.ScrapQuantity + pc.ReworkQuantity)) * 100 : 0,
                    PrimaryReason = g.Where(pc => pc.ScrapQuantity > 0)
                        .GroupBy(pc => pc.Notes ?? "Unknown")
                        .OrderByDescending(rg => rg.Count())
                        .Select(rg => rg.Key)
                        .FirstOrDefault()
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductionConfirmation>> GetForPostingAsync()
        {
            return await _dbSet
                .Include(pc => pc.WorkOrder)
                    .ThenInclude(wo => wo.ProductMaterialCard)
                .Include(pc => pc.OperatorUser)
                .Where(pc => 
                    pc.Status == "CONFIRMED" && 
                    pc.RequiresStockPosting && 
                    !pc.StockPosted)
                .OrderBy(pc => pc.ConfirmedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductionConfirmation>> SearchAsync(string searchTerm)
        {
            var searchLower = searchTerm.ToLower();
            
            return await _dbSet
                .Include(pc => pc.WorkOrder)
                    .ThenInclude(wo => wo.ProductMaterialCard)
                .Include(pc => pc.OperatorUser)
                .Where(pc => 
                    pc.ConfirmationNumber.ToLower().Contains(searchLower) ||
                    pc.WorkOrder.WorkOrderNumber.ToLower().Contains(searchLower) ||
                    pc.WorkOrder.ProductMaterialCard.CardName.ToLower().Contains(searchLower) ||
                    pc.WorkOrder.ProductMaterialCard.CardCode.ToLower().Contains(searchLower) ||
                    (pc.BatchNumber != null && pc.BatchNumber.ToLower().Contains(searchLower)) ||
                    (pc.Notes != null && pc.Notes.ToLower().Contains(searchLower)))
                .OrderByDescending(pc => pc.ConfirmationDate)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(string confirmationNumber)
        {
            return await _dbSet
                .AnyAsync(pc => pc.ConfirmationNumber == confirmationNumber);
        }

        public async Task<string> GetNextConfirmationNumberAsync()
        {
            var today = DateTime.Today;
            var yearMonth = today.ToString("yyyyMM");
            var prefix = $"PC-{yearMonth}-";
            
            var lastConfirmation = await _dbSet
                .Where(pc => pc.ConfirmationNumber.StartsWith(prefix))
                .OrderByDescending(pc => pc.ConfirmationNumber)
                .FirstOrDefaultAsync();

            if (lastConfirmation == null)
            {
                return $"{prefix}0001";
            }

            var lastNumberPart = lastConfirmation.ConfirmationNumber.Substring(prefix.Length);
            if (int.TryParse(lastNumberPart, out int lastNumber))
            {
                return $"{prefix}{(lastNumber + 1):D4}";
            }

            return $"{prefix}0001";
        }

        public override async Task<List<ProductionConfirmation>> GetAllAsync()
        {
            return await _dbSet
                .Include(pc => pc.WorkOrder)
                    .ThenInclude(wo => wo.ProductMaterialCard)
                .Include(pc => pc.OperatorUser)
                .Include(pc => pc.ConfirmedByUser)
                .OrderByDescending(pc => pc.ConfirmationDate)
                .ToListAsync();
        }

        public override async Task<ProductionConfirmation?> GetByIdAsync(long id)
        {
            return await _dbSet
                .Include(pc => pc.WorkOrder)
                    .ThenInclude(wo => wo.BillOfMaterial)
                .Include(pc => pc.WorkOrder)
                    .ThenInclude(wo => wo.ProductMaterialCard)
                .Include(pc => pc.OperatorUser)
                .Include(pc => pc.ConfirmedByUser)
                .FirstOrDefaultAsync(pc => pc.Id == id);
        }
    }
} 