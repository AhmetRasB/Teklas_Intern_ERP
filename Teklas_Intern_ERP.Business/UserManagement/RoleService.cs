using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Teklas_Intern_ERP.Business.Interfaces;
using Teklas_Intern_ERP.DataAccess.UserManagement;
using Teklas_Intern_ERP.DataAccess.Repositories;
using Teklas_Intern_ERP.DTOs;
using Teklas_Intern_ERP.Entities.UserManagement;
using ValidationException = FluentValidation.ValidationException;

namespace Teklas_Intern_ERP.Business.UserManagement
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RoleService(
            IRoleRepository roleRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region Basic CRUD Operations

        public async Task<List<RoleDto>> GetAllAsync()
        {
            var entities = await _roleRepository.GetAllAsync();
            return _mapper.Map<List<RoleDto>>(entities);
        }

        public async Task<RoleDto?> GetByIdAsync(long id)
        {
            var entity = await _roleRepository.GetByIdAsync(id);
            return entity != null ? _mapper.Map<RoleDto>(entity) : null;
        }

        public async Task<RoleDto> AddAsync(RoleDto dto)
        {
            // VALIDATION
            var validator = new RoleDtoValidator();
            var validationResult = validator.Validate(dto);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                // Check role name uniqueness
                if (!await _roleRepository.IsRoleNameUniqueAsync(dto.Name!, dto.Id == 0 ? null : dto.Id))
                    throw new InvalidOperationException("Bu rol adı zaten kullanılmaktadır.");

                var entity = _mapper.Map<Role>(dto);
                entity.Status = Entities.StatusType.Active;
                entity.CreateUserId = 1; // TODO: Get from current user context
                entity.UpdateUserId = 1;

                var addedEntity = await _roleRepository.AddAsync(entity);
                await _unitOfWork.CommitTransactionAsync();

                return _mapper.Map<RoleDto>(addedEntity);
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<RoleDto> UpdateAsync(RoleDto dto)
        {
            // VALIDATION
            var validator = new RoleDtoValidator();
            var validationResult = validator.Validate(dto);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                // Check role name uniqueness
                if (!await _roleRepository.IsRoleNameUniqueAsync(dto.Name!, dto.Id))
                    throw new InvalidOperationException("Bu rol adı zaten kullanılmaktadır.");

                // Check if it's a system role
                var existingRole = await _roleRepository.GetByIdAsync(dto.Id);
                if (existingRole?.IsSystemRole == true)
                    throw new InvalidOperationException("Sistem rolleri düzenlenemez.");

                var entity = _mapper.Map<Role>(dto);
                await _roleRepository.UpdateAsync(entity);
                await _unitOfWork.CommitTransactionAsync();

                return _mapper.Map<RoleDto>(entity);
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<bool> DeleteAsync(long id)
        {
            // Check if it's a system role
            var role = await _roleRepository.GetByIdAsync(id);
            if (role?.IsSystemRole == true)
                throw new InvalidOperationException("Sistem rolleri silinemez.");

            // Check if role has users
            var usersInRole = await _userRepository.GetUsersByRoleAsync(id);
            if (usersInRole.Any())
                throw new InvalidOperationException("Bu role atanmış kullanıcılar bulunmaktadır. Önce kullanıcıları başka rolle değiştirin.");

            return await _roleRepository.DeleteAsync(id);
        }

        public async Task<List<RoleDto>> GetDeletedAsync()
        {
            var entities = await _roleRepository.GetDeletedAsync();
            return _mapper.Map<List<RoleDto>>(entities);
        }

        public async Task<bool> RestoreAsync(long id)
        {
            return await _roleRepository.RestoreAsync(id);
        }

        public async Task<bool> PermanentDeleteAsync(long id)
        {
            // Check if it's a system role
            var role = await _roleRepository.GetByIdIncludeDeletedAsync(id);
            if (role?.IsSystemRole == true)
                throw new InvalidOperationException("Sistem rolleri kalıcı olarak silinemez.");

            return await _roleRepository.PermanentDeleteAsync(id);
        }

        #endregion

        #region Role Management Operations

        public async Task<RoleDto?> GetByNameAsync(string name)
        {
            var entity = await _roleRepository.GetByNameAsync(name);
            return entity != null ? _mapper.Map<RoleDto>(entity) : null;
        }

        public async Task<List<RoleDto>> GetSystemRolesAsync()
        {
            var entities = await _roleRepository.GetSystemRolesAsync();
            return _mapper.Map<List<RoleDto>>(entities);
        }

        public async Task<List<RoleDto>> GetByPriorityAsync(int minPriority)
        {
            var entities = await _roleRepository.GetByPriorityAsync(minPriority);
            return _mapper.Map<List<RoleDto>>(entities);
        }

        public async Task<bool> IsRoleNameUniqueAsync(string name, long? excludeId = null)
        {
            return await _roleRepository.IsRoleNameUniqueAsync(name, excludeId);
        }

        #endregion

        #region User Role Operations

        public async Task<List<UserDto>> GetUsersInRoleAsync(long roleId)
        {
            var entities = await _userRepository.GetUsersByRoleAsync(roleId);
            return _mapper.Map<List<UserDto>>(entities);
        }

        public async Task<bool> AddUserToRoleAsync(long userId, long roleId)
        {
            try
            {
                // Check if user exists
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null) return false;

                // Check if role exists
                var role = await _roleRepository.GetByIdAsync(roleId);
                if (role == null) return false;

                await _roleRepository.AddUserToRoleAsync(userId, roleId);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RemoveUserFromRoleAsync(long userId, long roleId)
        {
            try
            {
                await _roleRepository.RemoveUserFromRoleAsync(userId, roleId);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UserHasRoleAsync(long userId, string roleName)
        {
            return await _roleRepository.UserHasRoleAsync(userId, roleName);
        }

        public async Task<List<string>> GetUserPermissionsAsync(long userId)
        {
            return await _roleRepository.GetUserPermissionsAsync(userId);
        }

        #endregion

        #region Controller Specific Methods

        public async Task<List<RoleDto>> GetAllRolesAsync(string? search = null, bool? isActive = null, int page = 1, int pageSize = 10)
        {
            // Simple implementation - can be enhanced later
            var roles = await GetAllAsync();
            return roles.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }

        public async Task<int> GetRoleCountAsync(string? search = null, bool? isActive = null)
        {
            var roles = await GetAllAsync();
            return roles.Count;
        }

        public async Task<RoleDto?> GetRoleByIdAsync(long id)
        {
            return await GetByIdAsync(id);
        }

        public async Task<RoleDto> CreateRoleAsync(object createRoleDto)
        {
            // Convert dynamic object to RoleDto for proper handling
            var json = System.Text.Json.JsonSerializer.Serialize(createRoleDto);
            var createDto = System.Text.Json.JsonSerializer.Deserialize<System.Text.Json.JsonElement>(json);
            
            // Validate required fields
            if (!createDto.TryGetProperty("name", out var nameElement) || string.IsNullOrWhiteSpace(nameElement.GetString()))
            {
                throw new ValidationException("Role name is required");
            }
            
            var name = nameElement.GetString()!;
            
            // Check if role name already exists
            var existingRole = await GetByNameAsync(name);
            if (existingRole != null)
            {
                throw new InvalidOperationException($"Bu isimde bir rol zaten mevcut: {name}");
            }
            
            // Create new role DTO
            var dto = new RoleDto
            {
                Name = name,
                Description = createDto.TryGetProperty("description", out var descElement) ? descElement.GetString() : null,
                IsSystemRole = false,
                Priority = 0,
                Status = createDto.TryGetProperty("isActive", out var activeElement) && activeElement.GetBoolean() ? 1 : 0,
                CreateDate = DateTime.UtcNow
            };
            
            return await AddAsync(dto);
        }

        public async Task<RoleDto?> UpdateRoleAsync(long id, object updateRoleDto)
        {
            // Basic implementation - can be enhanced with proper DTO conversion
            var dto = new RoleDto { Id = id }; // Convert from object later
            return await UpdateAsync(dto);
        }

        public async Task<bool> DeleteRoleAsync(long id)
        {
            return await DeleteAsync(id);
        }

        public async Task<List<UserDto>> GetRoleUsersAsync(long id)
        {
            return await GetUsersInRoleAsync(id);
        }

        public async Task<List<RoleDto>> SearchRolesAsync(string searchTerm)
        {
            // Basic implementation - can be enhanced later
            var roles = await GetAllAsync();
            return roles.Where(r => r.Name != null && r.Name.Contains(searchTerm)).ToList();
        }

        public async Task<List<RoleDto>> GetAssignableRolesAsync()
        {
            // Basic implementation - non-system roles
            var roles = await GetAllAsync();
            return roles.Where(r => !r.IsSystemRole).ToList();
        }

        public async Task<bool> ActivateRoleAsync(long id)
        {
            // Implementation needed
            return true;
        }

        public async Task<bool> DeactivateRoleAsync(long id)
        {
            // Implementation needed
            return true;
        }

        #endregion
    }
}
