using Teklas_Intern_ERP.DataAccess.MaterialManagement;
using Teklas_Intern_ERP.Entities.MaterialManagement;
using System.Collections.Generic;
using Teklas_Intern_ERP.DataAccess;

namespace Teklas_Intern_ERP.Business.MaterialManagement
{
    public class MaterialCardManager
    {
        private readonly MaterialCardRepository _repo;
        public MaterialCardManager(AppDbContext context)
        {
            _repo = new MaterialCardRepository(context);
        }

        public List<MaterialCard> GetAll() => _repo.GetAll();
        public MaterialCard GetById(int id) => _repo.GetById(id);
        public MaterialCard Add(MaterialCard card) => _repo.Add(card);
        public bool Update(MaterialCard card) => _repo.Update(card);
        public bool Delete(int id) => _repo.Delete(id);
    }
} 