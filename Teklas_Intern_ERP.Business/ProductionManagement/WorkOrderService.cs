using Teklas_Intern_ERP.Business.Interfaces;
using Teklas_Intern_ERP.DTOs;
using Teklas_Intern_ERP.Entities.ProductionManagment;
using Teklas_Intern_ERP.DataAccess.Repositories;
using AutoMapper;

namespace Teklas_Intern_ERP.Business.ProductionManagement;

public class WorkOrderService : IWorkOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public WorkOrderService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<WorkOrderDto?> GetByIdAsync(long id)
    {
        var entity = await _unitOfWork.WorkOrderRepository.GetWithOperationsAsync(id);
        return entity == null ? null : _mapper.Map<WorkOrderDto>(entity);
    }

    public async Task<List<WorkOrderDto>> GetAllAsync()
    {
        var entities = await _unitOfWork.WorkOrderRepository.GetAllWithOperationsAsync();
        return _mapper.Map<List<WorkOrderDto>>(entities);
    }

    public async Task<WorkOrderDto> CreateAsync(CreateWorkOrderDto dto)
    {
        var entity = _mapper.Map<WorkOrder>(dto);
        await _unitOfWork.WorkOrderRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<WorkOrderDto>(entity);
    }

    public async Task<WorkOrderDto> UpdateAsync(UpdateWorkOrderDto dto)
    {
        var entity = await _unitOfWork.WorkOrderRepository.GetByIdAsync(dto.WorkOrderId);
        if (entity == null) throw new Exception("İş emri bulunamadı.");
        _mapper.Map(dto, entity);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<WorkOrderDto>(entity);
    }

    public async Task<bool> SoftDeleteAsync(long id)
    {
        var entity = await _unitOfWork.WorkOrderRepository.GetByIdForDeleteAsync(id);
        if (entity == null) return false;
        entity.IsDeleted = true;
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RestoreAsync(long id)
    {
        var entity = await _unitOfWork.WorkOrderRepository.GetByIdForDeleteAsync(id);
        if (entity == null) return false;
        entity.IsDeleted = false;
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<List<WorkOrderDto>> GetDeletedAsync()
    {
        var entities = await _unitOfWork.WorkOrderRepository.GetDeletedAsync();
        return _mapper.Map<List<WorkOrderDto>>(entities);
    }

    public async Task<bool> PermanentDeleteAsync(long id)
    {
        var entity = await _unitOfWork.WorkOrderRepository.GetByIdForDeleteAsync(id);
        if (entity == null) return false;
        await _unitOfWork.WorkOrderRepository.PermanentDeleteAsync(entity);
        return true;
    }
} 