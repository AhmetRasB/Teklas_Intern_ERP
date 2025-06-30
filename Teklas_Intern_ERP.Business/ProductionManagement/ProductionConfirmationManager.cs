using Teklas_Intern_ERP.DataAccess.ProductionManagement;
using Teklas_Intern_ERP.Entities.ProductionManagement;
using System.Collections.Generic;
using Teklas_Intern_ERP.DataAccess;

namespace Teklas_Intern_ERP.Business.ProductionManagement
{
    public class ProductionConfirmationManager
    {
        private readonly ProductionConfirmationRepository _repo;
        public ProductionConfirmationManager(AppDbContext context)
        {
            _repo = new ProductionConfirmationRepository(context);
        }

        public List<ProductionConfirmation> GetAll() => _repo.GetAll();
        public ProductionConfirmation GetById(int id) => _repo.GetById(id);
        public ProductionConfirmation Add(ProductionConfirmation confirmation) => _repo.Add(confirmation);
        public bool Update(ProductionConfirmation confirmation) => _repo.Update(confirmation);
        public bool Delete(int id) => _repo.Delete(id);
    }
} 