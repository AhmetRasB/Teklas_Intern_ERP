using System.Collections.Generic;
using System.Linq;
using Teklas_Intern_ERP.Entities.PurchasingManagement;
using Teklas_Intern_ERP.DataAccess;

namespace Teklas_Intern_ERP.DataAccess.PurchasingManagement
{
    public class PurchaseOrderRepository
    {
        private readonly AppDbContext _context;
        public PurchaseOrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<PurchaseOrder> GetAll() => _context.PurchaseOrders.ToList();
        public PurchaseOrder GetById(int id) => _context.PurchaseOrders.Find(id);
        public PurchaseOrder Add(PurchaseOrder order)
        {
            _context.PurchaseOrders.Add(order);
            _context.SaveChanges();
            return order;
        }
        public bool Update(PurchaseOrder order)
        {
            var existing = _context.PurchaseOrders.Find(order.Id);
            if (existing == null) return false;
            existing.OrderNumber = order.OrderNumber;
            existing.Date = order.Date;
            _context.SaveChanges();
            return true;
        }
        public bool Delete(int id)
        {
            var order = _context.PurchaseOrders.Find(id);
            if (order == null) return false;
            _context.PurchaseOrders.Remove(order);
            _context.SaveChanges();
            return true;
        }
    }
} 