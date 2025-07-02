using Teklas_Intern_ERP.DataAccess.MaterialManagement;
using Teklas_Intern_ERP.Entities.MaterialManagement;
using System.Collections.Generic;
using Teklas_Intern_ERP.DataAccess;
using System.Threading.Tasks;

namespace Teklas_Intern_ERP.Business.MaterialManagement
{
    public class MaterialCategoryManager
    {
        private readonly MaterialCategoryRepository _repo;
        public MaterialCategoryManager(MaterialCategoryRepository repo)
        {
            _repo = repo;
        }

        public List<MaterialCategory> GetAll() => _repo.GetAll();
        public MaterialCategory GetById(int id) => _repo.GetById(id);
        public MaterialCategory Add(MaterialCategory category) => _repo.Add(category);
        public bool Update(MaterialCategory category) => _repo.Update(category);
        public bool Delete(int id) => _repo.Delete(id);

        public async Task<List<MaterialCategory>> GetAllAsync() => await _repo.GetAllAsync();
        public async Task<MaterialCategory> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);
        public async Task<MaterialCategory> AddAsync(MaterialCategory category) => await _repo.AddAsync(category);
        public async Task<bool> UpdateAsync(MaterialCategory category) => await _repo.UpdateAsync(category);
        public async Task<bool> DeleteAsync(int id) => await _repo.DeleteAsync(id);
    }
} 