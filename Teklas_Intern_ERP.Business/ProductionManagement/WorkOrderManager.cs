using Teklas_Intern_ERP.DataAccess.ProductionManagement;
using Teklas_Intern_ERP.Entities.ProductionManagement;
using System.Collections.Generic;
using Teklas_Intern_ERP.DataAccess;

namespace Teklas_Intern_ERP.Business.ProductionManagement
{
    public class WorkOrderManager
    {
        private readonly WorkOrderRepository _repo;
        public WorkOrderManager(AppDbContext context)
        {
            _repo = new WorkOrderRepository(context);
        }

        public List<WorkOrder> GetAll() => _repo.GetAll();
        public WorkOrder GetById(int id) => _repo.GetById(id);
        public WorkOrder Add(WorkOrder order) => _repo.Add(order);
        public bool Update(WorkOrder order) => _repo.Update(order);
        public bool Delete(int id) => _repo.Delete(id);
    }
} 