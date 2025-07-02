using System.Collections.Generic;
using System.Linq;
using Teklas_Intern_ERP.Entities.MaterialManagement;
using Teklas_Intern_ERP.DataAccess;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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

        public async Task<List<MaterialMovement>> GetAllAsync() => await _context.MaterialMovements.ToListAsync();
        public async Task<MaterialMovement> GetByIdAsync(int id) => await _context.MaterialMovements.FindAsync(id);
        public async Task<MaterialMovement> AddAsync(MaterialMovement movement)
        {
            _context.MaterialMovements.Add(movement);
            await _context.SaveChangesAsync();
            return movement;
        }
        public async Task<bool> UpdateAsync(MaterialMovement movement)
        {
            var existing = await _context.MaterialMovements.FindAsync(movement.Id);
            if (existing == null) return false;
            existing.MaterialId = movement.MaterialId;
            existing.Quantity = movement.Quantity;
            existing.MovementDate = movement.MovementDate;
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var movement = await _context.MaterialMovements.FindAsync(id);
            if (movement == null) return false;
            _context.MaterialMovements.Remove(movement);
            await _context.SaveChangesAsync();
            return true;
        }
    }
} 