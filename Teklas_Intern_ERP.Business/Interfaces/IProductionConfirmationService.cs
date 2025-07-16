using Teklas_Intern_ERP.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Teklas_Intern_ERP.Business.Interfaces;

public interface IProductionConfirmationService
{
    Task<ProductionConfirmationDto?> GetByIdAsync(long id);
    Task<List<ProductionConfirmationDto>> GetAllAsync();
    Task<ProductionConfirmationDto> CreateAsync(CreateProductionConfirmationDto dto);
    Task<ProductionConfirmationDto> UpdateAsync(UpdateProductionConfirmationDto dto);
    Task<bool> SoftDeleteAsync(long id);
    Task<bool> RestoreAsync(long id);
    Task<List<ProductionConfirmationDto>> GetDeletedAsync();
    Task<bool> PermanentDeleteAsync(long id);
} 