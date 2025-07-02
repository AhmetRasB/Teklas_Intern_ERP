using Teklas_Intern_ERP.DataAccess.ProductionManagement;
using Teklas_Intern_ERP.Entities.ProductionManagement;
using System.Collections.Generic;
using Teklas_Intern_ERP.DataAccess;
using System.Threading.Tasks;

namespace Teklas_Intern_ERP.Business.ProductionManagement
{
    public class ProductionConfirmationManager
    {
        private readonly ProductionConfirmationRepository _repo;
        public ProductionConfirmationManager(ProductionConfirmationRepository repo)
        {
            _repo = repo;
        }

        public List<ProductionConfirmation> GetAll() => _repo.GetAll();
        public ProductionConfirmation GetById(int id) => _repo.GetById(id);
        public ProductionConfirmation Add(ProductionConfirmation confirmation) => _repo.Add(confirmation);
        public bool Update(ProductionConfirmation confirmation) => _repo.Update(confirmation);
        public bool Delete(int id) => _repo.Delete(id);

        public async Task<List<ProductionConfirmation>> GetAllAsync() => await _repo.GetAllAsync();
        public async Task<ProductionConfirmation> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);
        public async Task<ProductionConfirmation> AddAsync(ProductionConfirmation confirmation) => await _repo.AddAsync(confirmation);
        public async Task<bool> UpdateAsync(ProductionConfirmation confirmation) => await _repo.UpdateAsync(confirmation);
        public async Task<bool> DeleteAsync(int id) => await _repo.DeleteAsync(id);
    }
} 