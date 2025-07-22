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
    public sealed class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _repository;
        private readonly IMapper _mapper;

        public SupplierService(ISupplierRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SupplierDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync("SupplierType");
            return _mapper.Map<IEnumerable<SupplierDto>>(entities);
        }

        public async Task<SupplierDto?> GetByIdAsync(long id)
        {
            var entity = await _repository.GetByIdAsync(id, "SupplierType");
            return _mapper.Map<SupplierDto>(entity);
        }

        public async Task<SupplierDto> CreateAsync(SupplierDto dto)
        {
            var entity = _mapper.Map<Supplier>(dto);
            var createdEntity = await _repository.AddAsync(entity);
            return _mapper.Map<SupplierDto>(createdEntity);
        }

        public async Task<SupplierDto> UpdateAsync(long id, SupplierDto dto)
        {
            var existingEntity = await _repository.GetByIdAsync(id);
            if (existingEntity == null)
                throw new ArgumentException("Supplier not found");

            _mapper.Map(dto, existingEntity);
            await _repository.UpdateAsync(existingEntity);
            return _mapper.Map<SupplierDto>(existingEntity);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<bool> RestoreAsync(long id)
        {
            return await _repository.RestoreAsync(id);
        }

        public async Task<IEnumerable<SupplierDto>> SearchAsync(string searchTerm)
        {
            var entities = await _repository.SearchAsync(searchTerm, "Name", "ContactPerson", "Email");
            return _mapper.Map<IEnumerable<SupplierDto>>(entities);
        }

        public async Task<IEnumerable<SupplierDto>> GetActiveSuppliersAsync()
        {
            var entities = await _repository.GetActiveSuppliersAsync();
            return _mapper.Map<IEnumerable<SupplierDto>>(entities);
        }

        public async Task<IEnumerable<SupplierDto>> GetSuppliersByTypeAsync(long supplierTypeId)
        {
            var entities = await _repository.GetSuppliersByTypeAsync(supplierTypeId);
            return _mapper.Map<IEnumerable<SupplierDto>>(entities);
        }

        public async Task<IEnumerable<SupplierDto>> GetDeletedAsync()
        {
            var entities = await _repository.GetDeletedAsync();
            return _mapper.Map<IEnumerable<SupplierDto>>(entities);
        }

        public async Task<bool> PermanentDeleteAsync(long id)
        {
            return await _repository.PermanentDeleteAsync(id);
        }
    }
} 