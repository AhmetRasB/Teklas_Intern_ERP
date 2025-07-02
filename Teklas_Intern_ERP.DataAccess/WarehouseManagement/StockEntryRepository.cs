using System.Collections.Generic;
using System.Linq;
using Teklas_Intern_ERP.Entities.WarehouseManagement;
using System.Threading.Tasks;

namespace Teklas_Intern_ERP.DataAccess.WarehouseManagement
{
    public class StockEntryRepository
    {
        private static List<StockEntry> _entries = new List<StockEntry>();
        private static int _nextId = 1;

        public List<StockEntry> GetAll() => _entries;
        public StockEntry GetById(int id) => _entries.FirstOrDefault(x => x.Id == id);
        public StockEntry Add(StockEntry entry)
        {
            entry.Id = _nextId++;
            _entries.Add(entry);
            return entry;
        }
        public bool Update(StockEntry entry)
        {
            var existing = GetById(entry.Id);
            if (existing == null) return false;
            existing.WarehouseId = entry.WarehouseId;
            existing.Quantity = entry.Quantity;
            existing.EntryDate = entry.EntryDate;
            return true;
        }
        public bool Delete(int id)
        {
            var entry = GetById(id);
            if (entry == null) return false;
            _entries.Remove(entry);
            return true;
        }
        public async Task<List<StockEntry>> GetAllAsync()
        {
            return await Task.Run(() => GetAll());
        }
        public async Task<StockEntry> GetByIdAsync(int id)
        {
            return await Task.Run(() => GetById(id));
        }
        public async Task<StockEntry> AddAsync(StockEntry entry)
        {
            return await Task.Run(() => Add(entry));
        }
        public async Task<bool> UpdateAsync(StockEntry entry)
        {
            return await Task.Run(() => Update(entry));
        }
        public async Task<bool> DeleteAsync(int id)
        {
            return await Task.Run(() => Delete(id));
        }
    }
} 