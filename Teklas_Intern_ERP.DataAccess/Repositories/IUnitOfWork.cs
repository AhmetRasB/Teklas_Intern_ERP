using System;
using System.Threading.Tasks;

namespace Teklas_Intern_ERP.DataAccess.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> Repository<T>() where T : class;
        Task<int> SaveChangesAsync();
    }
} 