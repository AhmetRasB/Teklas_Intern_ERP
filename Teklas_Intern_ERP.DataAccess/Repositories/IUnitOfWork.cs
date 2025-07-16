using System;
using System.Data;
using System.Threading.Tasks;
using Teklas_Intern_ERP.Entities.Interfaces;
using Teklas_Intern_ERP.DataAccess.Repositories;

namespace Teklas_Intern_ERP.DataAccess.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> Repository<T>() where T : class, IEntity;
        Task<int> SaveChangesAsync();
        
        // Transaction Management
        Task BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();

        // Production Management Repositories
        IBillOfMaterialRepository BillOfMaterialRepository { get; }
        IWorkOrderRepository WorkOrderRepository { get; }
        IProductionConfirmationRepository ProductionConfirmationRepository { get; }
        IWorkOrderOperationRepository WorkOrderOperationRepository { get; }
        IMaterialConsumptionRepository MaterialConsumptionRepository { get; }
    }
} 