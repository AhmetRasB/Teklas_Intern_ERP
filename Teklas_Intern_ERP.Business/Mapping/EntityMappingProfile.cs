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
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.MaterialCode))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.MaterialName))
                .ReverseMap()
                .ForMember(dest => dest.MaterialCode, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.MaterialName, opt => opt.MapFrom(src => src.Name));
        }
    }
} 