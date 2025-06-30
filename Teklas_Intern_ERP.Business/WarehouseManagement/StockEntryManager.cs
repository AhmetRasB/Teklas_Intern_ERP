using Teklas_Intern_ERP.DataAccess.WarehouseManagement;
using Teklas_Intern_ERP.Entities.WarehouseManagement;
using System.Collections.Generic;

namespace Teklas_Intern_ERP.Business.WarehouseManagement
{
    public class StockEntryManager
    {
        private readonly StockEntryRepository _repo = new StockEntryRepository();

        public List<StockEntry> GetAll() => _repo.GetAll();
        public StockEntry GetById(int id) => _repo.GetById(id);
        public StockEntry Add(StockEntry entry) => _repo.Add(entry);
        public bool Update(StockEntry entry) => _repo.Update(entry);
        public bool Delete(int id) => _repo.Delete(id);
    }
} 