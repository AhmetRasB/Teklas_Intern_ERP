using Teklas_Intern_ERP.DataAccess.ProductionManagement;
using Teklas_Intern_ERP.Entities.ProductionManagement;
using System.Collections.Generic;
using Teklas_Intern_ERP.DataAccess;

namespace Teklas_Intern_ERP.Business.ProductionManagement
{
    public class BillOfMaterialsManager
    {
        private readonly BillOfMaterialsRepository _repo;
        public BillOfMaterialsManager(AppDbContext context)
        {
            _repo = new BillOfMaterialsRepository(context);
        }

        public List<BillOfMaterials> GetAll() => _repo.GetAll();
        public BillOfMaterials GetById(int id) => _repo.GetById(id);
        public BillOfMaterials Add(BillOfMaterials bom) => _repo.Add(bom);
        public bool Update(BillOfMaterials bom) => _repo.Update(bom);
        public bool Delete(int id) => _repo.Delete(id);
    }
} 