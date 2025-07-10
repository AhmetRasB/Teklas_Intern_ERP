using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Teklas_Intern_ERP.DTOs;

namespace Teklas_Intern_ERP.Business.Interfaces
{
    public interface IMaterialMovementService
    {
        // Basic CRUD Operations
        Task<List<MaterialMovementDto>> GetAllAsync();
        Task<MaterialMovementDto?> GetByIdAsync(long id);
        Task<MaterialMovementDto> AddAsync(MaterialMovementDto dto);
        Task<MaterialMovementDto> UpdateAsync(MaterialMovementDto dto);
        Task<bool> DeleteAsync(long id);
        Task<List<MaterialMovementDto>> GetDeletedAsync();
        Task<bool> RestoreAsync(long id);
        Task<bool> PermanentDeleteAsync(long id);
        
        // Material Card Related Operations
        Task<List<MaterialMovementDto>> GetMovementsByMaterialCardAsync(long materialCardId);
        Task<decimal> GetCurrentStockBalanceAsync(long materialCardId);
        Task<decimal> GetTotalMovementAmountAsync(long materialCardId, string movementType);
        
        // Date and Filter Operations
        Task<List<MaterialMovementDto>> GetMovementsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<List<MaterialMovementDto>> GetMovementsByTypeAsync(string movementType);
        Task<List<MaterialMovementDto>> GetMovementsByStatusAsync(string status);
        Task<List<MaterialMovementDto>> GetPendingMovementsAsync();
        
        // Location and Reference Operations
        Task<List<MaterialMovementDto>> GetMovementsByLocationAsync(string location);
        Task<List<MaterialMovementDto>> GetMovementsByReferenceAsync(string referenceNumber, string referenceType);
        
        // Search and General Operations
        Task<List<MaterialMovementDto>> GetRecentMovementsAsync(int take = 50);
        Task<List<MaterialMovementDto>> SearchMovementsAsync(string searchTerm);
        
        // Stock Management Operations
        Task<bool> UpdateStockBalanceAsync(long materialCardId, decimal newBalance);
        Task<MaterialMovementDto> ProcessStockInAsync(MaterialMovementDto dto);
        Task<MaterialMovementDto> ProcessStockOutAsync(MaterialMovementDto dto);
        Task<MaterialMovementDto> ProcessTransferAsync(MaterialMovementDto dto);
        Task<MaterialMovementDto> ProcessAdjustmentAsync(MaterialMovementDto dto);
        
        // Status Operations
        Task<bool> ConfirmMovementAsync(long id);
        Task<bool> CancelMovementAsync(long id);
        Task<bool> CompleteMovementAsync(long id);
        
        // Report Operations
        Task<List<MaterialMovementDto>> GetStockInReportAsync(DateTime startDate, DateTime endDate);
        Task<List<MaterialMovementDto>> GetStockOutReportAsync(DateTime startDate, DateTime endDate);
        Task<List<MaterialMovementDto>> GetTransferReportAsync(DateTime startDate, DateTime endDate);
        Task<List<MaterialMovementDto>> GetInventoryAdjustmentReportAsync(DateTime startDate, DateTime endDate);
    }
} 