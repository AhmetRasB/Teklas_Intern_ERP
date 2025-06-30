using Teklas_Intern_ERP.DataAccess.PurchasingManagement;
using Teklas_Intern_ERP.Entities.PurchasingManagement;
using System.Collections.Generic;
using Teklas_Intern_ERP.DataAccess;

namespace Teklas_Intern_ERP.Business.PurchasingManagement
{
    public class SupplierTypeManager
    {
        private readonly SupplierTypeRepository _repo;
        public SupplierTypeManager(AppDbContext context)
        {
            _repo = new SupplierTypeRepository(context);
        }

        public List<SupplierType> GetAll() => _repo.GetAll();
        public SupplierType GetById(int id) => _repo.GetById(id);
        public SupplierType Add(SupplierType type) => _repo.Add(type);
        public bool Update(SupplierType type) => _repo.Update(type);
        public bool Delete(int id) => _repo.Delete(id);
    }
} 