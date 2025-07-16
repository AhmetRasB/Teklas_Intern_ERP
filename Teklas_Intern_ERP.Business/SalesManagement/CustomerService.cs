using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Teklas_Intern_ERP.Business.Interfaces;
using Teklas_Intern_ERP.DataAccess;
using Teklas_Intern_ERP.DataAccess.SalesManagement;
using Teklas_Intern_ERP.DTOs.SalesManagement;
using Teklas_Intern_ERP.Entities.SalesManagement;

namespace Teklas_Intern_ERP.Business.SalesManagement
{
    public sealed class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repository;
        private readonly IMapper _mapper;

        public CustomerService(ICustomerRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CustomerDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<CustomerDto>>(entities);
        }

        public async Task<CustomerDto?> GetByIdAsync(long id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return _mapper.Map<CustomerDto>(entity);
        }

        public async Task<CustomerDto> CreateAsync(CustomerDto dto)
        {
            var entity = _mapper.Map<Customer>(dto);
            var createdEntity = await _repository.AddAsync(entity);
            return _mapper.Map<CustomerDto>(createdEntity);
        }

        public async Task<CustomerDto> UpdateAsync(long id, CustomerDto dto)
        {
            var existingEntity = await _repository.GetByIdAsync(id);
            if (existingEntity == null)
                throw new ArgumentException("Customer not found");

            _mapper.Map(dto, existingEntity);
            await _repository.UpdateAsync(existingEntity);
            return _mapper.Map<CustomerDto>(existingEntity);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<bool> RestoreAsync(long id)
        {
            return await _repository.RestoreAsync(id);
        }

        public async Task<IEnumerable<CustomerDto>> GetDeletedAsync()
        {
            var entities = await _repository.GetDeletedAsync();
            return _mapper.Map<IEnumerable<CustomerDto>>(entities);
        }

        public async Task<bool> PermanentDeleteAsync(long id)
        {
            return await _repository.PermanentDeleteAsync(id);
        }

        public async Task<IEnumerable<CustomerDto>> SearchAsync(string searchTerm)
        {
            var entities = await _repository.SearchAsync(searchTerm, "Name", "ContactPerson", "Email");
            return _mapper.Map<IEnumerable<CustomerDto>>(entities);
        }

        public async Task<IEnumerable<CustomerDto>> GetActiveCustomersAsync()
        {
            var entities = await _repository.GetActiveCustomersAsync();
            return _mapper.Map<IEnumerable<CustomerDto>>(entities);
        }
    }
} 