using System.Threading.Tasks;
using AutoMapper;
using Teklas_Intern_ERP.Business.Interfaces;
using Teklas_Intern_ERP.DataAccess.UserManagement;
using Teklas_Intern_ERP.DTOs.UserManagement;
using Teklas_Intern_ERP.Entities.UserManagement;

namespace Teklas_Intern_ERP.Business.UserManagement
{
    public class UserTableColumnPreferenceService : IUserTableColumnPreferenceService
    {
        private readonly UserTableColumnPreferenceRepository _repository;
        private readonly IMapper _mapper;

        public UserTableColumnPreferenceService(UserTableColumnPreferenceRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<UserTableColumnPreferenceDto?> GetPreferenceAsync(int userId, string tableKey)
        {
            var entity = await _repository.GetByUserAndTableAsync(userId, tableKey);
            return entity != null ? _mapper.Map<UserTableColumnPreferenceDto>(entity) : null;
        }

        public async Task<bool> SetPreferenceAsync(UserTableColumnPreferenceDto dto)
        {
            var entity = _mapper.Map<UserTableColumnPreference>(dto);
            entity.UpdatedAt = System.DateTime.UtcNow;
            if (entity.Id == 0)
                entity.CreatedAt = entity.UpdatedAt;
            return await _repository.SetPreferenceAsync(entity);
        }
    }
} 