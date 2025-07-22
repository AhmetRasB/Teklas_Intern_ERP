using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Teklas_Intern_ERP.Business.Interfaces;
using Teklas_Intern_ERP.DataAccess.SalesManagement;
using Teklas_Intern_ERP.DTOs.SalesManagement;
using Teklas_Intern_ERP.Entities.SalesManagement;

namespace Teklas_Intern_ERP.Business.SalesManagement
{
    public sealed class CustomerOrderService : ICustomerOrderService
    {
        private readonly ICustomerOrderRepository _repository;
        private readonly IMapper _mapper;

        public CustomerOrderService(ICustomerOrderRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CustomerOrderDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync("Customer");
            return _mapper.Map<IEnumerable<CustomerOrderDto>>(entities);
        }

        public async Task<CustomerOrderDto?> GetByIdAsync(long id)
        {
            var entity = await _repository.GetByIdAsync(id, "Customer");
            return _mapper.Map<CustomerOrderDto>(entity);
        }

        public async Task<CustomerOrderDto> CreateAsync(CustomerOrderDto dto)
        {
            var entity = _mapper.Map<CustomerOrder>(dto);
            var createdEntity = await _repository.AddAsync(entity);
            return _mapper.Map<CustomerOrderDto>(createdEntity);
        }

        public async Task<CustomerOrderDto> UpdateAsync(long id, CustomerOrderDto dto)
        {
            var existingEntity = await _repository.GetByIdAsync(id);
            if (existingEntity == null)
                throw new ArgumentException("Customer order not found");

            _mapper.Map(dto, existingEntity);
            await _repository.UpdateAsync(existingEntity);
            return _mapper.Map<CustomerOrderDto>(existingEntity);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<bool> RestoreAsync(long id)
        {
            return await _repository.RestoreAsync(id);
        }

        public async Task<IEnumerable<CustomerOrderDto>> SearchAsync(string searchTerm)
        {
            var entities = await _repository.SearchAsync(searchTerm, "OrderNumber", "Description");
            return _mapper.Map<IEnumerable<CustomerOrderDto>>(entities);
        }

        public async Task<IEnumerable<CustomerOrderDto>> GetOrdersByCustomerAsync(long customerId)
        {
            var entities = await _repository.GetOrdersByCustomerAsync(customerId);
            return _mapper.Map<IEnumerable<CustomerOrderDto>>(entities);
        }

        public async Task<IEnumerable<CustomerOrderDto>> GetOrdersByStatusAsync(string status)
        {
            var entities = await _repository.GetOrdersByStatusAsync(status);
            return _mapper.Map<IEnumerable<CustomerOrderDto>>(entities);
        }

        public async Task<IEnumerable<CustomerOrderDto>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var entities = await _repository.GetOrdersByDateRangeAsync(startDate, endDate);
            return _mapper.Map<IEnumerable<CustomerOrderDto>>(entities);
        }

        public async Task<IEnumerable<CustomerOrderDto>> GetDeletedAsync()
        {
            var entities = await _repository.GetDeletedAsync();
            return _mapper.Map<IEnumerable<CustomerOrderDto>>(entities);
        }

        public async Task<bool> PermanentDeleteAsync(long id)
        {
            return await _repository.PermanentDeleteAsync(id);
        }
    }
} 