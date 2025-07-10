using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Teklas_Intern_ERP.Business.Interfaces;
using Teklas_Intern_ERP.DataAccess.MaterialManagement;
using Teklas_Intern_ERP.DataAccess.Repositories;
using Teklas_Intern_ERP.DTOs;
using Teklas_Intern_ERP.Entities.MaterialManagement;
using ValidationException = FluentValidation.ValidationException;

namespace Teklas_Intern_ERP.Business.MaterialManagement
{
    public class MaterialCardService : IMaterialCardService
    {
        private readonly IMaterialCardRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MaterialCardService(
            IMaterialCardRepository repository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;   
        }

        #region Basic CRUD Operations

        public async Task<List<MaterialCardDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<List<MaterialCardDto>>(entities);
        }

        public async Task<MaterialCardDto?> GetByIdAsync(long id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return _mapper.Map<MaterialCardDto>(entity);
        }

        public async Task<MaterialCardDto> AddAsync(MaterialCardDto dto)
        {
            // VALIDATION
            var validator = new MaterialCardDtoValidator();
            var validationResult = validator.Validate(dto);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var entity = _mapper.Map<MaterialCard>(dto);
                entity.Status = "Active";
                entity.CreateUserId = 1;
                entity.UpdateUserId = 1;
                
                var addedEntity = await _repository.AddAsync(entity);
                await _unitOfWork.CommitTransactionAsync();

                return _mapper.Map<MaterialCardDto>(addedEntity);
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<MaterialCardDto> UpdateAsync(MaterialCardDto dto)
        {
            // VALIDATION
            var validator = new MaterialCardDtoValidator();
            var validationResult = validator.Validate(dto);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var entity = _mapper.Map<MaterialCard>(dto);
                await _repository.UpdateAsync(entity);
                await _unitOfWork.CommitTransactionAsync();

                return _mapper.Map<MaterialCardDto>(entity);
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

        public async Task<List<MaterialCardDto>> GetDeletedAsync()
        {
            var entities = await _repository.GetDeletedAsync();
            return _mapper.Map<List<MaterialCardDto>>(entities);
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

        #region Basic Search Operations

        public async Task<List<MaterialCardDto>> GetMaterialsByCategoryAsync(long categoryId)
        {
            var entities = await _repository.GetMaterialsByCategoryAsync(categoryId);
            return _mapper.Map<List<MaterialCardDto>>(entities);
        }

        public async Task<List<MaterialCardDto>> SearchMaterialsAsync(string searchTerm)
        {
            var entities = await _repository.SearchMaterialsAsync(searchTerm);
            return _mapper.Map<List<MaterialCardDto>>(entities);
        }

        public async Task<MaterialCardDto?> GetMaterialByBarcodeAsync(string barcode)
        {
            var entity = await _repository.GetMaterialByBarcodeAsync(barcode);
            return _mapper.Map<MaterialCardDto>(entity);
        }

        public async Task<bool> IsMaterialCodeUniqueAsync(string code, long? excludeId = null)
        {
            return await _repository.IsMaterialCodeUniqueAsync(code, excludeId);
        }

        #endregion
    }
} 