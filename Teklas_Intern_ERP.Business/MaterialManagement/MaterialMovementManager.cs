using Teklas_Intern_ERP.DataAccess.MaterialManagement;
using Teklas_Intern_ERP.Entities.MaterialManagement;
using System.Collections.Generic;
using Teklas_Intern_ERP.DataAccess;
using System.Threading.Tasks;

namespace Teklas_Intern_ERP.Business.MaterialManagement
{
    public class MaterialMovementManager
    {
        private readonly MaterialMovementRepository _repo;
        public MaterialMovementManager(MaterialMovementRepository repo)
        {
            _repo = repo;
        }

        public List<MaterialMovement> GetAll() => _repo.GetAll();
        public MaterialMovement GetById(int id) => _repo.GetById(id);
        public MaterialMovement Add(MaterialMovement movement) => _repo.Add(movement);
        public bool Update(MaterialMovement movement) => _repo.Update(movement);
        public bool Delete(int id) => _repo.Delete(id);

        public async Task<List<MaterialMovement>> GetAllAsync() => await _repo.GetAllAsync();
        public async Task<MaterialMovement> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);
        public async Task<MaterialMovement> AddAsync(MaterialMovement movement) => await _repo.AddAsync(movement);
        public async Task<bool> UpdateAsync(MaterialMovement movement) => await _repo.UpdateAsync(movement);
        public async Task<bool> DeleteAsync(int id) => await _repo.DeleteAsync(id);
    }
} 