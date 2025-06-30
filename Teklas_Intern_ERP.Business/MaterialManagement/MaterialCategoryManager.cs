using Teklas_Intern_ERP.DataAccess.MaterialManagement;
using Teklas_Intern_ERP.Entities.MaterialManagement;
using System.Collections.Generic;
using Teklas_Intern_ERP.DataAccess;

namespace Teklas_Intern_ERP.Business.MaterialManagement
{
    public class MaterialCategoryManager
    {
        private readonly MaterialCategoryRepository _repo;
        public MaterialCategoryManager(AppDbContext context)
        {
            _repo = new MaterialCategoryRepository(context);
        }

        public List<MaterialCategory> GetAll() => _repo.GetAll();
        public MaterialCategory GetById(int id) => _repo.GetById(id);
        public MaterialCategory Add(MaterialCategory category) => _repo.Add(category);
        public bool Update(MaterialCategory category) => _repo.Update(category);
        public bool Delete(int id) => _repo.Delete(id);
    }
} 