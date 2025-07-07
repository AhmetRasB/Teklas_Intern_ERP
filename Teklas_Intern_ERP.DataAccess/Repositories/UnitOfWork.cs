using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Teklas_Intern_ERP.DataAccess;

namespace Teklas_Intern_ERP.DataAccess.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly Dictionary<Type, object> _repositories = new();
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }
        public IRepository<T> Repository<T>() where T : class
        {
            if (!_repositories.ContainsKey(typeof(T)))
            {
                var repoType = typeof(BaseRepository<>).MakeGenericType(typeof(T));
                var repoInstance = Activator.CreateInstance(repoType, _context);
                _repositories[typeof(T)] = repoInstance;
            }
            return (IRepository<T>)_repositories[typeof(T)];
        }
        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
        public void Dispose() => _context.Dispose();
    }
} 