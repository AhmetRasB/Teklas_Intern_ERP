using System.Collections.Generic;
using System.Linq;
using Teklas_Intern_ERP.Entities.MaterialManagement;
using Teklas_Intern_ERP.DataAccess;

namespace Teklas_Intern_ERP.DataAccess.MaterialManagement
{
    public class MaterialMovementRepository
    {
        private readonly AppDbContext _context;
        public MaterialMovementRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<MaterialMovement> GetAll() => _context.MaterialMovements.ToList();
        public MaterialMovement GetById(int id) => _context.MaterialMovements.Find(id);
        public MaterialMovement Add(MaterialMovement movement)
        {
            _context.MaterialMovements.Add(movement);
            _context.SaveChanges();
            return movement;
        }
        public bool Update(MaterialMovement movement)
        {
            var existing = _context.MaterialMovements.Find(movement.Id);
            if (existing == null) return false;
            existing.MaterialId = movement.MaterialId;
            existing.Quantity = movement.Quantity;
            existing.MovementDate = movement.MovementDate;
            _context.SaveChanges();
            return true;
        }
        public bool Delete(int id)
        {
            var movement = _context.MaterialMovements.Find(id);
            if (movement == null) return false;
            _context.MaterialMovements.Remove(movement);
            _context.SaveChanges();
            return true;
        }
    }
} 