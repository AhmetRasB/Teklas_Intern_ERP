using System.Collections.Generic;
using System.Threading.Tasks;
using Teklas_Intern_ERP.Entities.MaterialManagement;

namespace Teklas_Intern_ERP.Business.Interfaces
{
    public interface IMaterialCardService
    {
        Task<List<MaterialCard>> GetAllAsync();
        Task<MaterialCard> GetByIdAsync(int id);
        Task<MaterialCard> AddAsync(MaterialCard card);
        Task<bool> UpdateAsync(MaterialCard card);
        Task<bool> DeleteAsync(int id);
        Task<List<MaterialCard>> GetDeletedAsync();
        Task<bool> RestoreAsync(int id);
        Task<bool> PermanentDeleteAsync(int id);
    }
} 