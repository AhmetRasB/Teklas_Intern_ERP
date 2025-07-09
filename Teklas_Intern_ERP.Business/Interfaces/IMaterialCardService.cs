using System.Collections.Generic;
using System.Threading.Tasks;
using Teklas_Intern_ERP.DTOs;

namespace Teklas_Intern_ERP.Business.Interfaces
{
    public interface IMaterialCardService
    {
        // Basic CRUD Operations
        Task<List<MaterialCardDto>> GetAllAsync();
        Task<MaterialCardDto?> GetByIdAsync(long id);
        Task<MaterialCardDto> AddAsync(MaterialCardDto dto);
        Task<MaterialCardDto> UpdateAsync(MaterialCardDto dto);
        Task<bool> DeleteAsync(long id);
        Task<List<MaterialCardDto>> GetDeletedAsync();
        Task<bool> RestoreAsync(long id);
        Task<bool> PermanentDeleteAsync(long id);
        
        // Basic Search Operations
        Task<List<MaterialCardDto>> GetMaterialsByCategoryAsync(long categoryId);
        Task<List<MaterialCardDto>> SearchMaterialsAsync(string searchTerm);
        Task<MaterialCardDto?> GetMaterialByBarcodeAsync(string barcode);
        Task<bool> IsMaterialCodeUniqueAsync(string code, long? excludeId = null);
    }
} 