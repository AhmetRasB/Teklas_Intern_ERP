using System.Collections.Generic;
using System.Threading.Tasks;
using Teklas_Intern_ERP.Entities.Interfaces;

namespace Teklas_Intern_ERP.DataAccess.Repositories
{
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        Task<TEntity> GetByIdAsync(int id);
        Task<List<TEntity>> GetAllAsync();
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task<List<TEntity>> GetDeletedAsync();
        Task<TEntity> GetByIdIncludeDeletedAsync(int id);
        Task RestoreAsync(TEntity entity);
        Task PermanentDeleteAsync(TEntity entity);
    }
} 