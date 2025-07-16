using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Teklas_Intern_ERP.Business.Interfaces;
using Teklas_Intern_ERP.DataAccess.WarehouseManagement;
using Teklas_Intern_ERP.DataAccess.Repositories;
using Teklas_Intern_ERP.DTOs.WarehouseManagement;
using Teklas_Intern_ERP.Entities.WarehouseManagement;
using ValidationException = FluentValidation.ValidationException;

namespace Teklas_Intern_ERP.Business.WarehouseManagement
{
    public class WarehouseService : IWarehouseService
    {
        private readonly IWarehouseRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public WarehouseService(
            IWarehouseRepository repository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;   
        }

        #region Basic CRUD Operations

        public async Task<List<WarehouseDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<List<WarehouseDto>>(entities);
        }

        public async Task<WarehouseDto?> GetByIdAsync(long id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return _mapper.Map<WarehouseDto>(entity);
        }

        public async Task<WarehouseDto> AddAsync(WarehouseDto dto)
        {
            // VALIDATION
            var validator = new WarehouseDtoValidator();
            var validationResult = validator.Validate(dto);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var entity = _mapper.Map<Warehouse>(dto);
                entity.Status = "Active";
                entity.CreateUserId = 1;
                entity.UpdateUserId = 1;
                
                var addedEntity = await _repository.AddAsync(entity);
                await _unitOfWork.CommitTransactionAsync();

                return _mapper.Map<WarehouseDto>(addedEntity);
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<WarehouseDto> UpdateAsync(WarehouseDto dto)
        {
            // VALIDATION
            var validator = new WarehouseDtoValidator();
            var validationResult = validator.Validate(dto);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var entity = _mapper.Map<Warehouse>(dto);
                await _repository.UpdateAsync(entity);
                await _unitOfWork.CommitTransactionAsync();

                return _mapper.Map<WarehouseDto>(entity);
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<bool> DeleteAsync(long id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<List<WarehouseDto>> GetDeletedAsync()
        {
            var entities = await _repository.GetDeletedAsync();
            return _mapper.Map<List<WarehouseDto>>(entities);
        }

        public async Task<bool> RestoreAsync(long id)
        {
            return await _repository.RestoreAsync(id);
        }

        public async Task<bool> PermanentDeleteAsync(long id)
        {
            return await _repository.PermanentDeleteAsync(id);
        }

        #endregion

        #region Search Operations

        public async Task<List<WarehouseDto>> SearchWarehousesAsync(string searchTerm)
        {
            var entities = await _repository.SearchWarehousesAsync(searchTerm);
            return _mapper.Map<List<WarehouseDto>>(entities);
        }

        public async Task<bool> IsWarehouseCodeUniqueAsync(string code, long? excludeId = null)
        {
            return await _repository.IsWarehouseCodeUniqueAsync(code, excludeId);
        }

        public async Task<List<WarehouseDto>> GetWarehousesByTypeAsync(string warehouseType)
        {
            var entities = await _repository.GetWarehousesByTypeAsync(warehouseType);
            return _mapper.Map<List<WarehouseDto>>(entities);
        }

        #endregion
    }
} 