using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Teklas_Intern_ERP.Entities.Interfaces;

namespace Teklas_Intern_ERP.DataAccess.Repositories
{
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        protected readonly AppDbContext _context;
        public BaseRepository(AppDbContext context)
        {
            _context = context;
        }

        public virtual async Task<TEntity> GetByIdAsync(int id) => await _context.Set<TEntity>().Where(e => EF.Property<bool>(e, "IsDeleted") == false).FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
        public virtual async Task<List<TEntity>> GetAllAsync() => await _context.Set<TEntity>().Where(e => EF.Property<bool>(e, "IsDeleted") == false).ToListAsync();
        public virtual async Task AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }
      public virtual async Task UpdateAsync(TEntity entity)
{
    var existing = await _context.Set<TEntity>().FindAsync(entity.Id);
    if (existing == null)
        throw new Exception("Entity not found");

    _context.Entry(existing).CurrentValues.SetValues(entity);
    await _context.SaveChangesAsync();
}
        public virtual async Task DeleteAsync(TEntity entity)
        {
            _context.Entry(entity).Property("IsDeleted").CurrentValue = true;
            await _context.SaveChangesAsync();
        }
        public virtual async Task<List<TEntity>> GetDeletedAsync() => await _context.Set<TEntity>().Where(e => EF.Property<bool>(e, "IsDeleted") == true).ToListAsync();
        public virtual async Task<TEntity> GetByIdIncludeDeletedAsync(int id) => await _context.Set<TEntity>().FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
        public virtual async Task RestoreAsync(TEntity entity)
        {
            _context.Entry(entity).Property("IsDeleted").CurrentValue = false;
            await _context.SaveChangesAsync();
        }
        public virtual async Task PermanentDeleteAsync(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
} 