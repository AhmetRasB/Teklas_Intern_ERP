using System;
using System.Threading.Tasks;
using Teklas_Intern_ERP.Entities.Interfaces;

namespace Teklas_Intern_ERP.DataAccess.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> Repository<T>() where T : class, IEntity;
        Task<int> SaveChangesAsync();
    }
} 