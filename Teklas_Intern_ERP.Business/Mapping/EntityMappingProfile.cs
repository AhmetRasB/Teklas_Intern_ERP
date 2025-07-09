using AutoMapper;
using Teklas_Intern_ERP.Entities.MaterialManagement;
using Teklas_Intern_ERP.DTOs;

namespace Teklas_Intern_ERP.Business.Mapping
{
    public class EntityMappingProfile : Profile
    {
        public EntityMappingProfile()
        {
            CreateMap<MaterialCard, MaterialCardDto>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ReverseMap()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Category, opt => opt.Ignore());

            CreateMap<MaterialCategory, MaterialCategoryDto>()
                .ForMember(dest => dest.ParentCategoryName, opt => opt.MapFrom(src => src.ParentCategory != null ? src.ParentCategory.Name : null))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (int)src.Status))
                .ReverseMap()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (Teklas_Intern_ERP.Entities.StatusType)src.Status))
                .ForMember(dest => dest.ParentCategory, opt => opt.Ignore());
        }
    }
} 