using Teklas_Intern_ERP.Business.Interfaces;
using Teklas_Intern_ERP.DTOs;
using Teklas_Intern_ERP.Entities.ProductionManagment;
using Teklas_Intern_ERP.DataAccess.Repositories;
using AutoMapper;

namespace Teklas_Intern_ERP.Business.ProductionManagement;

public class ProductionConfirmationService : IProductionConfirmationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ProductionConfirmationService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ProductionConfirmationDto?> GetByIdAsync(long id)
    {
        var entity = await _unitOfWork.ProductionConfirmationRepository.GetWithConsumptionsAsync(id);
        return entity == null ? null : _mapper.Map<ProductionConfirmationDto>(entity);
    }

    public async Task<List<ProductionConfirmationDto>> GetAllAsync()
    {
        var entities = await _unitOfWork.ProductionConfirmationRepository.GetAllWithConsumptionsAsync();
        return _mapper.Map<List<ProductionConfirmationDto>>(entities);
    }

    public async Task<ProductionConfirmationDto> CreateAsync(CreateProductionConfirmationDto dto)
    {
        var entity = _mapper.Map<ProductionConfirmation>(dto);
        await _unitOfWork.ProductionConfirmationRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<ProductionConfirmationDto>(entity);
    }

    public async Task<ProductionConfirmationDto> UpdateAsync(UpdateProductionConfirmationDto dto)
    {
        var entity = await _unitOfWork.ProductionConfirmationRepository.GetByIdAsync(dto.ConfirmationId);
        if (entity == null) throw new Exception("Üretim teyidi bulunamadı.");
        _mapper.Map(dto, entity);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<ProductionConfirmationDto>(entity);
    }

    public async Task<bool> SoftDeleteAsync(long id)
    {
        var entity = await _unitOfWork.ProductionConfirmationRepository.GetByIdForDeleteAsync(id);
        if (entity == null) return false;
        entity.IsDeleted = true;
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RestoreAsync(long id)
    {
        var entity = await _unitOfWork.ProductionConfirmationRepository.GetDeletedByIdAsync(id);
        if (entity == null) return false;
        entity.IsDeleted = false;
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<List<ProductionConfirmationDto>> GetDeletedAsync()
    {
        var entities = await _unitOfWork.ProductionConfirmationRepository.GetAllDeletedAsync();
        return _mapper.Map<List<ProductionConfirmationDto>>(entities);
    }

    public async Task<bool> PermanentDeleteAsync(long id)
    {
        var entity = await _unitOfWork.ProductionConfirmationRepository.GetDeletedByIdAsync(id);
        if (entity == null) return false;
        await _unitOfWork.ProductionConfirmationRepository.DeleteAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
} 