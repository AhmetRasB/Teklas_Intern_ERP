using Teklas_Intern_ERP.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Teklas_Intern_ERP.Business.Interfaces;

public interface IBillOfMaterialService
{
    Task<BOMHeaderDto?> GetByIdAsync(long id);
    Task<List<BOMHeaderDto>> GetAllAsync();
    Task<BOMHeaderDto> CreateAsync(CreateBOMHeaderDto dto);
    Task<BOMHeaderDto> UpdateAsync(UpdateBOMHeaderDto dto);
    Task<bool> SoftDeleteAsync(long id);
    Task<bool> RestoreAsync(long id);
    Task<List<BOMHeaderDto>> GetDeletedAsync();
    Task<bool> PermanentDeleteAsync(long id);
} 