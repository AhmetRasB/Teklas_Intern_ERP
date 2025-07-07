using Teklas_Intern_ERP.DataAccess.Repositories;
using Teklas_Intern_ERP.Entities.MaterialManagement;
using System.Collections.Generic;
using System.Threading.Tasks;
using Teklas_Intern_ERP.Business.Interfaces;

namespace Teklas_Intern_ERP.Business.MaterialManagement
{
    public class MaterialCardService : IMaterialCardService
    {
        private readonly IRepository<MaterialCard> _repo;
        public MaterialCardService(IRepository<MaterialCard> repo)
        {
            _repo = repo;
        }
        public async Task<List<MaterialCard>> GetAllAsync() => await _repo.GetAllAsync();
        public async Task<MaterialCard> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);
        public async Task<MaterialCard> AddAsync(MaterialCard card)
        {
            card.UpdatedDate = DateTime.Now;
            card.CreatedDate = DateTime.Now;
            await _repo.AddAsync(card);
            return card;
        }
        public async Task<bool> UpdateAsync(MaterialCard card)
        {
            var existing = await _repo.GetByIdAsync(card.Id);
            if (existing == null) return false;
            card.UpdatedDate = DateTime.Now;
            await _repo.UpdateAsync(card);
            return true;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var card = await _repo.GetByIdAsync(id);
            if (card == null) return false;
            await _repo.DeleteAsync(card);
            return true;
        }
        public async Task<List<MaterialCard>> GetDeletedAsync() => await _repo.GetDeletedAsync();
        public async Task<bool> RestoreAsync(int id)
        {
            var card = await _repo.GetByIdIncludeDeletedAsync(id);
            if (card == null) return false;
            await _repo.RestoreAsync(card);
            return true;
        }
        public async Task<bool> PermanentDeleteAsync(int id)
        {
            var card = await _repo.GetByIdIncludeDeletedAsync(id);
            if (card == null) return false;
            await _repo.PermanentDeleteAsync(card);
            return true;
        }
    }
} 