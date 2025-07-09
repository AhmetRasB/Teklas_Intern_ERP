using System.Collections.Generic;
using System.Threading.Tasks;
using Teklas_Intern_ERP.DTOs;

namespace Teklas_Intern_ERP.Business.Interfaces
{
    public interface IMaterialCategoryService
    {
        // Basic CRUD Operations
        Task<List<MaterialCategoryDto>> GetAllAsync();
        Task<MaterialCategoryDto?> GetByIdAsync(long id);
        Task<MaterialCategoryDto> AddAsync(MaterialCategoryDto dto);
        Task<MaterialCategoryDto> UpdateAsync(MaterialCategoryDto dto);
        Task<bool> DeleteAsync(long id);
        Task<List<MaterialCategoryDto>> GetDeletedAsync();
        Task<bool> RestoreAsync(long id);
        Task<bool> PermanentDeleteAsync(long id);
        
        // Basic Search Operations
        Task<List<MaterialCategoryDto>> GetRootCategoriesAsync();
        Task<List<MaterialCategoryDto>> GetSubCategoriesAsync(long parentId);
        Task<bool> HasMaterialsAsync(long categoryId);
        Task<int> GetMaterialCountAsync(long categoryId);
    }
} 