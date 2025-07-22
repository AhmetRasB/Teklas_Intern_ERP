using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Teklas_Intern_ERP.Business.Interfaces;
using Teklas_Intern_ERP.DataAccess.PurchasingManagement;
using Teklas_Intern_ERP.DTOs.PurchasingManagement;
using Teklas_Intern_ERP.Entities.PurchasingManagement;

namespace Teklas_Intern_ERP.Business.PurchasingManagement
{
    public sealed class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly IPurchaseOrderRepository _repository;
        private readonly IMapper _mapper;

        public PurchaseOrderService(IPurchaseOrderRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PurchaseOrderDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync("Supplier");
            return _mapper.Map<IEnumerable<PurchaseOrderDto>>(entities);
        }

        public async Task<PurchaseOrderDto?> GetByIdAsync(long id)
        {
            var entity = await _repository.GetByIdAsync(id, "Supplier");
            return _mapper.Map<PurchaseOrderDto>(entity);
        }

        public async Task<PurchaseOrderDto> CreateAsync(PurchaseOrderDto dto)
        {
            var entity = _mapper.Map<PurchaseOrder>(dto);
            var createdEntity = await _repository.AddAsync(entity);
            return _mapper.Map<PurchaseOrderDto>(createdEntity);
        }

        public async Task<PurchaseOrderDto> UpdateAsync(long id, PurchaseOrderDto dto)
        {
            var existingEntity = await _repository.GetByIdAsync(id);
            if (existingEntity == null)
                throw new ArgumentException("Purchase order not found");

            _mapper.Map(dto, existingEntity);
            await _repository.UpdateAsync(existingEntity);
            return _mapper.Map<PurchaseOrderDto>(existingEntity);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<bool> RestoreAsync(long id)
        {
            return await _repository.RestoreAsync(id);
        }

        public async Task<IEnumerable<PurchaseOrderDto>> SearchAsync(string searchTerm)
        {
            var entities = await _repository.SearchAsync(searchTerm, "OrderNumber", "Description");
            return _mapper.Map<IEnumerable<PurchaseOrderDto>>(entities);
        }

        public async Task<IEnumerable<PurchaseOrderDto>> GetOrdersBySupplierAsync(long supplierId)
        {
            var entities = await _repository.GetOrdersBySupplierAsync(supplierId);
            return _mapper.Map<IEnumerable<PurchaseOrderDto>>(entities);
        }

        public async Task<IEnumerable<PurchaseOrderDto>> GetOrdersByStatusAsync(string status)
        {
            var entities = await _repository.GetOrdersByStatusAsync(status);
            return _mapper.Map<IEnumerable<PurchaseOrderDto>>(entities);
        }

        public async Task<IEnumerable<PurchaseOrderDto>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var entities = await _repository.GetOrdersByDateRangeAsync(startDate, endDate);
            return _mapper.Map<IEnumerable<PurchaseOrderDto>>(entities);
        }

        public async Task<IEnumerable<PurchaseOrderDto>> GetDeletedAsync()
        {
            var entities = await _repository.GetDeletedAsync();
            return _mapper.Map<IEnumerable<PurchaseOrderDto>>(entities);
        }

        public async Task<bool> PermanentDeleteAsync(long id)
        {
            return await _repository.PermanentDeleteAsync(id);
        }
    }
} 