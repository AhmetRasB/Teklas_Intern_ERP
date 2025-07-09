using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Teklas_Intern_ERP.Entities.Interfaces;
using Teklas_Intern_ERP.Entities;

namespace Teklas_Intern_ERP.DataAccess.Repositories
{
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public BaseRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        #region Basic CRUD Operations

        public virtual async Task<TEntity?> GetByIdAsync(long id)
        {
            return await _dbSet.Where(e => EF.Property<bool>(e, "IsDeleted") == false)
                              .FirstOrDefaultAsync(e => EF.Property<long>(e, "Id") == id);
        }

        public virtual async Task<TEntity?> GetByIdAsync(long id, params string[] includes)
        {
            var query = _dbSet.Where(e => EF.Property<bool>(e, "IsDeleted") == false);
            
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            
            return await query.FirstOrDefaultAsync(e => EF.Property<long>(e, "Id") == id);
        }

        public virtual async Task<List<TEntity>> GetAllAsync()
        {
            return await _dbSet.Where(e => EF.Property<bool>(e, "IsDeleted") == false).ToListAsync();
        }

        public virtual async Task<List<TEntity>> GetAllAsync(params string[] includes)
        {
            var query = _dbSet.Where(e => EF.Property<bool>(e, "IsDeleted") == false);
            
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            
            return await query.ToListAsync();
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            SetAuditFields(entity, isNew: true);
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<List<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities)
        {
            var entityList = entities.ToList();
            foreach (var entity in entityList)
            {
                SetAuditFields(entity, isNew: true);
            }
            
            await _dbSet.AddRangeAsync(entityList);
            await _context.SaveChangesAsync();
            return entityList;
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            var existing = await _dbSet.FindAsync(entity.Id);
            if (existing == null)
                throw new InvalidOperationException($"Entity with ID {entity.Id} not found");

            SetAuditFields(entity, isNew: false);
            _context.Entry(existing).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task UpdateRangeAsync(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                SetAuditFields(entity, isNew: false);
                _dbSet.Update(entity);
            }
            
            await _context.SaveChangesAsync();
        }

        public virtual async Task<bool> DeleteAsync(long id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null) return false;
            
            await DeleteAsync(entity);
            return true;
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            SetDeleteAuditFields(entity);
            _context.Entry(entity).Property("IsDeleted").CurrentValue = true;
            _context.Entry(entity).Property("DeleteDate").CurrentValue = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        public virtual async Task<int> DeleteRangeAsync(IEnumerable<TEntity> entities)
        {
            var count = 0;
            foreach (var entity in entities)
            {
                SetDeleteAuditFields(entity);
                _context.Entry(entity).Property("IsDeleted").CurrentValue = true;
                _context.Entry(entity).Property("DeleteDate").CurrentValue = DateTime.UtcNow;
                count++;
            }
            
            await _context.SaveChangesAsync();
            return count;
        }

        #endregion

        #region Soft Delete Operations

        public virtual async Task<List<TEntity>> GetDeletedAsync()
        {
            return await _dbSet
                .IgnoreQueryFilters()
                .Where(e => EF.Property<bool>(e, "IsDeleted") == true)
                .ToListAsync();
        }

        public virtual async Task<TEntity?> GetByIdIncludeDeletedAsync(long id)
        {
            return await _dbSet.FirstOrDefaultAsync(e => EF.Property<long>(e, "Id") == id);
        }

        public virtual async Task<bool> RestoreAsync(long id)
        {
            var entity = await _dbSet.IgnoreQueryFilters().FirstOrDefaultAsync(e => EF.Property<long>(e, "Id") == id);
            if (entity == null) return false;
            
            await RestoreAsync(entity);
            return true;
        }

        public virtual async Task RestoreAsync(TEntity entity)
        {
            _context.Entry(entity).Property("IsDeleted").CurrentValue = false;
            _context.Entry(entity).Property("DeleteDate").CurrentValue = null;
            _context.Entry(entity).Property("DeleteUserId").CurrentValue = null;
            SetAuditFields(entity, isNew: false);
            await _context.SaveChangesAsync();
        }

        public virtual async Task<bool> PermanentDeleteAsync(long id)
        {
            var entity = await _dbSet.IgnoreQueryFilters().FirstOrDefaultAsync(e => EF.Property<long>(e, "Id") == id);
            if (entity == null) return false;
            
            await PermanentDeleteAsync(entity);
            return true;
        }

        public virtual async Task PermanentDeleteAsync(TEntity entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        #endregion

        #region Query Operations

        public virtual async Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.Where(e => EF.Property<bool>(e, "IsDeleted") == false)
                              .Where(predicate)
                              .ToListAsync();
        }

        public virtual async Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, params string[] includes)
        {
            var query = _dbSet.Where(e => EF.Property<bool>(e, "IsDeleted") == false)
                             .Where(predicate);
            
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            
            return await query.ToListAsync();
        }

        public virtual async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.Where(e => EF.Property<bool>(e, "IsDeleted") == false)
                              .FirstOrDefaultAsync(predicate);
        }

        public virtual async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, params string[] includes)
        {
            var query = _dbSet.Where(e => EF.Property<bool>(e, "IsDeleted") == false)
                             .Where(predicate);
            
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            
            return await query.FirstOrDefaultAsync();
        }

        #endregion

        #region Pagination & Sorting

        public virtual async Task<(List<TEntity> Items, int TotalCount)> GetPagedAsync(int page, int pageSize)
        {
            var query = _dbSet.Where(e => EF.Property<bool>(e, "IsDeleted") == false);
            var totalCount = await query.CountAsync();
            
            var items = await query.Skip((page - 1) * pageSize)
                                  .Take(pageSize)
                                  .ToListAsync();
            
            return (items, totalCount);
        }

        public virtual async Task<(List<TEntity> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, Expression<Func<TEntity, bool>>? filter = null)
        {
            var query = _dbSet.Where(e => EF.Property<bool>(e, "IsDeleted") == false);
            
            if (filter != null)
                query = query.Where(filter);
            
            var totalCount = await query.CountAsync();
            
            var items = await query.Skip((page - 1) * pageSize)
                                  .Take(pageSize)
                                  .ToListAsync();
            
            return (items, totalCount);
        }

        public virtual async Task<(List<TEntity> Items, int TotalCount)> GetPagedAsync<TKey>(int page, int pageSize, Expression<Func<TEntity, TKey>> orderBy, bool ascending = true)
        {
            var query = _dbSet.Where(e => EF.Property<bool>(e, "IsDeleted") == false);
            
            query = ascending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);
            
            var totalCount = await query.CountAsync();
            
            var items = await query.Skip((page - 1) * pageSize)
                                  .Take(pageSize)
                                  .ToListAsync();
            
            return (items, totalCount);
        }

        public virtual async Task<(List<TEntity> Items, int TotalCount)> GetPagedAsync<TKey>(int page, int pageSize, Expression<Func<TEntity, bool>>? filter, Expression<Func<TEntity, TKey>> orderBy, bool ascending = true)
        {
            var query = _dbSet.Where(e => EF.Property<bool>(e, "IsDeleted") == false);
            
            if (filter != null)
                query = query.Where(filter);
            
            query = ascending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);
            
            var totalCount = await query.CountAsync();
            
            var items = await query.Skip((page - 1) * pageSize)
                                  .Take(pageSize)
                                  .ToListAsync();
            
            return (items, totalCount);
        }

        #endregion

        #region Counting Operations

        public virtual async Task<int> CountAsync()
        {
            return await _dbSet.Where(e => EF.Property<bool>(e, "IsDeleted") == false).CountAsync();
        }

        public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.Where(e => EF.Property<bool>(e, "IsDeleted") == false)
                              .CountAsync(predicate);
        }

        public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.Where(e => EF.Property<bool>(e, "IsDeleted") == false)
                              .AnyAsync(predicate);
        }

        #endregion

        #region Advanced Queries

        public virtual IQueryable<TEntity> AsQueryable()
        {
            return _dbSet.Where(e => EF.Property<bool>(e, "IsDeleted") == false);
        }

        public virtual IQueryable<TEntity> AsQueryableIncludeDeleted()
        {
            return _dbSet.AsQueryable();
        }

        public virtual async Task<List<TEntity>> SearchAsync(string searchTerm, params string[] searchFields)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || !searchFields.Any())
                return new List<TEntity>();
            
            var query = _dbSet.Where(e => EF.Property<bool>(e, "IsDeleted") == false);
            
            // Build dynamic search expression
            var parameter = Expression.Parameter(typeof(TEntity), "x");
            Expression? searchExpression = null;
            
            foreach (var field in searchFields)
            {
                var property = Expression.Property(parameter, field);
                var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                var searchValue = Expression.Constant(searchTerm);
                var containsCall = Expression.Call(property, containsMethod!, searchValue);
                
                searchExpression = searchExpression == null 
                    ? containsCall 
                    : Expression.OrElse(searchExpression, containsCall);
            }
            
            if (searchExpression != null)
            {
                var lambda = Expression.Lambda<Func<TEntity, bool>>(searchExpression, parameter);
                query = query.Where(lambda);
            }
            
            return await query.ToListAsync();
        }

        public virtual async Task<int> BulkUpdateAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> updateExpression)
        {
            var entities = await _dbSet.Where(e => EF.Property<bool>(e, "IsDeleted") == false)
                                      .Where(predicate)
                                      .ToListAsync();
            
            foreach (var entity in entities)
            {
                var updated = updateExpression.Compile()(entity);
                SetAuditFields(updated, isNew: false);
                _context.Entry(entity).CurrentValues.SetValues(updated);
            }
            
            return await _context.SaveChangesAsync();
        }

        public virtual async Task<int> BulkDeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var entities = await _dbSet.Where(e => EF.Property<bool>(e, "IsDeleted") == false)
                                      .Where(predicate)
                                      .ToListAsync();
            
            foreach (var entity in entities)
            {
                SetDeleteAuditFields(entity);
                _context.Entry(entity).Property("IsDeleted").CurrentValue = true;
                _context.Entry(entity).Property("DeleteDate").CurrentValue = DateTime.UtcNow;
            }
            
            return await _context.SaveChangesAsync();
        }

        #endregion

        #region Unit of Work Integration

        public virtual void Add(TEntity entity)
        {
            SetAuditFields(entity, isNew: true);
            _dbSet.Add(entity);
        }

        public virtual void AddRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                SetAuditFields(entity, isNew: true);
            }
            _dbSet.AddRange(entities);
        }

        public virtual void Update(TEntity entity)
        {
            SetAuditFields(entity, isNew: false);
            _dbSet.Update(entity);
        }

        public virtual void UpdateRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                SetAuditFields(entity, isNew: false);
            }
            _dbSet.UpdateRange(entities);
        }

        public virtual void Delete(TEntity entity)
        {
            SetDeleteAuditFields(entity);
            _context.Entry(entity).Property("IsDeleted").CurrentValue = true;
            _context.Entry(entity).Property("DeleteDate").CurrentValue = DateTime.UtcNow;
        }

        public virtual void DeleteRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                SetDeleteAuditFields(entity);
                _context.Entry(entity).Property("IsDeleted").CurrentValue = true;
                _context.Entry(entity).Property("DeleteDate").CurrentValue = DateTime.UtcNow;
            }
        }

        #endregion

        #region Private Helper Methods

        private void SetAuditFields(TEntity entity, bool isNew)
        {
            var now = DateTime.UtcNow;
            var userId = GetCurrentUserId();
            
            if (isNew)
            {
                _context.Entry(entity).Property("CreateDate").CurrentValue = now;
                _context.Entry(entity).Property("CreateUserId").CurrentValue = userId;
            }
            
            _context.Entry(entity).Property("UpdateDate").CurrentValue = now;
            _context.Entry(entity).Property("UpdateUserId").CurrentValue = userId;
        }

        private void SetDeleteAuditFields(TEntity entity)
        {
            var now = DateTime.UtcNow;
            var userId = GetCurrentUserId();
            
            _context.Entry(entity).Property("DeleteDate").CurrentValue = now;
            _context.Entry(entity).Property("DeleteUserId").CurrentValue = userId;
        }

        private long GetCurrentUserId()
        {
            // TODO: Implement user context service to get current user ID
            return 1; // Default system user for now
        }

        #endregion
    }
} 