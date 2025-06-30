using Teklas_Intern_ERP.DataAccess.MaterialManagement;
using Teklas_Intern_ERP.Entities.MaterialManagement;
using System.Collections.Generic;
using Teklas_Intern_ERP.DataAccess;

namespace Teklas_Intern_ERP.Business.MaterialManagement
{
    public class MaterialMovementManager
    {
        private readonly MaterialMovementRepository _repo;
        public MaterialMovementManager(AppDbContext context)
        {
            _repo = new MaterialMovementRepository(context);
        }

        public List<MaterialMovement> GetAll() => _repo.GetAll();
        public MaterialMovement GetById(int id) => _repo.GetById(id);
        public MaterialMovement Add(MaterialMovement movement) => _repo.Add(movement);
        public bool Update(MaterialMovement movement) => _repo.Update(movement);
        public bool Delete(int id) => _repo.Delete(id);
    }
} 