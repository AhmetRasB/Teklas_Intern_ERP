using AutoMapper;
using Teklas_Intern_ERP.Entities.MaterialManagement;
using Teklas_Intern_ERP.Entities.UserManagement;
using Teklas_Intern_ERP.Entities.WarehouseManagement;
using Teklas_Intern_ERP.Entities.PurchasingManagement;
using Teklas_Intern_ERP.Entities.SalesManagement;
using Teklas_Intern_ERP.DTOs;
using Teklas_Intern_ERP.DTOs.WarehouseManagement;
using Teklas_Intern_ERP.DTOs.PurchasingManagement;
using Teklas_Intern_ERP.DTOs.SalesManagement;
using Teklas_Intern_ERP.Entities.ProductionManagment;
using EntityMaterialConsumption = Teklas_Intern_ERP.Entities.ProductionManagment.MaterialConsumption;
using DtoMaterialConsumption = Teklas_Intern_ERP.DTOs.MaterialConsumptionDto;
using EntityBOMHeader = Teklas_Intern_ERP.Entities.ProductionManagment.BOMHeader;
using EntityBOMItem = Teklas_Intern_ERP.Entities.ProductionManagment.BOMItem;
using EntityWorkOrder = Teklas_Intern_ERP.Entities.ProductionManagment.WorkOrder;
using EntityWorkOrderOperation = Teklas_Intern_ERP.Entities.ProductionManagment.WorkOrderOperation;
using EntityProductionConfirmation = Teklas_Intern_ERP.Entities.ProductionManagment.ProductionConfirmation;

namespace Teklas_Intern_ERP.Business.Mapping
{
    public class EntityMappingProfile : Profile
    {
        public EntityMappingProfile()
        {
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

            // Production Management Mappings
            CreateMap<EntityBOMHeader, BOMHeaderDto>()
                .ForMember(dest => dest.ParentMaterialCardName, opt => opt.MapFrom(src => src.ParentMaterialCard.CardName))
                .ForMember(dest => dest.BOMItems, opt => opt.MapFrom(src => src.BOMItems))
                .ReverseMap();
            CreateMap<CreateBOMHeaderDto, EntityBOMHeader>()
                .ForMember(dest => dest.Version, opt => opt.MapFrom(src => src.Version ?? "1.0"))
                .ReverseMap();
            CreateMap<UpdateBOMHeaderDto, EntityBOMHeader>().ReverseMap();

            CreateMap<EntityBOMItem, BOMItemDto>()
                .ForMember(dest => dest.ComponentMaterialCardName, opt => opt.MapFrom(src => src.ComponentMaterialCard.CardName))
                .ReverseMap();
            CreateMap<CreateBOMItemDto, EntityBOMItem>().ReverseMap();
            CreateMap<UpdateBOMItemDto, EntityBOMItem>().ReverseMap();

            CreateMap<EntityWorkOrder, WorkOrderDto>()
                .ForMember(dest => dest.BOMName, opt => opt.MapFrom(src => src.BOMHeader.Version))
                .ForMember(dest => dest.MaterialCardName, opt => opt.MapFrom(src => src.MaterialCard.CardName))
                .ForMember(dest => dest.Operations, opt => opt.MapFrom(src => src.Operations))
                .ReverseMap();
            CreateMap<CreateWorkOrderDto, EntityWorkOrder>().ReverseMap();
            CreateMap<UpdateWorkOrderDto, EntityWorkOrder>().ReverseMap();

            CreateMap<EntityWorkOrderOperation, WorkOrderOperationDto>().ReverseMap();
            CreateMap<CreateWorkOrderOperationDto, EntityWorkOrderOperation>().ReverseMap();
            CreateMap<UpdateWorkOrderOperationDto, EntityWorkOrderOperation>().ReverseMap();

            CreateMap<EntityProductionConfirmation, ProductionConfirmationDto>()
                .ForMember(dest => dest.Consumptions, opt => opt.MapFrom(src => src.Consumptions))
                .ReverseMap();
            CreateMap<CreateProductionConfirmationDto, EntityProductionConfirmation>().ReverseMap();
            CreateMap<UpdateProductionConfirmationDto, EntityProductionConfirmation>().ReverseMap();

            CreateMap<EntityMaterialConsumption, DtoMaterialConsumption>()
                .ForMember(dest => dest.MaterialCardName, opt => opt.MapFrom(src => src.MaterialCard.CardName))
                .ReverseMap();
            CreateMap<CreateMaterialConsumptionDto, EntityMaterialConsumption>().ReverseMap();
            CreateMap<UpdateMaterialConsumptionDto, EntityMaterialConsumption>().ReverseMap();

            // Warehouse Management Mappings
            CreateMap<Warehouse, WarehouseDto>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.WarehouseCode))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.WarehouseName))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.Status == "Active"))
                .ReverseMap()
                .ForMember(dest => dest.WarehouseCode, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.WarehouseName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.IsActive ? "Active" : "Inactive"))
                .ForMember(dest => dest.Locations, opt => opt.Ignore())
                .ForMember(dest => dest.StockEntries, opt => opt.Ignore());

            CreateMap<Location, LocationDto>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.LocationCode))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.LocationName))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.Status == "Active"))
                .ForMember(dest => dest.WarehouseName, opt => opt.MapFrom(src => src.Warehouse != null ? src.Warehouse.WarehouseName : null))
                .ReverseMap()
                .ForMember(dest => dest.LocationCode, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.LocationName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.IsActive ? "Active" : "Inactive"))
                .ForMember(dest => dest.Warehouse, opt => opt.Ignore())
                .ForMember(dest => dest.StockEntries, opt => opt.Ignore());

            CreateMap<StockEntry, StockEntryDto>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.Status == "Active"))
                .ForMember(dest => dest.WarehouseName, opt => opt.MapFrom(src => src.Warehouse != null ? src.Warehouse.WarehouseName : null))
                .ForMember(dest => dest.LocationName, opt => opt.MapFrom(src => src.Location != null ? src.Location.LocationName : null))
                .ForMember(dest => dest.MaterialName, opt => opt.MapFrom(src => src.Material != null ? src.Material.CardName : null))
                .ForMember(dest => dest.MaterialCode, opt => opt.MapFrom(src => src.Material != null ? src.Material.CardCode : null))
                .ReverseMap()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.IsActive ? "Active" : "Inactive"))
                .ForMember(dest => dest.Warehouse, opt => opt.Ignore())
                .ForMember(dest => dest.Location, opt => opt.Ignore())
                .ForMember(dest => dest.Material, opt => opt.Ignore());

            // Purchasing Management Mappings
            CreateMap<SupplierType, SupplierTypeDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ReverseMap();

            CreateMap<Supplier, SupplierDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.TaxNumber, opt => opt.MapFrom(src => src.TaxNumber))
                .ForMember(dest => dest.ContactPerson, opt => opt.MapFrom(src => src.ContactPerson))
                .ForMember(dest => dest.SupplierTypeId, opt => opt.MapFrom(src => src.SupplierTypeId))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.SupplierTypeName, opt => opt.MapFrom(src => src.SupplierType != null ? src.SupplierType.Name : null))
                .ReverseMap()
                .ForMember(dest => dest.SupplierType, opt => opt.Ignore());

            CreateMap<PurchaseOrder, PurchaseOrderDto>()
                .ForMember(dest => dest.OrderNumber, opt => opt.MapFrom(src => src.OrderNumber))
                .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.OrderDate))
                .ForMember(dest => dest.ExpectedDeliveryDate, opt => opt.MapFrom(src => src.ExpectedDeliveryDate))
                .ForMember(dest => dest.SupplierId, opt => opt.MapFrom(src => src.SupplierId))
                .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreateDate))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => src.UpdateDate))
                .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Supplier != null ? src.Supplier.Name : null))
                .ReverseMap()
                .ForMember(dest => dest.Supplier, opt => opt.Ignore());

            // Sales Management Mappings
            CreateMap<Customer, CustomerDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.TaxNumber, opt => opt.MapFrom(src => src.TaxNumber))
                .ForMember(dest => dest.ContactPerson, opt => opt.MapFrom(src => src.ContactPerson))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ReverseMap();

            CreateMap<CustomerOrder, CustomerOrderDto>()
                .ForMember(dest => dest.OrderNumber, opt => opt.MapFrom(src => src.OrderNumber))
                .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.OrderDate))
                .ForMember(dest => dest.ExpectedDeliveryDate, opt => opt.MapFrom(src => src.ExpectedDeliveryDate))
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
                .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreateDate))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => src.UpdateDate))
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer != null ? src.Customer.Name : null))
                .ReverseMap()
                .ForMember(dest => dest.Customer, opt => opt.Ignore());
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