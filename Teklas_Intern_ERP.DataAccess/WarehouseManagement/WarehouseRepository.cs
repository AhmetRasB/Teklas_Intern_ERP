using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Teklas_Intern_ERP.Entities.WarehouseManagement;
using Teklas_Intern_ERP.DataAccess.Repositories;

namespace Teklas_Intern_ERP.DataAccess.WarehouseManagement
{
    public class WarehouseRepository : BaseRepository<Warehouse>, IWarehouseRepository
    {
        public WarehouseRepository(AppDbContext context) : base(context) { }

        #region Type Related Methods

        public async Task<List<Warehouse>> GetWarehousesByTypeAsync(string warehouseType)
        {
            return await _dbSet
                .Where(w => !w.IsDeleted && w.WarehouseType == warehouseType)
                .OrderBy(w => w.WarehouseName)
                .ToListAsync();
        }

        #endregion

        #region Search and Filter Methods

        public async Task<List<Warehouse>> SearchWarehousesAsync(string searchTerm)
        {
            return await _dbSet
                .Where(w => !w.IsDeleted && (
                    w.WarehouseCode.Contains(searchTerm) ||
                    w.WarehouseName.Contains(searchTerm) ||
                    (w.Description != null && w.Description.Contains(searchTerm)) ||
                    (w.City != null && w.City.Contains(searchTerm)) ||
                    (w.ManagerName != null && w.ManagerName.Contains(searchTerm))
                ))
                .OrderBy(w => w.WarehouseName)
                .ToListAsync();
        }

        public async Task<bool> IsWarehouseCodeUniqueAsync(string code, long? excludeId = null)
        {
            var query = _dbSet.Where(w => w.WarehouseCode == code);
            
            if (excludeId.HasValue)
                query = query.Where(w => w.Id != excludeId.Value);

            return !await query.AnyAsync();
        }

        #endregion
    }
} 