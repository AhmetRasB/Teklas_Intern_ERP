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
    public class MaterialCategoryService : IMaterialCategoryService
    {
        private readonly IMaterialCategoryRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MaterialCategoryService(
            IMaterialCategoryRepository repository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region Basic CRUD Operations

        public async Task<List<MaterialCategoryDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<List<MaterialCategoryDto>>(entities);
        }

        public async Task<MaterialCategoryDto?> GetByIdAsync(long id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return _mapper.Map<MaterialCategoryDto>(entity);
        }

        public async Task<MaterialCategoryDto> AddAsync(MaterialCategoryDto dto)
        {
            // VALIDATION
            var validator = new MaterialCategoryDto.MaterialCategoryDtoValidator();
            var validationResult = validator.Validate(dto);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var entity = _mapper.Map<MaterialCategory>(dto);
                entity.Status = Entities.StatusType.Active;
                entity.CreateUserId = 1;
                entity.UpdateUserId = 1;
                
                var addedEntity = await _repository.AddAsync(entity);
                await _unitOfWork.CommitTransactionAsync();

                return _mapper.Map<MaterialCategoryDto>(addedEntity);
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<MaterialCategoryDto> UpdateAsync(MaterialCategoryDto dto)
        {
            // VALIDATION
            var validator = new MaterialCategoryDto.MaterialCategoryDtoValidator();
            var validationResult = validator.Validate(dto);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var entity = _mapper.Map<MaterialCategory>(dto);
                await _repository.UpdateAsync(entity);
                await _unitOfWork.CommitTransactionAsync();

                return _mapper.Map<MaterialCategoryDto>(entity);
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

        public async Task<List<MaterialCategoryDto>> GetDeletedAsync()
        {
            var entities = await _repository.GetDeletedAsync();
            return _mapper.Map<List<MaterialCategoryDto>>(entities);
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

        public async Task<List<MaterialCategoryDto>> GetRootCategoriesAsync()
        {
            var entities = await _repository.GetRootCategoriesAsync();
            return _mapper.Map<List<MaterialCategoryDto>>(entities);
        }

        public async Task<List<MaterialCategoryDto>> GetSubCategoriesAsync(long parentId)
        {
            var entities = await _repository.GetSubCategoriesAsync(parentId);
            return _mapper.Map<List<MaterialCategoryDto>>(entities);
        }

        public async Task<bool> HasMaterialsAsync(long categoryId)
        {
            return await _repository.HasMaterialsAsync(categoryId);
        }

        public async Task<int> GetMaterialCountAsync(long categoryId)
        {
            return await _repository.GetMaterialCountAsync(categoryId);
        }

        #endregion
    }
} 