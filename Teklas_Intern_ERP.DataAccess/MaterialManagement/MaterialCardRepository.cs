using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Teklas_Intern_ERP.Entities.MaterialManagement;
using Teklas_Intern_ERP.DataAccess.Repositories;
using Teklas_Intern_ERP.DTOs;

namespace Teklas_Intern_ERP.DataAccess.MaterialManagement
{
    public interface IMaterialCardRepository : IRepository<MaterialCard>
    {
        Task<List<MaterialCard>> GetMaterialsByCategoryAsync(long categoryId);
        Task<List<MaterialCard>> GetMaterialsWithCategoryAsync();
        Task<List<MaterialCard>> SearchMaterialsAsync(string searchTerm);
        Task<bool> IsMaterialCodeUniqueAsync(string code, long? excludeId = null);
        Task<MaterialCard?> GetMaterialByBarcodeAsync(string barcode);
    }

    public class MaterialCardRepository : BaseRepository<MaterialCard>, IMaterialCardRepository
    {
        public MaterialCardRepository(AppDbContext context) : base(context) { }

        #region Category Related Methods

        public async Task<List<MaterialCard>> GetMaterialsByCategoryAsync(long categoryId)
        {
            return await _dbSet
                .Where(m => !m.IsDeleted && m.CategoryId == categoryId)
                .Include(m => m.Category)
                .OrderBy(m => m.Name)
                .ToListAsync();
        }

        public async Task<List<MaterialCard>> GetMaterialsWithCategoryAsync()
        {
            return await _dbSet
                .Where(m => !m.IsDeleted)
                .Include(m => m.Category)
                .OrderBy(m => m.Name)
                .ToListAsync();
        }

        #endregion

        #region Search and Filter Methods

        public async Task<List<MaterialCard>> SearchMaterialsAsync(string searchTerm)
        {
            return await _dbSet
                .Where(m => !m.IsDeleted && (
                    m.Code.Contains(searchTerm) ||
                    m.Name.Contains(searchTerm) ||
                    (m.Description != null && m.Description.Contains(searchTerm)) ||
                    (m.Brand != null && m.Brand.Contains(searchTerm))
                ))
                .Include(m => m.Category)
                .OrderBy(m => m.Name)
                .ToListAsync();
        }

        public async Task<bool> IsMaterialCodeUniqueAsync(string code, long? excludeId = null)
        {
            var query = _dbSet.Where(m => m.Code == code);
            
            if (excludeId.HasValue)
                query = query.Where(m => m.Id != excludeId.Value);

            return !await query.AnyAsync();
        }

        public async Task<MaterialCard?> GetMaterialByBarcodeAsync(string barcode)
        {
            return await _dbSet
                .Where(m => !m.IsDeleted && m.Barcode == barcode)
                .Include(m => m.Category)
                .FirstOrDefaultAsync();
        }

        #endregion
    }
} 