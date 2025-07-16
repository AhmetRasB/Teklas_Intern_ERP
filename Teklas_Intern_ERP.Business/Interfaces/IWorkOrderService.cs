using Teklas_Intern_ERP.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Teklas_Intern_ERP.Business.Interfaces;

public interface IWorkOrderService
{
    Task<WorkOrderDto?> GetByIdAsync(long id);
    Task<List<WorkOrderDto>> GetAllAsync();
    Task<WorkOrderDto> CreateAsync(CreateWorkOrderDto dto);
    Task<WorkOrderDto> UpdateAsync(UpdateWorkOrderDto dto);
    Task<bool> SoftDeleteAsync(long id);
    Task<bool> RestoreAsync(long id);
    Task<List<WorkOrderDto>> GetDeletedAsync();
    Task<bool> PermanentDeleteAsync(long id);
} 