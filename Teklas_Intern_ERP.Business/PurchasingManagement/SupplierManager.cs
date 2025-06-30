using Teklas_Intern_ERP.DataAccess.PurchasingManagement;
using Teklas_Intern_ERP.Entities.PurchasingManagement;
using System.Collections.Generic;
using Teklas_Intern_ERP.DataAccess;

namespace Teklas_Intern_ERP.Business.PurchasingManagement
{
    public class SupplierManager
    {
        private readonly SupplierRepository _repo;
        public SupplierManager(AppDbContext context)
        {
            _repo = new SupplierRepository(context);
        }

        public List<Supplier> GetAll() => _repo.GetAll();
        public Supplier GetById(int id) => _repo.GetById(id);
        public Supplier Add(Supplier supplier) => _repo.Add(supplier);
        public bool Update(Supplier supplier) => _repo.Update(supplier);
        public bool Delete(int id) => _repo.Delete(id);
    }
} 