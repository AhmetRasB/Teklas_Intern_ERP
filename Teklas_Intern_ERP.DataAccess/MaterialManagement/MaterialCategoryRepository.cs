using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Teklas_Intern_ERP.Entities.MaterialManagement;
using Teklas_Intern_ERP.DataAccess.Repositories;

namespace Teklas_Intern_ERP.DataAccess.MaterialManagement
{
    public interface IMaterialCategoryRepository : IRepository<MaterialCategory>
    {
        // Category Hierarchy Methods
        Task<List<MaterialCategory>> GetRootCategoriesAsync();
        Task<List<MaterialCategory>> GetSubCategoriesAsync(long parentId);
        Task<List<MaterialCategory>> GetCategoryHierarchyAsync(long categoryId);
        Task<List<MaterialCategory>> GetAllCategoriesWithChildrenAsync();
        Task<MaterialCategory?> GetCategoryWithMaterialsAsync(long categoryId);
        
        // Material Relationship Methods
        Task<List<MaterialCategory>> GetCategoriesWithMaterialCountAsync();
        Task<List<MaterialCategory>> GetEmptyCategoriesAsync();
        Task<bool> HasMaterialsAsync(long categoryId);
        Task<int> GetMaterialCountAsync(long categoryId);
        
        // Business Logic Methods
        Task<bool> CanDeleteCategoryAsync(long categoryId);
        Task<List<MaterialCategory>> GetCategoriesByManagerAsync(string managerName);
        Task<List<MaterialCategory>> GetCategoriesByCostCenterAsync(string costCenter);
        Task<List<MaterialCategory>> GetActiveCategoriesAsync();
    }

    public class MaterialCategoryRepository : BaseRepository<MaterialCategory>, IMaterialCategoryRepository
    {
        public MaterialCategoryRepository(AppDbContext context) : base(context) { }

        #region Category Hierarchy Methods

        public async Task<List<MaterialCategory>> GetRootCategoriesAsync()
        {
            return await _dbSet
                .Where(c => !c.IsDeleted && c.ParentCategoryId == null)
                .Include(c => c.ChildCategories.Where(sc => !sc.IsDeleted))
                .OrderBy(c => c.CategoryName)
                .ToListAsync();
        }

        public async Task<List<MaterialCategory>> GetSubCategoriesAsync(long parentId)
        {
            return await _dbSet
                .Where(c => !c.IsDeleted && c.ParentCategoryId == parentId)
                .Include(c => c.ChildCategories.Where(sc => !sc.IsDeleted))
                .OrderBy(c => c.CategoryName)
                .ToListAsync();
        }

        public async Task<List<MaterialCategory>> GetCategoryHierarchyAsync(long categoryId)
        {
            var hierarchy = new List<MaterialCategory>();
            var category = await GetByIdAsync(categoryId, "ParentCategory");
            
            while (category != null)
            {
                hierarchy.Insert(0, category);
                category = category.ParentCategory;
            }
            
            return hierarchy;
        }

        public async Task<List<MaterialCategory>> GetAllCategoriesWithChildrenAsync()
        {
            return await _dbSet
                .Where(c => !c.IsDeleted)
                .Include(c => c.ChildCategories.Where(sc => !sc.IsDeleted))
                .Include(c => c.MaterialCards.Where(m => !m.IsDeleted))
                .OrderBy(c => c.CategoryName)
                .ToListAsync();
        }

        public async Task<MaterialCategory?> GetCategoryWithMaterialsAsync(long categoryId)
        {
            return await _dbSet
                .Where(c => !c.IsDeleted && c.Id == categoryId)
                .Include(c => c.MaterialCards.Where(m => !m.IsDeleted))
                .Include(c => c.ChildCategories.Where(sc => !sc.IsDeleted))
                .FirstOrDefaultAsync();
        }

        #endregion

        #region Material Relationship Methods

        public async Task<List<MaterialCategory>> GetCategoriesWithMaterialCountAsync()
        {
            return await _dbSet
                .Where(c => !c.IsDeleted)
                .Include(c => c.MaterialCards.Where(m => !m.IsDeleted))
                .ToListAsync();
        }

        public async Task<List<MaterialCategory>> GetEmptyCategoriesAsync()
        {
            return await _dbSet
                .Where(c => !c.IsDeleted && 
                           !c.MaterialCards.Any(m => !m.IsDeleted) &&
                           !c.ChildCategories.Any(sc => !sc.IsDeleted))
                .ToListAsync();
        }

        public async Task<bool> HasMaterialsAsync(long categoryId)
        {
            return await _dbSet
                .Where(c => c.Id == categoryId && !c.IsDeleted)
                .AnyAsync(c => c.MaterialCards.Any(m => !m.IsDeleted));
        }

        public async Task<int> GetMaterialCountAsync(long categoryId)
        {
            var category = await _dbSet
                .Where(c => c.Id == categoryId && !c.IsDeleted)
                .Include(c => c.MaterialCards)
                .FirstOrDefaultAsync();

            return category?.MaterialCards?.Count(m => !m.IsDeleted) ?? 0;
        }

        #endregion

        #region Business Logic Methods

        public async Task<bool> CanDeleteCategoryAsync(long categoryId)
        {
            var category = await _dbSet
                .Where(c => c.Id == categoryId && !c.IsDeleted)
                .Include(c => c.MaterialCards)
                .Include(c => c.ChildCategories)
                .FirstOrDefaultAsync();

            if (category == null) return false;

            // Cannot delete if has active materials or subcategories
            return !category.MaterialCards.Any(m => !m.IsDeleted) && 
                   !category.ChildCategories.Any(sc => !sc.IsDeleted);
        }

        public async Task<List<MaterialCategory>> GetCategoriesByManagerAsync(string managerName)
        {
            return await _dbSet
                .Where(c => !c.IsDeleted)
                .Include(c => c.MaterialCards.Where(m => !m.IsDeleted))
                .OrderBy(c => c.CategoryName)
                .ToListAsync();
        }

        public async Task<List<MaterialCategory>> GetCategoriesByCostCenterAsync(string costCenter)
        {
            return await _dbSet
                .Where(c => !c.IsDeleted)
                .Include(c => c.MaterialCards.Where(m => !m.IsDeleted))
                .OrderBy(c => c.CategoryName)
                .ToListAsync();
        }

        public async Task<List<MaterialCategory>> GetActiveCategoriesAsync()
        {
            return await _dbSet
                .Where(c => !c.IsDeleted && c.Status == "Active")
                .OrderBy(c => c.CategoryName)
                .ToListAsync();
        }

        #endregion
    }
} 