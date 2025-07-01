using System.Collections.Generic;
using System.Linq;
using Teklas_Intern_ERP.Entities.SalesOrderManagement;
using Teklas_Intern_ERP.DataAccess;

namespace Teklas_Intern_ERP.DataAccess.SalesOrderManagement
{
    public class CustomerOrderRepository
    {
        private readonly AppDbContext _context;
        public CustomerOrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<CustomerOrder> GetAll() => _context.CustomerOrders.ToList();
        public CustomerOrder GetById(int id) => _context.CustomerOrders.Find(id);
        public CustomerOrder Add(CustomerOrder order)
        {
            _context.CustomerOrders.Add(order);
            _context.SaveChanges();
            return order;
        }
        public bool Update(CustomerOrder order)
        {
            var existing = _context.CustomerOrders.Find(order.Id);
            if (existing == null) return false;
            existing.OrderNo = order.OrderNo;
            existing.OrderDate = order.OrderDate;
            _context.SaveChanges();
            return true;
        }
        public bool Delete(int id)
        {
            var order = _context.CustomerOrders.Find(id);
            if (order == null) return false;
            _context.CustomerOrders.Remove(order);
            _context.SaveChanges();
            return true;
        }
    }
} 