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
    public class StockEntryService : IStockEntryService
    {
        private readonly IStockEntryRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StockEntryService(
            IStockEntryRepository repository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;   
        }

        #region Basic CRUD Operations

        public async Task<List<StockEntryDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<List<StockEntryDto>>(entities);
        }

        public async Task<StockEntryDto?> GetByIdAsync(long id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return _mapper.Map<StockEntryDto>(entity);
        }

        public async Task<StockEntryDto> AddAsync(StockEntryDto dto)
        {
            // VALIDATION
            var validator = new StockEntryDtoValidator();
            var validationResult = validator.Validate(dto);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var entity = _mapper.Map<StockEntry>(dto);
                entity.Status = "Active";
                entity.CreateUserId = 1;
                entity.UpdateUserId = 1;
                
                var addedEntity = await _repository.AddAsync(entity);
                await _unitOfWork.CommitTransactionAsync();

                return _mapper.Map<StockEntryDto>(addedEntity);
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<StockEntryDto> UpdateAsync(StockEntryDto dto)
        {
            // VALIDATION
            var validator = new StockEntryDtoValidator();
            var validationResult = validator.Validate(dto);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var entity = _mapper.Map<StockEntry>(dto);
                await _repository.UpdateAsync(entity);
                await _unitOfWork.CommitTransactionAsync();

                return _mapper.Map<StockEntryDto>(entity);
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

        public async Task<List<StockEntryDto>> GetDeletedAsync()
        {
            var entities = await _repository.GetDeletedAsync();
            return _mapper.Map<List<StockEntryDto>>(entities);
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

        public async Task<List<StockEntryDto>> GetStockEntriesByWarehouseAsync(long warehouseId)
        {
            var entities = await _repository.GetStockEntriesByWarehouseAsync(warehouseId);
            return _mapper.Map<List<StockEntryDto>>(entities);
        }

        public async Task<List<StockEntryDto>> GetStockEntriesByLocationAsync(long locationId)
        {
            var entities = await _repository.GetStockEntriesByLocationAsync(locationId);
            return _mapper.Map<List<StockEntryDto>>(entities);
        }

        public async Task<List<StockEntryDto>> GetStockEntriesByMaterialAsync(long materialId)
        {
            var entities = await _repository.GetStockEntriesByMaterialAsync(materialId);
            return _mapper.Map<List<StockEntryDto>>(entities);
        }

        public async Task<List<StockEntryDto>> GetStockEntriesByTypeAsync(string entryType)
        {
            var entities = await _repository.GetStockEntriesByTypeAsync(entryType);
            return _mapper.Map<List<StockEntryDto>>(entities);
        }

        public async Task<List<StockEntryDto>> SearchStockEntriesAsync(string searchTerm)
        {
            var entities = await _repository.SearchStockEntriesAsync(searchTerm);
            return _mapper.Map<List<StockEntryDto>>(entities);
        }

        public async Task<bool> IsEntryNumberUniqueAsync(string entryNumber, long? excludeId = null)
        {
            return await _repository.IsEntryNumberUniqueAsync(entryNumber, excludeId);
        }

        #endregion
    }
} 