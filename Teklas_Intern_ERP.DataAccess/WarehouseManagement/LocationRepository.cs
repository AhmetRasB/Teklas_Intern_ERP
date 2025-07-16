using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Teklas_Intern_ERP.Entities.WarehouseManagement;
using Teklas_Intern_ERP.DataAccess.Repositories;

namespace Teklas_Intern_ERP.DataAccess.WarehouseManagement
{
    public class LocationRepository : BaseRepository<Location>, ILocationRepository
    {
        public LocationRepository(AppDbContext context) : base(context) { }

        #region Warehouse Related Methods

        public async Task<List<Location>> GetLocationsByWarehouseAsync(long warehouseId)
        {
            return await _dbSet
                .Where(l => !l.IsDeleted && l.WarehouseId == warehouseId)
                .Include(l => l.Warehouse)
                .OrderBy(l => l.LocationName)
                .ToListAsync();
        }

        public async Task<List<Location>> GetLocationsWithWarehouseAsync()
        {
            return await _dbSet
                .Where(l => !l.IsDeleted)
                .Include(l => l.Warehouse)
                .OrderBy(l => l.LocationName)
                .ToListAsync();
        }

        #endregion

        #region Type Related Methods

        public async Task<List<Location>> GetLocationsByTypeAsync(string locationType)
        {
            return await _dbSet
                .Where(l => !l.IsDeleted && l.LocationType == locationType)
                .Include(l => l.Warehouse)
                .OrderBy(l => l.LocationName)
                .ToListAsync();
        }

        #endregion

        #region Search and Filter Methods

        public async Task<List<Location>> SearchLocationsAsync(string searchTerm)
        {
            return await _dbSet
                .Where(l => !l.IsDeleted && (
                    l.LocationCode.Contains(searchTerm) ||
                    l.LocationName.Contains(searchTerm) ||
                    (l.Description != null && l.Description.Contains(searchTerm)) ||
                    (l.Aisle != null && l.Aisle.Contains(searchTerm)) ||
                    (l.Rack != null && l.Rack.Contains(searchTerm))
                ))
                .Include(l => l.Warehouse)
                .OrderBy(l => l.LocationName)
                .ToListAsync();
        }

        public async Task<bool> IsLocationCodeUniqueAsync(string code, long? excludeId = null)
        {
            var query = _dbSet.Where(l => l.LocationCode == code);
            
            if (excludeId.HasValue)
                query = query.Where(l => l.Id != excludeId.Value);

            return !await query.AnyAsync();
        }

        #endregion
    }
} 