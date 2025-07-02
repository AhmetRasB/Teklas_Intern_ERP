using Teklas_Intern_ERP.DataAccess.WarehouseManagement;
using Teklas_Intern_ERP.Entities.WarehouseManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Teklas_Intern_ERP.Business.WarehouseManagement
{
    public class StockEntryManager
    {
        private readonly StockEntryRepository _repo;

        public StockEntryManager(StockEntryRepository repo)
        {
            _repo = repo;
        }
        public StockEntryManager() : this(new StockEntryRepository()) { }

        public List<StockEntry> GetAll() => _repo.GetAll();
        public StockEntry GetById(int id) => _repo.GetById(id);
        public StockEntry Add(StockEntry entry) => _repo.Add(entry);
        public bool Update(StockEntry entry) => _repo.Update(entry);
        public bool Delete(int id) => _repo.Delete(id);

        public async Task<List<StockEntry>> GetAllAsync() => await _repo.GetAllAsync();
        public async Task<StockEntry> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);
        public async Task<StockEntry> AddAsync(StockEntry entry) => await _repo.AddAsync(entry);
        public async Task<bool> UpdateAsync(StockEntry entry) => await _repo.UpdateAsync(entry);
        public async Task<bool> DeleteAsync(int id) => await _repo.DeleteAsync(id);
    }
} 