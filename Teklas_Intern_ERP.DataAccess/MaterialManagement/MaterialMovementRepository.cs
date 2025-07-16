using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Teklas_Intern_ERP.Entities.MaterialManagement;
using Teklas_Intern_ERP.DataAccess.Repositories;

namespace Teklas_Intern_ERP.DataAccess.MaterialManagement
{
    public interface IMaterialMovementRepository : IRepository<MaterialMovement>
    {
        Task<List<MaterialMovement>> GetMovementsByMaterialCardAsync(long materialCardId);
        Task<List<MaterialMovement>> GetMovementsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<List<MaterialMovement>> GetMovementsByTypeAsync(string movementType);
        Task<List<MaterialMovement>> GetMovementsByStatusAsync(string status);
        Task<List<MaterialMovement>> GetPendingMovementsAsync();
        Task<List<MaterialMovement>> GetMovementsByLocationAsync(string location);
        Task<List<MaterialMovement>> GetMovementsByReferenceAsync(string referenceNumber, string referenceType);
        Task<decimal> GetCurrentStockBalanceAsync(long materialCardId);
        Task<decimal> GetTotalMovementAmountAsync(long materialCardId, string movementType);
        Task<List<MaterialMovement>> GetRecentMovementsAsync(int take = 50);
        Task<List<MaterialMovement>> SearchMovementsAsync(string searchTerm);
        Task<bool> UpdateStockBalanceAsync(long materialCardId, decimal newBalance);
        Task<List<MaterialMovement>> GetMovementsWithMaterialCardAsync();
        
        // Soft Delete Methods
        new Task<List<MaterialMovement>> GetDeletedAsync();
        new Task<bool> RestoreAsync(long id);
        new Task<bool> PermanentDeleteAsync(long id);
    }

    public class MaterialMovementRepository : BaseRepository<MaterialMovement>, IMaterialMovementRepository
    {
        public MaterialMovementRepository(AppDbContext context) : base(context) { }

        #region Material Card Related Methods

        public async Task<List<MaterialMovement>> GetMovementsByMaterialCardAsync(long materialCardId)
        {
            return await _dbSet
                .Where(m => !m.IsDeleted && m.MaterialCardId == materialCardId)
                .Include(m => m.MaterialCard)
                .OrderByDescending(m => m.MovementDate)
                .ToListAsync();
        }

        public async Task<decimal> GetCurrentStockBalanceAsync(long materialCardId)
        {
            var lastMovement = await _dbSet
                .Where(m => !m.IsDeleted && m.MaterialCardId == materialCardId && m.StockBalance.HasValue)
                .OrderByDescending(m => m.MovementDate)
                .FirstOrDefaultAsync();

            return lastMovement?.StockBalance ?? 0;
        }

        public async Task<decimal> GetTotalMovementAmountAsync(long materialCardId, string movementType)
        {
            return await _dbSet
                .Where(m => !m.IsDeleted && m.MaterialCardId == materialCardId && m.MovementType == movementType)
                .SumAsync(m => m.Quantity);
        }

        #endregion

        #region Date and Filter Methods

        public async Task<List<MaterialMovement>> GetMovementsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Where(m => !m.IsDeleted && m.MovementDate >= startDate && m.MovementDate <= endDate)
                .Include(m => m.MaterialCard)
                .OrderByDescending(m => m.MovementDate)
                .ToListAsync();
        }

        public async Task<List<MaterialMovement>> GetMovementsByTypeAsync(string movementType)
        {
            return await _dbSet
                .Where(m => !m.IsDeleted && m.MovementType == movementType)
                .Include(m => m.MaterialCard)
                .OrderByDescending(m => m.MovementDate)
                .ToListAsync();
        }

        public async Task<List<MaterialMovement>> GetMovementsByStatusAsync(string status)
        {
            return await _dbSet
                .Where(m => !m.IsDeleted && m.Status == status)
                .Include(m => m.MaterialCard)
                .OrderByDescending(m => m.MovementDate)
                .ToListAsync();
        }

        public async Task<List<MaterialMovement>> GetPendingMovementsAsync()
        {
            return await _dbSet
                .Where(m => !m.IsDeleted && (m.Status == "PENDING" || string.IsNullOrEmpty(m.Status)))
                .Include(m => m.MaterialCard)
                .OrderBy(m => m.MovementDate)
                .ToListAsync();
        }

        #endregion

        #region Location and Reference Methods

        public async Task<List<MaterialMovement>> GetMovementsByLocationAsync(string location)
        {
            return await _dbSet
                .Where(m => !m.IsDeleted && (m.LocationFrom == location || m.LocationTo == location))
                .Include(m => m.MaterialCard)
                .OrderByDescending(m => m.MovementDate)
                .ToListAsync();
        }

        public async Task<List<MaterialMovement>> GetMovementsByReferenceAsync(string referenceNumber, string referenceType)
        {
            return await _dbSet
                .Where(m => !m.IsDeleted && m.ReferenceNumber == referenceNumber && m.ReferenceType == referenceType)
                .Include(m => m.MaterialCard)
                .OrderByDescending(m => m.MovementDate)
                .ToListAsync();
        }

        #endregion

        #region Search and General Methods

        public async Task<List<MaterialMovement>> GetRecentMovementsAsync(int take = 50)
        {
            return await _dbSet
                .Where(m => !m.IsDeleted)
                .Include(m => m.MaterialCard)
                .OrderByDescending(m => m.MovementDate)
                .Take(take)
                .ToListAsync();
        }

        public async Task<List<MaterialMovement>> SearchMovementsAsync(string searchTerm)
        {
            return await _dbSet
                .Where(m => !m.IsDeleted && (
                    m.MaterialCard.CardCode.Contains(searchTerm) ||
                    m.MaterialCard.CardName.Contains(searchTerm) ||
                    (m.ReferenceNumber != null && m.ReferenceNumber.Contains(searchTerm)) ||
                    (m.Description != null && m.Description.Contains(searchTerm)) ||
                    (m.ResponsiblePerson != null && m.ResponsiblePerson.Contains(searchTerm))
                ))
                .Include(m => m.MaterialCard)
                .OrderByDescending(m => m.MovementDate)
                .ToListAsync();
        }

        public async Task<List<MaterialMovement>> GetMovementsWithMaterialCardAsync()
        {
            return await _dbSet
                .Where(m => !m.IsDeleted)
                .Include(m => m.MaterialCard)
                .OrderByDescending(m => m.MovementDate)
                .ToListAsync();
        }

        #endregion

        #region Stock Balance Management

        public async Task<bool> UpdateStockBalanceAsync(long materialCardId, decimal newBalance)
        {
            var movements = await _dbSet
                .Where(m => m.MaterialCardId == materialCardId && !m.IsDeleted)
                .ToListAsync();

            foreach (var movement in movements)
            {
                movement.StockBalance = newBalance;
                movement.UpdateDate = DateTime.UtcNow;
            }

            return await _context.SaveChangesAsync() > 0;
        }

        #endregion

        // Soft Delete Methods
        public new async Task<List<MaterialMovement>> GetDeletedAsync()
        {
            return await _dbSet
                .IgnoreQueryFilters()
                .Where(m => m.IsDeleted)
                .Include(m => m.MaterialCard)
                .OrderByDescending(m => m.MovementDate)
                .ToListAsync();
        }

        public new async Task<bool> RestoreAsync(long id)
        {
            var movement = await _dbSet.IgnoreQueryFilters().FirstOrDefaultAsync(m => m.Id == id);
            if (movement == null) return false;

            movement.IsDeleted = false;
            movement.DeleteDate = null;
            movement.UpdateDate = DateTime.UtcNow;

            return await _context.SaveChangesAsync() > 0;
        }

        public new async Task<bool> PermanentDeleteAsync(long id)
        {
            var movement = await _dbSet.IgnoreQueryFilters().FirstOrDefaultAsync(m => m.Id == id);
            if (movement == null) return false;

            _dbSet.Remove(movement);
            return await _context.SaveChangesAsync() > 0;
        }
    }
} 