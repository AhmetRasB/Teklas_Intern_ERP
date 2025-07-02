using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teklas_Intern_ERP.Entities.MaterialManagement;
using Teklas_Intern_ERP.DataAccess.DTOs;

namespace Teklas_Intern_ERP.DataAccess.Mapping
{
    public class EntityMappingProfile : Profile
    {
        public EntityMappingProfile()
        {
            CreateMap<MaterialCard, MaterialCardDto>().ReverseMap();
            CreateMap<MaterialCard, MaterialCard>().ForMember(dest => dest.Id, opt => opt.Ignore()).ForMember(dest => dest.CreatedDate, opt => opt.Ignore()).ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<MaterialBatch, MaterialBatchDto>().ReverseMap();
            CreateMap<MaterialCategory, MaterialCategoryDto>().ReverseMap();
            CreateMap<MaterialMovement, MaterialMovementDto>().ReverseMap();
        }
    }
}
