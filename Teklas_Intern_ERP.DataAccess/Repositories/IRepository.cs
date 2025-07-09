using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Teklas_Intern_ERP.Entities.Interfaces;

namespace Teklas_Intern_ERP.DataAccess.Repositories
{
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        // Basic CRUD Operations
        Task<TEntity?> GetByIdAsync(long id);
        Task<TEntity?> GetByIdAsync(long id, params string[] includes);
        Task<List<TEntity>> GetAllAsync();
        Task<List<TEntity>> GetAllAsync(params string[] includes);
        Task<TEntity> AddAsync(TEntity entity);
        Task<List<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities);
        Task UpdateAsync(TEntity entity);
        Task UpdateRangeAsync(IEnumerable<TEntity> entities);
        Task<bool> DeleteAsync(long id);
        Task DeleteAsync(TEntity entity);
        Task<int> DeleteRangeAsync(IEnumerable<TEntity> entities);
        
        // Soft Delete Operations
        Task<List<TEntity>> GetDeletedAsync();
        Task<TEntity?> GetByIdIncludeDeletedAsync(long id);
        Task<bool> RestoreAsync(long id);
        Task RestoreAsync(TEntity entity);
        Task<bool> PermanentDeleteAsync(long id);
        Task PermanentDeleteAsync(TEntity entity);
        
        // Query Operations
        Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
        Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, params string[] includes);
        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, params string[] includes);
        
        // Pagination & Sorting
        Task<(List<TEntity> Items, int TotalCount)> GetPagedAsync(int page, int pageSize);
        Task<(List<TEntity> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, Expression<Func<TEntity, bool>>? filter = null);
        Task<(List<TEntity> Items, int TotalCount)> GetPagedAsync<TKey>(int page, int pageSize, Expression<Func<TEntity, TKey>> orderBy, bool ascending = true);
        Task<(List<TEntity> Items, int TotalCount)> GetPagedAsync<TKey>(int page, int pageSize, Expression<Func<TEntity, bool>>? filter, Expression<Func<TEntity, TKey>> orderBy, bool ascending = true);
        
        // Counting Operations
        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
        
        // Advanced Queries
        IQueryable<TEntity> AsQueryable();
        IQueryable<TEntity> AsQueryableIncludeDeleted();
        Task<List<TEntity>> SearchAsync(string searchTerm, params string[] searchFields);
        
        // Bulk Operations
        Task<int> BulkUpdateAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> updateExpression);
        Task<int> BulkDeleteAsync(Expression<Func<TEntity, bool>> predicate);
        
        // Unit of Work Integration (without SaveChanges in each operation)
        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        void UpdateRange(IEnumerable<TEntity> entities);
        void Delete(TEntity entity);
        void DeleteRange(IEnumerable<TEntity> entities);
    }
} 