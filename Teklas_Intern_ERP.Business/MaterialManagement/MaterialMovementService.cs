using System;
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
    public class MaterialMovementService : IMaterialMovementService
    {
        private readonly IMaterialMovementRepository _repository;
        private readonly IMaterialCardRepository _materialCardRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MaterialMovementService(
            IMaterialMovementRepository repository,
            IMaterialCardRepository materialCardRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _repository = repository;
            _materialCardRepository = materialCardRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region Basic CRUD Operations

        public async Task<List<MaterialMovementDto>> GetAllAsync()
        {
            var entities = await _repository.GetMovementsWithMaterialCardAsync();
            return _mapper.Map<List<MaterialMovementDto>>(entities);
        }

        public async Task<MaterialMovementDto?> GetByIdAsync(long id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity != null ? _mapper.Map<MaterialMovementDto>(entity) : null;
        }

        public async Task<MaterialMovementDto> AddAsync(MaterialMovementDto dto)
        {
            // VALIDATION
            var validator = new MaterialMovementDtoValidator();
            var validationResult = validator.Validate(dto);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                // Check if material card exists
                var materialCard = await _materialCardRepository.GetByIdAsync(dto.MaterialCardId);
                if (materialCard == null)
                    throw new InvalidOperationException("Belirtilen malzeme kartı bulunamadı.");

                var entity = _mapper.Map<MaterialMovement>(dto);
                
                // Set default values
                entity.MovementDate = dto.MovementDate == default ? DateTime.UtcNow : dto.MovementDate;
                entity.Status = string.IsNullOrEmpty(dto.Status) ? "PENDING" : dto.Status;
                entity.CreateUserId = 1; // TODO: Get from current user
                entity.UpdateUserId = 1;

                // Calculate total amount if not provided
                if (!entity.TotalAmount.HasValue && entity.UnitPrice.HasValue)
                {
                    entity.TotalAmount = entity.Quantity * entity.UnitPrice.Value;
                }

                // Calculate stock balance based on movement type
                await CalculateStockBalanceAsync(entity);

                var addedEntity = await _repository.AddAsync(entity);
                await _unitOfWork.CommitTransactionAsync();

                return _mapper.Map<MaterialMovementDto>(addedEntity);
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<MaterialMovementDto> UpdateAsync(MaterialMovementDto dto)
        {
            // VALIDATION
            var validator = new MaterialMovementDtoValidator();
            var validationResult = validator.Validate(dto);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var existingEntity = await _repository.GetByIdAsync(dto.Id);
                if (existingEntity == null)
                    throw new InvalidOperationException("Belirtilen hareket bulunamadı.");

                // Update entity
                _mapper.Map(dto, existingEntity);
                existingEntity.UpdateUserId = 1; // TODO: Get from current user
                existingEntity.UpdateDate = DateTime.UtcNow;

                // Recalculate stock balance if quantity or type changed
                await CalculateStockBalanceAsync(existingEntity);

                await _repository.UpdateAsync(existingEntity);
                await _unitOfWork.CommitTransactionAsync();

                return _mapper.Map<MaterialMovementDto>(existingEntity);
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

        public async Task<List<MaterialMovementDto>> GetDeletedAsync()
        {
            var entities = await _repository.GetDeletedAsync();
            return _mapper.Map<List<MaterialMovementDto>>(entities);
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

        #region Material Card Related Operations

        public async Task<List<MaterialMovementDto>> GetMovementsByMaterialCardAsync(long materialCardId)
        {
            var entities = await _repository.GetMovementsByMaterialCardAsync(materialCardId);
            return _mapper.Map<List<MaterialMovementDto>>(entities);
        }

        public async Task<decimal> GetCurrentStockBalanceAsync(long materialCardId)
        {
            return await _repository.GetCurrentStockBalanceAsync(materialCardId);
        }

        public async Task<decimal> GetTotalMovementAmountAsync(long materialCardId, string movementType)
        {
            return await _repository.GetTotalMovementAmountAsync(materialCardId, movementType);
        }

        #endregion

        #region Date and Filter Operations

        public async Task<List<MaterialMovementDto>> GetMovementsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var entities = await _repository.GetMovementsByDateRangeAsync(startDate, endDate);
            return _mapper.Map<List<MaterialMovementDto>>(entities);
        }

        public async Task<List<MaterialMovementDto>> GetMovementsByTypeAsync(string movementType)
        {
            var entities = await _repository.GetMovementsByTypeAsync(movementType);
            return _mapper.Map<List<MaterialMovementDto>>(entities);
        }

        public async Task<List<MaterialMovementDto>> GetMovementsByStatusAsync(string status)
        {
            var entities = await _repository.GetMovementsByStatusAsync(status);
            return _mapper.Map<List<MaterialMovementDto>>(entities);
        }

        public async Task<List<MaterialMovementDto>> GetPendingMovementsAsync()
        {
            var entities = await _repository.GetPendingMovementsAsync();
            return _mapper.Map<List<MaterialMovementDto>>(entities);
        }

        #endregion

        #region Location and Reference Operations

        public async Task<List<MaterialMovementDto>> GetMovementsByLocationAsync(string location)
        {
            var entities = await _repository.GetMovementsByLocationAsync(location);
            return _mapper.Map<List<MaterialMovementDto>>(entities);
        }

        public async Task<List<MaterialMovementDto>> GetMovementsByReferenceAsync(string referenceNumber, string referenceType)
        {
            var entities = await _repository.GetMovementsByReferenceAsync(referenceNumber, referenceType);
            return _mapper.Map<List<MaterialMovementDto>>(entities);
        }

        #endregion

        #region Search and General Operations

        public async Task<List<MaterialMovementDto>> GetRecentMovementsAsync(int take = 50)
        {
            var entities = await _repository.GetRecentMovementsAsync(take);
            return _mapper.Map<List<MaterialMovementDto>>(entities);
        }

        public async Task<List<MaterialMovementDto>> SearchMovementsAsync(string searchTerm)
        {
            var entities = await _repository.SearchMovementsAsync(searchTerm);
            return _mapper.Map<List<MaterialMovementDto>>(entities);
        }

        #endregion

        #region Stock Management Operations

        public async Task<bool> UpdateStockBalanceAsync(long materialCardId, decimal newBalance)
        {
            return await _repository.UpdateStockBalanceAsync(materialCardId, newBalance);
        }

        public async Task<MaterialMovementDto> ProcessStockInAsync(MaterialMovementDto dto)
        {
            dto.MovementType = "IN";
            dto.Quantity = Math.Abs(dto.Quantity); // Ensure positive for IN
            return await AddAsync(dto);
        }

        public async Task<MaterialMovementDto> ProcessStockOutAsync(MaterialMovementDto dto)
        {
            // Check stock availability
            var currentBalance = await GetCurrentStockBalanceAsync(dto.MaterialCardId);
            if (currentBalance < Math.Abs(dto.Quantity))
            {
                throw new InvalidOperationException($"Yetersiz stok. Mevcut: {currentBalance}, İstenen: {Math.Abs(dto.Quantity)}");
            }

            dto.MovementType = "OUT";
            dto.Quantity = -Math.Abs(dto.Quantity); // Ensure negative for OUT
            return await AddAsync(dto);
        }

        public async Task<MaterialMovementDto> ProcessTransferAsync(MaterialMovementDto dto)
        {
            if (string.IsNullOrEmpty(dto.LocationFrom) || string.IsNullOrEmpty(dto.LocationTo))
            {
                throw new InvalidOperationException("Transfer işlemi için çıkış ve varış lokasyonları zorunludur.");
            }

            dto.MovementType = "TRANSFER";
            return await AddAsync(dto);
        }

        public async Task<MaterialMovementDto> ProcessAdjustmentAsync(MaterialMovementDto dto)
        {
            dto.MovementType = "ADJUSTMENT";
            return await AddAsync(dto);
        }

        #endregion

        #region Status Operations

        public async Task<bool> ConfirmMovementAsync(long id)
        {
            var movement = await _repository.GetByIdAsync(id);
            if (movement == null) return false;

            movement.Status = "CONFIRMED";
            movement.UpdateDate = DateTime.UtcNow;
            movement.UpdateUserId = 1; // TODO: Get from current user

            await _repository.UpdateAsync(movement);
            return true;
        }

        public async Task<bool> CancelMovementAsync(long id)
        {
            var movement = await _repository.GetByIdAsync(id);
            if (movement == null) return false;

            movement.Status = "CANCELLED";
            movement.UpdateDate = DateTime.UtcNow;
            movement.UpdateUserId = 1; // TODO: Get from current user

            await _repository.UpdateAsync(movement);
            return true;
        }

        public async Task<bool> CompleteMovementAsync(long id)
        {
            var movement = await _repository.GetByIdAsync(id);
            if (movement == null) return false;

            movement.Status = "COMPLETED";
            movement.UpdateDate = DateTime.UtcNow;
            movement.UpdateUserId = 1; // TODO: Get from current user

            await _repository.UpdateAsync(movement);
            return true;
        }

        #endregion

        #region Report Operations

        public async Task<List<MaterialMovementDto>> GetStockInReportAsync(DateTime startDate, DateTime endDate)
        {
            var entities = await _repository.GetMovementsByDateRangeAsync(startDate, endDate);
            var stockInMovements = entities.Where(m => m.MovementType == "IN").ToList();
            return _mapper.Map<List<MaterialMovementDto>>(stockInMovements);
        }

        public async Task<List<MaterialMovementDto>> GetStockOutReportAsync(DateTime startDate, DateTime endDate)
        {
            var entities = await _repository.GetMovementsByDateRangeAsync(startDate, endDate);
            var stockOutMovements = entities.Where(m => m.MovementType == "OUT").ToList();
            return _mapper.Map<List<MaterialMovementDto>>(stockOutMovements);
        }

        public async Task<List<MaterialMovementDto>> GetTransferReportAsync(DateTime startDate, DateTime endDate)
        {
            var entities = await _repository.GetMovementsByDateRangeAsync(startDate, endDate);
            var transferMovements = entities.Where(m => m.MovementType == "TRANSFER").ToList();
            return _mapper.Map<List<MaterialMovementDto>>(transferMovements);
        }

        public async Task<List<MaterialMovementDto>> GetInventoryAdjustmentReportAsync(DateTime startDate, DateTime endDate)
        {
            var entities = await _repository.GetMovementsByDateRangeAsync(startDate, endDate);
            var adjustmentMovements = entities.Where(m => m.MovementType == "ADJUSTMENT").ToList();
            return _mapper.Map<List<MaterialMovementDto>>(adjustmentMovements);
        }

        #endregion

        #region Private Helper Methods

        private async Task CalculateStockBalanceAsync(MaterialMovement movement)
        {
            var currentBalance = await _repository.GetCurrentStockBalanceAsync(movement.MaterialCardId);
            
            decimal newBalance = movement.MovementType?.ToUpper() switch
            {
                "IN" or "PRODUCTION" or "RETURN" => currentBalance + Math.Abs(movement.Quantity),
                "OUT" or "CONSUMPTION" => currentBalance - Math.Abs(movement.Quantity),
                "TRANSFER" => currentBalance, // No change in total stock for transfers
                "ADJUSTMENT" => currentBalance + movement.Quantity, // Can be positive or negative
                _ => currentBalance
            };

            movement.StockBalance = Math.Max(0, newBalance); // Prevent negative stock
        }

        #endregion
    }
} 