using AutoMapper;
using Teklas_Intern_ERP.Entities.MaterialManagement;
using Teklas_Intern_ERP.Entities.UserManagement;
using Teklas_Intern_ERP.Entities.ProductionManagement;
using Teklas_Intern_ERP.DTOs;

namespace Teklas_Intern_ERP.Business.Mapping
{
    public class EntityMappingProfile : Profile
    {
        public EntityMappingProfile()
        {
            // MaterialManagement Mappings
            CreateMap<MaterialCard, MaterialCardDto>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.CardCode))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CardName))
                .ForMember(dest => dest.MaterialType, opt => opt.MapFrom(src => src.CardType))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.MaterialCategoryId))
                .ForMember(dest => dest.UnitOfMeasure, opt => opt.MapFrom(src => src.Unit))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.MaterialCategory != null ? src.MaterialCategory.CategoryName : null))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.Status == "Active"))
                .ForMember(dest => dest.ManufacturerPartNumber, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(dest => dest.CardCode, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.CardName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.CardType, opt => opt.MapFrom(src => src.MaterialType))
                .ForMember(dest => dest.MaterialCategoryId, opt => opt.MapFrom(src => src.CategoryId ?? 1))
                .ForMember(dest => dest.Unit, opt => opt.MapFrom(src => src.UnitOfMeasure))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.IsActive ? "Active" : "Inactive"))
                .ForMember(dest => dest.MaterialCategory, opt => opt.Ignore())
                .ForMember(dest => dest.MaterialMovements, opt => opt.Ignore());

            CreateMap<MaterialCategory, MaterialCategoryDto>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.CategoryCode))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CategoryName))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status == "Active" ? 1 : 0))
                .ForMember(dest => dest.ParentCategoryName, opt => opt.MapFrom(src => src.ParentCategory != null ? src.ParentCategory.CategoryName : null))
                .ReverseMap()
                .ForMember(dest => dest.CategoryCode, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status == 1 ? "Active" : "Inactive"))
                .ForMember(dest => dest.ParentCategory, opt => opt.Ignore())
                .ForMember(dest => dest.ChildCategories, opt => opt.Ignore())
                .ForMember(dest => dest.MaterialCards, opt => opt.Ignore());

            // UserManagement Mappings
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.RoleNames, opt => opt.MapFrom(src => src.UserRoles.Select(ur => ur.Role.Name).ToList()))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}".Trim()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (int)src.Status))
                .ReverseMap()
                .ForMember(dest => dest.UserRoles, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordSalt, opt => opt.Ignore());

            CreateMap<Role, RoleDto>()
                .ReverseMap()
                .ForMember(dest => dest.UserRoles, opt => opt.Ignore());

            // MaterialMovement Mappings
            CreateMap<MaterialMovement, MaterialMovementDto>()
                .ForMember(dest => dest.MaterialCardName, opt => opt.MapFrom(src => src.MaterialCard.CardName))
                .ForMember(dest => dest.MaterialCardCode, opt => opt.MapFrom(src => src.MaterialCard.CardCode))
                .ForMember(dest => dest.MovementTypeDisplay, opt => opt.MapFrom(src => GetMovementTypeDisplay(src.MovementType)))
                .ForMember(dest => dest.StatusDisplay, opt => opt.MapFrom(src => GetStatusDisplay(src.Status)))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => src.UpdateDate))
                .ReverseMap()
                .ForMember(dest => dest.MaterialCard, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Condition(src => src.Id > 0));

            // ProductionManagement Mappings

            // BillOfMaterial mappings
            CreateMap<BillOfMaterial, BillOfMaterialDto>()
                .ForMember(dto => dto.ProductMaterialCardName, opt => opt.MapFrom(src => src.ProductMaterialCard != null ? src.ProductMaterialCard.CardName : null))
                .ForMember(dto => dto.ProductMaterialCardCode, opt => opt.MapFrom(src => src.ProductMaterialCard != null ? src.ProductMaterialCard.CardCode : null))
                .ForMember(dto => dto.ApprovedByUserName, opt => opt.Ignore()) // Will be set by service layer
                .ForMember(dto => dto.CreatedAt, opt => opt.MapFrom(src => src.CreateDate))
                .ForMember(dto => dto.UpdatedAt, opt => opt.MapFrom(src => src.UpdateDate))
                .ForMember(dto => dto.CreatedByUserId, opt => opt.MapFrom(src => src.CreateUserId))
                .ForMember(dto => dto.UpdatedByUserId, opt => opt.MapFrom(src => src.UpdateUserId))
                .ForMember(dto => dto.CreatedByUserName, opt => opt.Ignore()) // Will be set by service layer
                .ForMember(dto => dto.UpdatedByUserName, opt => opt.Ignore()); // Will be set by service layer

            CreateMap<BillOfMaterialDto, BillOfMaterial>()
                .ForMember(entity => entity.ProductMaterialCard, opt => opt.Ignore())
                .ForMember(entity => entity.BOMItems, opt => opt.Ignore())
                .ForMember(entity => entity.WorkOrders, opt => opt.Ignore())
                .ForMember(entity => entity.CreateDate, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(entity => entity.UpdateDate, opt => opt.MapFrom(src => src.UpdatedAt))
                .ForMember(entity => entity.CreateUserId, opt => opt.MapFrom(src => src.CreatedByUserId))
                .ForMember(entity => entity.UpdateUserId, opt => opt.MapFrom(src => src.UpdatedByUserId));

            // BillOfMaterialItem mappings
            CreateMap<BillOfMaterialItem, BillOfMaterialItemDto>()
                .ForMember(dto => dto.MaterialCardName, opt => opt.MapFrom(src => src.MaterialCard != null ? src.MaterialCard.CardName : null))
                .ForMember(dto => dto.MaterialCardCode, opt => opt.MapFrom(src => src.MaterialCard != null ? src.MaterialCard.CardCode : null))
                .ForMember(dto => dto.SupplierMaterialCardName, opt => opt.MapFrom(src => src.SupplierMaterialCard != null ? src.SupplierMaterialCard.CardName : null))
                .ForMember(dto => dto.CreatedAt, opt => opt.MapFrom(src => src.CreateDate))
                .ForMember(dto => dto.UpdatedAt, opt => opt.MapFrom(src => src.UpdateDate));

            CreateMap<BillOfMaterialItemDto, BillOfMaterialItem>()
                .ForMember(entity => entity.BillOfMaterial, opt => opt.Ignore())
                .ForMember(entity => entity.MaterialCard, opt => opt.Ignore())
                .ForMember(entity => entity.SupplierMaterialCard, opt => opt.Ignore())
                .ForMember(entity => entity.CreateDate, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(entity => entity.UpdateDate, opt => opt.MapFrom(src => src.UpdatedAt));

            // WorkOrder mappings
            CreateMap<WorkOrder, WorkOrderDto>()
                .ForMember(dto => dto.BOMCode, opt => opt.MapFrom(src => src.BillOfMaterial != null ? src.BillOfMaterial.BOMCode : null))
                .ForMember(dto => dto.BOMName, opt => opt.MapFrom(src => src.BillOfMaterial != null ? src.BillOfMaterial.BOMName : null))
                .ForMember(dto => dto.ProductMaterialCardName, opt => opt.MapFrom(src => src.ProductMaterialCard != null ? src.ProductMaterialCard.CardName : null))
                .ForMember(dto => dto.ProductMaterialCardCode, opt => opt.MapFrom(src => src.ProductMaterialCard != null ? src.ProductMaterialCard.CardCode : null))
                .ForMember(dto => dto.SupervisorUserName, opt => opt.MapFrom(src => src.SupervisorUser != null ? src.SupervisorUser.Username : null))
                .ForMember(dto => dto.ReleasedByUserName, opt => opt.MapFrom(src => src.ReleasedByUser != null ? src.ReleasedByUser.Username : null))
                .ForMember(dto => dto.CreatedAt, opt => opt.MapFrom(src => src.CreateDate))
                .ForMember(dto => dto.UpdatedAt, opt => opt.MapFrom(src => src.UpdateDate))
                .ForMember(dto => dto.CreatedByUserId, opt => opt.MapFrom(src => src.CreateUserId))
                .ForMember(dto => dto.UpdatedByUserId, opt => opt.MapFrom(src => src.UpdateUserId))
                .ForMember(dto => dto.CreatedByUserName, opt => opt.Ignore()) // Will be set by service layer
                .ForMember(dto => dto.UpdatedByUserName, opt => opt.Ignore()); // Will be set by service layer

            CreateMap<WorkOrderDto, WorkOrder>()
                .ForMember(entity => entity.BillOfMaterial, opt => opt.Ignore())
                .ForMember(entity => entity.ProductMaterialCard, opt => opt.Ignore())
                .ForMember(entity => entity.SupervisorUser, opt => opt.Ignore())
                .ForMember(entity => entity.ReleasedByUser, opt => opt.Ignore())
                .ForMember(entity => entity.ProductionConfirmations, opt => opt.Ignore())
                .ForMember(entity => entity.CreateDate, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(entity => entity.UpdateDate, opt => opt.MapFrom(src => src.UpdatedAt))
                .ForMember(entity => entity.CreateUserId, opt => opt.MapFrom(src => src.CreatedByUserId))
                .ForMember(entity => entity.UpdateUserId, opt => opt.MapFrom(src => src.UpdatedByUserId));

            // ProductionConfirmation mappings
            CreateMap<ProductionConfirmation, ProductionConfirmationDto>()
                .ForMember(dto => dto.WorkOrderNumber, opt => opt.MapFrom(src => src.WorkOrder != null ? src.WorkOrder.WorkOrderNumber : null))
                .ForMember(dto => dto.ProductMaterialCardName, opt => opt.MapFrom(src => src.WorkOrder != null && src.WorkOrder.ProductMaterialCard != null ? src.WorkOrder.ProductMaterialCard.CardName : null))
                .ForMember(dto => dto.ProductMaterialCardCode, opt => opt.MapFrom(src => src.WorkOrder != null && src.WorkOrder.ProductMaterialCard != null ? src.WorkOrder.ProductMaterialCard.CardCode : null))
                .ForMember(dto => dto.OperatorUserName, opt => opt.MapFrom(src => src.OperatorUser != null ? src.OperatorUser.Username : null))
                .ForMember(dto => dto.ConfirmedByUserName, opt => opt.MapFrom(src => src.ConfirmedByUser != null ? src.ConfirmedByUser.Username : null))
                .ForMember(dto => dto.CreatedAt, opt => opt.MapFrom(src => src.CreateDate))
                .ForMember(dto => dto.UpdatedAt, opt => opt.MapFrom(src => src.UpdateDate))
                .ForMember(dto => dto.CreatedByUserId, opt => opt.MapFrom(src => src.CreateUserId))
                .ForMember(dto => dto.UpdatedByUserId, opt => opt.MapFrom(src => src.UpdateUserId))
                .ForMember(dto => dto.CreatedByUserName, opt => opt.Ignore()) // Will be set by service layer
                .ForMember(dto => dto.UpdatedByUserName, opt => opt.Ignore()); // Will be set by service layer

            CreateMap<ProductionConfirmationDto, ProductionConfirmation>()
                .ForMember(entity => entity.WorkOrder, opt => opt.Ignore())
                .ForMember(entity => entity.OperatorUser, opt => opt.Ignore())
                .ForMember(entity => entity.ConfirmedByUser, opt => opt.Ignore())
                .ForMember(entity => entity.CreateDate, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(entity => entity.UpdateDate, opt => opt.MapFrom(src => src.UpdatedAt))
                .ForMember(entity => entity.CreateUserId, opt => opt.MapFrom(src => src.CreatedByUserId))
                .ForMember(entity => entity.UpdateUserId, opt => opt.MapFrom(src => src.UpdatedByUserId));
        }

        #region Helper Methods

        private static string GetMovementTypeDisplay(string? movementType)
        {
            return movementType?.ToUpper() switch
            {
                "IN" => "Giriş",
                "OUT" => "Çıkış", 
                "TRANSFER" => "Transfer",
                "ADJUSTMENT" => "Sayım",
                "PRODUCTION" => "Üretim",
                "CONSUMPTION" => "Tüketim",
                "RETURN" => "İade",
                _ => movementType ?? "Belirsiz"
            };
        }

        private static string GetStatusDisplay(string? status)
        {
            return status?.ToUpper() switch
            {
                "PENDING" => "Beklemede",
                "CONFIRMED" => "Onaylandı",
                "CANCELLED" => "İptal Edildi", 
                "COMPLETED" => "Tamamlandı",
                _ => status ?? "Belirsiz"
            };
        }

        #endregion
    }
} 