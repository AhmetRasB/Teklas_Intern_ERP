using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using Teklas_Intern_ERP.DataAccess;
using Teklas_Intern_ERP.Entities.Interfaces;
using Teklas_Intern_ERP.DataAccess.Repositories;
using Teklas_Intern_ERP.DataAccess.ProductionManagement;

namespace Teklas_Intern_ERP.DataAccess.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly Dictionary<Type, object> _repositories = new();
        private IDbContextTransaction? _transaction;

        // Production Management Repositories
        public IBillOfMaterialRepository BillOfMaterialRepository { get; }
        public IWorkOrderRepository WorkOrderRepository { get; }
        public IProductionConfirmationRepository ProductionConfirmationRepository { get; }
        public IWorkOrderOperationRepository WorkOrderOperationRepository { get; }
        public IMaterialConsumptionRepository MaterialConsumptionRepository { get; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            BillOfMaterialRepository = new BillOfMaterialRepository(_context);
            WorkOrderRepository = new WorkOrderRepository(_context);
            ProductionConfirmationRepository = new ProductionConfirmationRepository(_context);
            WorkOrderOperationRepository = new WorkOrderOperationRepository(_context);
            MaterialConsumptionRepository = new MaterialConsumptionRepository(_context);
        }

        public IRepository<T> Repository<T>() where T : class, IEntity
        {
            if (!_repositories.ContainsKey(typeof(T)))
            {
                var repoType = typeof(BaseRepository<>).MakeGenericType(typeof(T));
                var repoInstance = Activator.CreateInstance(repoType, _context);
                _repositories[typeof(T)] = repoInstance ?? throw new InvalidOperationException($"Could not create repository for {typeof(T).Name}");
            }
            return (IRepository<T>)_repositories[typeof(T)];
        }

        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

        public async Task BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
} 