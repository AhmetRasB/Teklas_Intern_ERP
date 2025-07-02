using Teklas_Intern_ERP.DataAccess.MaterialManagement;
using Teklas_Intern_ERP.Entities.MaterialManagement;
using System.Collections.Generic;
using Teklas_Intern_ERP.DataAccess;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Teklas_Intern_ERP.Business.MaterialManagement
{
    public class MaterialCardManager
    {
        private readonly MaterialCardRepository _repo;
        private readonly IDistributedCache _cache;
        public MaterialCardManager(MaterialCardRepository repo, IDistributedCache cache)
        {
            _repo = repo;
            _cache = cache;
        }

        public List<MaterialCard> GetAll() => _repo.GetAll();
        public MaterialCard GetById(int id) => _repo.GetById(id);
        public MaterialCard Add(MaterialCard card) => _repo.Add(card);
        public bool Update(MaterialCard card) => _repo.Update(card);
        public bool Delete(int id) => _repo.Delete(id);

        public async Task<List<MaterialCard>> GetAllAsync()
        {
            var cacheKey = "MaterialCards_All";
            var cached = await _cache.GetStringAsync(cacheKey);
            if (cached != null)
                return JsonSerializer.Deserialize<List<MaterialCard>>(cached);
            var data = await _repo.GetAllAsync();
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = System.TimeSpan.FromMinutes(10)
            };
            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(data), options);
            return data;
        }
        public async Task<MaterialCard> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);
        public async Task<MaterialCard> AddAsync(MaterialCard card)
        {
            var created = await _repo.AddAsync(card);
            await _cache.RemoveAsync("MaterialCards_All");
            return created;
        }
        public async Task<bool> UpdateAsync(MaterialCard card)
        {
            var result = await _repo.UpdateAsync(card);
            if (result)
                await _cache.RemoveAsync("MaterialCards_All");
            return result;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var result = await _repo.DeleteAsync(id);
            if (result)
                await _cache.RemoveAsync("MaterialCards_All");
            return result;
        }
        public async Task<(List<MaterialCard> Items, int TotalCount)> GetPagedAsync(int page, int pageSize)
            => await _repo.GetPagedAsync(page, pageSize);
    }
} 