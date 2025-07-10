using Microsoft.EntityFrameworkCore;
using Teklas_Intern_ERP.Entities.ProductionManagement;
using Teklas_Intern_ERP.DataAccess.Repositories;

namespace Teklas_Intern_ERP.DataAccess.ProductionManagement
{
    /// <summary>
    /// Bill of Material Repository Implementation
    /// </summary>
    public sealed class BillOfMaterialRepository : BaseRepository<BillOfMaterial>, IBillOfMaterialRepository
    {
        public BillOfMaterialRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<BillOfMaterial?> GetByCodeAsync(string bomCode)
        {
            return await _dbSet
                .Include(bom => bom.ProductMaterialCard)
                .Include(bom => bom.BOMItems)
                    .ThenInclude(item => item.MaterialCard)
                .FirstOrDefaultAsync(bom => bom.BOMCode == bomCode);
        }

        public async Task<IEnumerable<BillOfMaterial>> GetByProductMaterialCardAsync(long productMaterialCardId)
        {
            return await _dbSet
                .Include(bom => bom.ProductMaterialCard)
                .Include(bom => bom.BOMItems)
                    .ThenInclude(item => item.MaterialCard)
                .Where(bom => bom.ProductMaterialCardId == productMaterialCardId)
                .OrderByDescending(bom => bom.CreateDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<BillOfMaterial>> GetByStatusAsync(string approvalStatus)
        {
            return await _dbSet
                .Include(bom => bom.ProductMaterialCard)
                .Where(bom => bom.ApprovalStatus == approvalStatus)
                .OrderBy(bom => bom.BOMCode)
                .ToListAsync();
        }

        public async Task<IEnumerable<BillOfMaterial>> GetActiveAsync()
        {
            return await _dbSet
                .Include(bom => bom.ProductMaterialCard)
                .Where(bom => bom.IsActive && bom.ApprovalStatus == "APPROVED")
                .OrderBy(bom => bom.BOMCode)
                .ToListAsync();
        }

        public async Task<IEnumerable<BillOfMaterial>> GetByVersionAsync(string version)
        {
            return await _dbSet
                .Include(bom => bom.ProductMaterialCard)
                .Where(bom => bom.Version == version)
                .OrderBy(bom => bom.BOMCode)
                .ToListAsync();
        }

        public async Task<IEnumerable<BillOfMaterial>> GetByBOMTypeAsync(string bomType)
        {
            return await _dbSet
                .Include(bom => bom.ProductMaterialCard)
                .Where(bom => bom.BOMType == bomType)
                .OrderBy(bom => bom.BOMCode)
                .ToListAsync();
        }

        public async Task<IEnumerable<BillOfMaterial>> GetEffectiveAsync(DateTime effectiveDate)
        {
            return await _dbSet
                .Include(bom => bom.ProductMaterialCard)
                .Where(bom => bom.IsActive && 
                             bom.ApprovalStatus == "APPROVED" &&
                             (!bom.EffectiveFrom.HasValue || bom.EffectiveFrom.Value <= effectiveDate) &&
                             (!bom.EffectiveTo.HasValue || bom.EffectiveTo.Value >= effectiveDate))
                .OrderBy(bom => bom.BOMCode)
                .ToListAsync();
        }

        public async Task<IEnumerable<BillOfMaterial>> GetByApprovedByUserAsync(long approvedByUserId)
        {
            return await _dbSet
                .Include(bom => bom.ProductMaterialCard)
                .Where(bom => bom.ApprovedByUserId == approvedByUserId)
                .OrderByDescending(bom => bom.ApprovalDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<BillOfMaterial>> GetExpiredAsync()
        {
            var today = DateTime.Today;
            return await _dbSet
                .Include(bom => bom.ProductMaterialCard)
                .Where(bom => bom.EffectiveTo.HasValue && bom.EffectiveTo.Value < today)
                .OrderBy(bom => bom.EffectiveTo)
                .ToListAsync();
        }

        public async Task<IEnumerable<BillOfMaterial>> GetPendingApprovalAsync()
        {
            return await _dbSet
                .Include(bom => bom.ProductMaterialCard)
                .Where(bom => bom.ApprovalStatus == "PENDING")
                .OrderBy(bom => bom.CreateDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<BillOfMaterial>> GetObsoleteAsync()
        {
            return await _dbSet
                .Include(bom => bom.ProductMaterialCard)
                .Where(bom => bom.ApprovalStatus == "OBSOLETE")
                .OrderBy(bom => bom.CreateDate)
                .ToListAsync();
        }

        public async Task<BillOfMaterial?> GetWithItemsAsync(long id)
        {
            return await _dbSet
                .Include(bom => bom.ProductMaterialCard)
                .Include(bom => bom.BOMItems.OrderBy(item => item.LineNumber))
                    .ThenInclude(item => item.MaterialCard)
                .Include(bom => bom.BOMItems)
                    .ThenInclude(item => item.SupplierMaterialCard)
                .FirstOrDefaultAsync(bom => bom.Id == id);
        }

        public async Task<BillOfMaterial?> GetWithWorkOrdersAsync(long id)
        {
            return await _dbSet
                .Include(bom => bom.ProductMaterialCard)
                .Include(bom => bom.WorkOrders.OrderByDescending(wo => wo.CreateDate))
                    .ThenInclude(wo => wo.SupervisorUser)
                .FirstOrDefaultAsync(bom => bom.Id == id);
        }

        public async Task<IEnumerable<BillOfMaterial>> GetByRouteCodeAsync(string routeCode)
        {
            return await _dbSet
                .Include(bom => bom.ProductMaterialCard)
                .Where(bom => bom.RouteCode == routeCode)
                .OrderBy(bom => bom.BOMCode)
                .ToListAsync();
        }

        public async Task<IEnumerable<BillOfMaterial>> SearchAsync(string searchTerm)
        {
            var searchLower = searchTerm.ToLower();
            
            return await _dbSet
                .Include(bom => bom.ProductMaterialCard)
                .Where(bom => 
                    bom.BOMCode.ToLower().Contains(searchLower) ||
                    bom.BOMName.ToLower().Contains(searchLower) ||
                    bom.ProductMaterialCard.CardName.ToLower().Contains(searchLower) ||
                    bom.ProductMaterialCard.CardCode.ToLower().Contains(searchLower) ||
                    (bom.Description != null && bom.Description.ToLower().Contains(searchLower)))
                .OrderBy(bom => bom.BOMCode)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(string bomCode)
        {
            return await _dbSet
                .AnyAsync(bom => bom.BOMCode == bomCode);
        }

        public async Task<string> GetNextBOMCodeAsync()
        {
            var today = DateTime.Today;
            var yearMonth = today.ToString("yyyyMM");
            var prefix = $"BOM-{yearMonth}-";
            
            var lastBOM = await _dbSet
                .Where(bom => bom.BOMCode.StartsWith(prefix))
                .OrderByDescending(bom => bom.BOMCode)
                .FirstOrDefaultAsync();

            if (lastBOM == null)
            {
                return $"{prefix}0001";
            }

            var lastNumberPart = lastBOM.BOMCode.Substring(prefix.Length);
            if (int.TryParse(lastNumberPart, out int lastNumber))
            {
                return $"{prefix}{(lastNumber + 1):D4}";
            }

            return $"{prefix}0001";
        }

        public async Task<Dictionary<string, int>> GetBOMTypeStatisticsAsync()
        {
            return await _dbSet
                .Where(bom => bom.IsActive)
                .GroupBy(bom => bom.BOMType)
                .ToDictionaryAsync(g => g.Key, g => g.Count());
        }

        // Interface compatibility methods
        public async Task<IEnumerable<BillOfMaterial>> GetActiveBOMsAsync()
        {
            return await GetActiveAsync();
        }

        public async Task<IEnumerable<BillOfMaterial>> GetApprovedBOMsAsync()
        {
            return await _dbSet
                .Include(bom => bom.ProductMaterialCard)
                .Where(bom => bom.ApprovalStatus == "APPROVED")
                .OrderBy(bom => bom.BOMCode)
                .ToListAsync();
        }

        public async Task<IEnumerable<BillOfMaterial>> GetByTypeAsync(string bomType)
        {
            return await GetByBOMTypeAsync(bomType);
        }

        public async Task<IEnumerable<BillOfMaterial>> GetByApprovalStatusAsync(string approvalStatus)
        {
            return await GetByStatusAsync(approvalStatus);
        }

        public async Task<IEnumerable<BillOfMaterial>> GetEffectiveBOMsAsync(DateTime effectiveDate)
        {
            return await GetEffectiveAsync(effectiveDate);
        }

        public async Task<IEnumerable<BillOfMaterial>> GetVersionsAsync(string bomCode)
        {
            return await _dbSet
                .Include(bom => bom.ProductMaterialCard)
                .Where(bom => bom.BOMCode == bomCode)
                .OrderByDescending(bom => bom.Version)
                .ThenByDescending(bom => bom.CreateDate)
                .ToListAsync();
        }

        public async Task<BillOfMaterial?> GetLatestVersionAsync(string bomCode)
        {
            return await _dbSet
                .Include(bom => bom.ProductMaterialCard)
                .Include(bom => bom.BOMItems)
                    .ThenInclude(item => item.MaterialCard)
                .Where(bom => bom.BOMCode == bomCode)
                .OrderByDescending(bom => bom.Version)
                .ThenByDescending(bom => bom.CreateDate)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<BillOfMaterial>> GetForExportAsync()
        {
            return await _dbSet
                .Include(bom => bom.ProductMaterialCard)
                .Include(bom => bom.BOMItems)
                    .ThenInclude(item => item.MaterialCard)
                .Where(bom => bom.IsActive && bom.ApprovalStatus == "APPROVED")
                .OrderBy(bom => bom.BOMCode)
                .ToListAsync();
        }

        public async Task<IEnumerable<BillOfMaterial>> GetExpiredBOMsAsync()
        {
            return await GetExpiredAsync();
        }

        public async Task<IEnumerable<BillOfMaterial>> GetExpiringSoonAsync(int daysAhead = 30)
        {
            var today = DateTime.Today;
            var futureDate = today.AddDays(daysAhead);
            
            return await _dbSet
                .Include(bom => bom.ProductMaterialCard)
                .Where(bom => 
                    bom.EffectiveTo.HasValue && 
                    bom.EffectiveTo.Value >= today && 
                    bom.EffectiveTo.Value <= futureDate &&
                    bom.IsActive)
                .OrderBy(bom => bom.EffectiveTo)
                .ToListAsync();
        }

        public override async Task<List<BillOfMaterial>> GetAllAsync()
        {
            return await _dbSet
                .Include(bom => bom.ProductMaterialCard)
                .Include(bom => bom.BOMItems)
                    .ThenInclude(item => item.MaterialCard)
                .OrderByDescending(bom => bom.CreateDate)
                .ToListAsync();
        }

        public override async Task<BillOfMaterial?> GetByIdAsync(long id)
        {
            return await _dbSet
                .Include(bom => bom.ProductMaterialCard)
                .Include(bom => bom.BOMItems.OrderBy(item => item.LineNumber))
                    .ThenInclude(item => item.MaterialCard)
                .FirstOrDefaultAsync(bom => bom.Id == id);
        }
    }
} 