using Teklas_Intern_ERP.DataAccess.PurchasingManagement;
using Teklas_Intern_ERP.Entities.PurchasingManagement;
using System.Collections.Generic;
using Teklas_Intern_ERP.DataAccess;

namespace Teklas_Intern_ERP.Business.PurchasingManagement
{
    public class PurchaseOrderManager
    {
        private readonly PurchaseOrderRepository _repo;
        public PurchaseOrderManager(AppDbContext context)
        {
            _repo = new PurchaseOrderRepository(context);
        }

        public List<PurchaseOrder> GetAll() => _repo.GetAll();
        public PurchaseOrder GetById(int id) => _repo.GetById(id);
        public PurchaseOrder Add(PurchaseOrder order) => _repo.Add(order);
        public bool Update(PurchaseOrder order) => _repo.Update(order);
        public bool Delete(int id) => _repo.Delete(id);
    }
} 