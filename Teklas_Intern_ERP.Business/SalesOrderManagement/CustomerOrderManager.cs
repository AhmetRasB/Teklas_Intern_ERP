using Teklas_Intern_ERP.DataAccess.SalesOrderManagement;
using Teklas_Intern_ERP.Entities.SalesOrderManagement;
using System.Collections.Generic;
using Teklas_Intern_ERP.DataAccess;

namespace Teklas_Intern_ERP.Business.SalesOrderManagement
{
    public class CustomerOrderManager
    {
        private readonly CustomerOrderRepository _repo;
        public CustomerOrderManager(AppDbContext context)
        {
            _repo = new CustomerOrderRepository(context);
        }

        public List<CustomerOrder> GetAll() => _repo.GetAll();
        public CustomerOrder GetById(int id) => _repo.GetById(id);
        public CustomerOrder Add(CustomerOrder order) => _repo.Add(order);
        public bool Update(CustomerOrder order) => _repo.Update(order);
        public bool Delete(int id) => _repo.Delete(id);
    }
} 