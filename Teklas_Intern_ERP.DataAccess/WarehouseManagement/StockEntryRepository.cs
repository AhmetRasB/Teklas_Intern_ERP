using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Teklas_Intern_ERP.Entities.WarehouseManagement;
using Teklas_Intern_ERP.DataAccess.Repositories;

namespace Teklas_Intern_ERP.DataAccess.WarehouseManagement
{
    public class StockEntryRepository : BaseRepository<StockEntry>, IStockEntryRepository
    {
        public StockEntryRepository(AppDbContext context) : base(context) { }

        #region Related Entity Methods

        public async Task<List<StockEntry>> GetStockEntriesByWarehouseAsync(long warehouseId)
        {
            return await _dbSet
                .Where(s => !s.IsDeleted && s.WarehouseId == warehouseId)
                .Include(s => s.Warehouse)
                .Include(s => s.Location)
                .Include(s => s.Material)
                .OrderByDescending(s => s.EntryDate)
                .ToListAsync();
        }

        public async Task<List<StockEntry>> GetStockEntriesByLocationAsync(long locationId)
        {
            return await _dbSet
                .Where(s => !s.IsDeleted && s.LocationId == locationId)
                .Include(s => s.Warehouse)
                .Include(s => s.Location)
                .Include(s => s.Material)
                .OrderByDescending(s => s.EntryDate)
                .ToListAsync();
        }

        public async Task<List<StockEntry>> GetStockEntriesByMaterialAsync(long materialId)
        {
            return await _dbSet
                .Where(s => !s.IsDeleted && s.MaterialId == materialId)
                .Include(s => s.Warehouse)
                .Include(s => s.Location)
                .Include(s => s.Material)
                .OrderByDescending(s => s.EntryDate)
                .ToListAsync();
        }

        public async Task<List<StockEntry>> GetStockEntriesWithDetailsAsync()
        {
            return await _dbSet
                .Where(s => !s.IsDeleted)
                .Include(s => s.Warehouse)
                .Include(s => s.Location)
                .Include(s => s.Material)
                .OrderByDescending(s => s.EntryDate)
                .ToListAsync();
        }

        #endregion

        #region Type Related Methods

        public async Task<List<StockEntry>> GetStockEntriesByTypeAsync(string entryType)
        {
            return await _dbSet
                .Where(s => !s.IsDeleted && s.EntryType == entryType)
                .Include(s => s.Warehouse)
                .Include(s => s.Location)
                .Include(s => s.Material)
                .OrderByDescending(s => s.EntryDate)
                .ToListAsync();
        }

        #endregion

        #region Search and Filter Methods

        public async Task<List<StockEntry>> SearchStockEntriesAsync(string searchTerm)
        {
            return await _dbSet
                .Where(s => !s.IsDeleted && (
                    s.EntryNumber.Contains(searchTerm) ||
                    (s.ReferenceNumber != null && s.ReferenceNumber.Contains(searchTerm)) ||
                    (s.BatchNumber != null && s.BatchNumber.Contains(searchTerm)) ||
                    (s.SerialNumber != null && s.SerialNumber.Contains(searchTerm)) ||
                    (s.Notes != null && s.Notes.Contains(searchTerm)) ||
                    (s.ResponsiblePerson != null && s.ResponsiblePerson.Contains(searchTerm))
                ))
                .Include(s => s.Warehouse)
                .Include(s => s.Location)
                .Include(s => s.Material)
                .OrderByDescending(s => s.EntryDate)
                .ToListAsync();
        }

        public async Task<bool> IsEntryNumberUniqueAsync(string entryNumber, long? excludeId = null)
        {
            var query = _dbSet.Where(s => s.EntryNumber == entryNumber);
            
            if (excludeId.HasValue)
                query = query.Where(s => s.Id != excludeId.Value);

            return !await query.AnyAsync();
        }

        #endregion
    }
} 