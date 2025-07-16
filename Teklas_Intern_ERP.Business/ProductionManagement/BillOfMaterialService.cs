using Teklas_Intern_ERP.Business.Interfaces;
using Teklas_Intern_ERP.DTOs;
using Teklas_Intern_ERP.Entities.ProductionManagment;
using Teklas_Intern_ERP.DataAccess.Repositories;
using AutoMapper;

namespace Teklas_Intern_ERP.Business.ProductionManagement;

public class BillOfMaterialService : IBillOfMaterialService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public BillOfMaterialService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BOMHeaderDto?> GetByIdAsync(long id)
    {
        var entity = await _unitOfWork.BillOfMaterialRepository.GetWithItemsAsync(id);
        return entity == null ? null : _mapper.Map<BOMHeaderDto>(entity);
    }

    public async Task<List<BOMHeaderDto>> GetAllAsync()
    {
        var entities = await _unitOfWork.BillOfMaterialRepository.GetAllWithItemsAsync();
        return _mapper.Map<List<BOMHeaderDto>>(entities);
    }

    public async Task<BOMHeaderDto> CreateAsync(CreateBOMHeaderDto dto)
    {
        var entity = _mapper.Map<BOMHeader>(dto);
        await _unitOfWork.BillOfMaterialRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<BOMHeaderDto>(entity);
    }

    public async Task<BOMHeaderDto> UpdateAsync(UpdateBOMHeaderDto dto)
    {
        var entity = await _unitOfWork.BillOfMaterialRepository.GetWithItemsAsync(dto.BOMHeaderId);
        if (entity == null) throw new Exception("BOM bulunamadı.");
        _mapper.Map(dto, entity);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<BOMHeaderDto>(entity);
    }

    public async Task<bool> SoftDeleteAsync(long id)
    {
        var entity = await _unitOfWork.BillOfMaterialRepository.GetByIdForDeleteAsync(id);
        if (entity == null) return false;
        entity.IsDeleted = true;
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RestoreAsync(long id)
    {
        var entity = await _unitOfWork.BillOfMaterialRepository.GetByIdForDeleteAsync(id);
        if (entity == null) return false;
        entity.IsDeleted = false;
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<List<BOMHeaderDto>> GetDeletedAsync()
    {
        var entities = await _unitOfWork.BillOfMaterialRepository.GetDeletedAsync();
        return _mapper.Map<List<BOMHeaderDto>>(entities);
    }

    public async Task<bool> PermanentDeleteAsync(long id)
    {
        var entity = await _unitOfWork.BillOfMaterialRepository.GetByIdForDeleteAsync(id);
        if (entity == null) return false;
        await _unitOfWork.BillOfMaterialRepository.PermanentDeleteAsync(entity);
        return true;
    }
} 