using System.Collections.Generic;
using System.Linq;
using Teklas_Intern_ERP.Entities.ProductionManagement;
using Teklas_Intern_ERP.DataAccess;

namespace Teklas_Intern_ERP.DataAccess.ProductionManagement
{
    public class WorkOrderRepository
    {
        private readonly AppDbContext _context;
        public WorkOrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<WorkOrder> GetAll() => _context.WorkOrders.ToList();
        public WorkOrder GetById(int id) => _context.WorkOrders.Find(id);
        public WorkOrder Add(WorkOrder order)
        {
            _context.WorkOrders.Add(order);
            _context.SaveChanges();
            return order;
        }
        public bool Update(WorkOrder order)
        {
            var existing = _context.WorkOrders.Find(order.Id);
            if (existing == null) return false;
            existing.WorkOrderNo = order.WorkOrderNo;
            existing.PlannedStartDate = order.PlannedStartDate;
            _context.SaveChanges();
            return true;
        }
        public bool Delete(int id)
        {
            var order = _context.WorkOrders.Find(id);
            if (order == null) return false;
            _context.WorkOrders.Remove(order);
            _context.SaveChanges();
            return true;
        }
    }
} 