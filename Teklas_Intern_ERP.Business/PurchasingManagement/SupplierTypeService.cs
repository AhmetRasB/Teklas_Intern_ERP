using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Teklas_Intern_ERP.Business.Interfaces;
using Teklas_Intern_ERP.DataAccess;
using Teklas_Intern_ERP.DataAccess.PurchasingManagement;
using Teklas_Intern_ERP.DTOs.PurchasingManagement;
using Teklas_Intern_ERP.Entities.PurchasingManagement;

namespace Teklas_Intern_ERP.Business.PurchasingManagement
{
    public sealed class SupplierTypeService : ISupplierTypeService
    {
        private readonly ISupplierTypeRepository _repository;
        private readonly IMapper _mapper;

        public SupplierTypeService(ISupplierTypeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SupplierTypeDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<SupplierTypeDto>>(entities);
        }

        public async Task<SupplierTypeDto?> GetByIdAsync(long id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return _mapper.Map<SupplierTypeDto>(entity);
        }

        public async Task<SupplierTypeDto> CreateAsync(SupplierTypeDto dto)
        {
            var entity = _mapper.Map<SupplierType>(dto);
            var createdEntity = await _repository.AddAsync(entity);
            return _mapper.Map<SupplierTypeDto>(createdEntity);
        }

        public async Task<SupplierTypeDto> UpdateAsync(long id, SupplierTypeDto dto)
        {
            var existingEntity = await _repository.GetByIdAsync(id);
            if (existingEntity == null)
                throw new ArgumentException("Supplier type not found");

            _mapper.Map(dto, existingEntity);
            await _repository.UpdateAsync(existingEntity);
            return _mapper.Map<SupplierTypeDto>(existingEntity);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<bool> RestoreAsync(long id)
        {
            return await _repository.RestoreAsync(id);
        }

        public async Task<IEnumerable<SupplierTypeDto>> SearchAsync(string searchTerm)
        {
            var entities = await _repository.SearchAsync(searchTerm, "Name", "Description");
            return _mapper.Map<IEnumerable<SupplierTypeDto>>(entities);
        }

        public async Task<IEnumerable<SupplierTypeDto>> GetActiveSupplierTypesAsync()
        {
            var entities = await _repository.GetActiveSupplierTypesAsync();
            return _mapper.Map<IEnumerable<SupplierTypeDto>>(entities);
        }

        public async Task<IEnumerable<SupplierTypeDto>> GetDeletedAsync()
        {
            var entities = await _repository.GetDeletedAsync();
            return _mapper.Map<IEnumerable<SupplierTypeDto>>(entities);
        }

        public async Task<bool> PermanentDeleteAsync(long id)
        {
            return await _repository.PermanentDeleteAsync(id);
        }
    }
} 