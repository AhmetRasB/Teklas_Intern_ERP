using System.Collections.Generic;
using System.Linq;
using Teklas_Intern_ERP.Entities.ProductionManagement;
using Teklas_Intern_ERP.DataAccess;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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

        public async Task<List<WorkOrder>> GetAllAsync() => await _context.WorkOrders.ToListAsync();
        public async Task<WorkOrder> GetByIdAsync(int id) => await _context.WorkOrders.FindAsync(id);
        public async Task<WorkOrder> AddAsync(WorkOrder order)
        {
            _context.WorkOrders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }
        public async Task<bool> UpdateAsync(WorkOrder order)
        {
            var existing = await _context.WorkOrders.FindAsync(order.Id);
            if (existing == null) return false;
            existing.WorkOrderNo = order.WorkOrderNo;
            existing.PlannedStartDate = order.PlannedStartDate;
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var order = await _context.WorkOrders.FindAsync(id);
            if (order == null) return false;
            _context.WorkOrders.Remove(order);
            await _context.SaveChangesAsync();
            return true;
        }
    }
} 