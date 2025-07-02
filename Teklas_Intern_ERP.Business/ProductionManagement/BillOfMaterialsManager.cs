using Teklas_Intern_ERP.DataAccess.ProductionManagement;
using Teklas_Intern_ERP.Entities.ProductionManagement;
using System.Collections.Generic;
using Teklas_Intern_ERP.DataAccess;
using System.Threading.Tasks;

namespace Teklas_Intern_ERP.Business.ProductionManagement
{
    public class BillOfMaterialsManager
    {
        private readonly BillOfMaterialsRepository _repo;
        public BillOfMaterialsManager(BillOfMaterialsRepository repo)
        {
            _repo = repo;
        }

        public List<BillOfMaterials> GetAll() => _repo.GetAll();
        public BillOfMaterials GetById(int id) => _repo.GetById(id);
        public BillOfMaterials Add(BillOfMaterials bom) => _repo.Add(bom);
        public bool Update(BillOfMaterials bom) => _repo.Update(bom);
        public bool Delete(int id) => _repo.Delete(id);

        public async Task<List<BillOfMaterials>> GetAllAsync() => await _repo.GetAllAsync();
        public async Task<BillOfMaterials> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);
        public async Task<BillOfMaterials> AddAsync(BillOfMaterials bom) => await _repo.AddAsync(bom);
        public async Task<bool> UpdateAsync(BillOfMaterials bom) => await _repo.UpdateAsync(bom);
        public async Task<bool> DeleteAsync(int id) => await _repo.DeleteAsync(id);
    }
} 