using Teklas_Intern_ERP.DataAccess.ProductionManagement;
using Teklas_Intern_ERP.Entities.ProductionManagement;
using System.Collections.Generic;
using Teklas_Intern_ERP.DataAccess;
using System.Threading.Tasks;

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

        public async Task<List<WorkOrder>> GetAllAsync() => await _repo.GetAllAsync();
        public async Task<WorkOrder> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);
        public async Task<WorkOrder> AddAsync(WorkOrder order) => await _repo.AddAsync(order);
        public async Task<bool> UpdateAsync(WorkOrder order) => await _repo.UpdateAsync(order);
        public async Task<bool> DeleteAsync(int id) => await _repo.DeleteAsync(id);
    }
} 