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
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region Basic CRUD Operations

        public async Task<List<UserDto>> GetAllAsync()
        {
            var entities = await _userRepository.GetAllAsync();
            return _mapper.Map<List<UserDto>>(entities);
        }

        public async Task<UserDto?> GetByIdAsync(long id)
        {
            var entity = await _userRepository.GetByIdAsync(id);
            return entity != null ? _mapper.Map<UserDto>(entity) : null;
        }

        public async Task<UserDto> AddAsync(UserDto dto)
        {
            // VALIDATION
            var validator = new UserDtoValidator();
            var validationResult = validator.Validate(dto);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                // Check username uniqueness
                if (!await _userRepository.IsUsernameUniqueAsync(dto.Username!, dto.Id == 0 ? null : dto.Id))
                    throw new InvalidOperationException("Bu kullanıcı adı zaten kullanılmaktadır.");

                // Check email uniqueness
                if (!await _userRepository.IsEmailUniqueAsync(dto.Email!, dto.Id == 0 ? null : dto.Id))
                    throw new InvalidOperationException("Bu e-posta adresi zaten kullanılmaktadır.");

                var entity = _mapper.Map<User>(dto);
                entity.Status = Entities.StatusType.Active;
                entity.CreateUserId = 1; // TODO: Get from current user context
                entity.UpdateUserId = 1;

                var addedEntity = await _userRepository.AddAsync(entity);
                await _unitOfWork.CommitTransactionAsync();

                return _mapper.Map<UserDto>(addedEntity);
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<UserDto> UpdateAsync(UserDto dto)
        {
            // VALIDATION
            var validator = new UserDtoValidator();
            var validationResult = validator.Validate(dto);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                // Check username uniqueness
                if (!await _userRepository.IsUsernameUniqueAsync(dto.Username!, dto.Id))
                    throw new InvalidOperationException("Bu kullanıcı adı zaten kullanılmaktadır.");

                // Check email uniqueness
                if (!await _userRepository.IsEmailUniqueAsync(dto.Email!, dto.Id))
                    throw new InvalidOperationException("Bu e-posta adresi zaten kullanılmaktadır.");

                var entity = _mapper.Map<User>(dto);
                await _userRepository.UpdateAsync(entity);
                await _unitOfWork.CommitTransactionAsync();

                return _mapper.Map<UserDto>(entity);
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<bool> DeleteAsync(long id)
        {
            return await _userRepository.DeleteAsync(id);
        }

        public async Task<List<UserDto>> GetDeletedAsync()
        {
            var entities = await _userRepository.GetDeletedAsync();
            return _mapper.Map<List<UserDto>>(entities);
        }

        public async Task<bool> RestoreAsync(long id)
        {
            return await _userRepository.RestoreAsync(id);
        }

        public async Task<bool> PermanentDeleteAsync(long id)
        {
            return await _userRepository.PermanentDeleteAsync(id);
        }

        #endregion

        #region User Management Operations

        public async Task<UserDto?> GetByUsernameAsync(string username)
        {
            var entity = await _userRepository.GetByUsernameAsync(username);
            return entity != null ? _mapper.Map<UserDto>(entity) : null;
        }

        public async Task<UserDto?> GetByEmailAsync(string email)
        {
            var entity = await _userRepository.GetByEmailAsync(email);
            return entity != null ? _mapper.Map<UserDto>(entity) : null;
        }

        public async Task<List<UserDto>> GetUsersByRoleAsync(long roleId)
        {
            var entities = await _userRepository.GetUsersByRoleAsync(roleId);
            return _mapper.Map<List<UserDto>>(entities);
        }

        public async Task<List<UserDto>> SearchUsersAsync(string searchTerm)
        {
            var entities = await _userRepository.SearchUsersAsync(searchTerm);
            return _mapper.Map<List<UserDto>>(entities);
        }

        public async Task<bool> IsUsernameUniqueAsync(string username, long? excludeId = null)
        {
            return await _userRepository.IsUsernameUniqueAsync(username, excludeId);
        }

        public async Task<bool> IsEmailUniqueAsync(string email, long? excludeId = null)
        {
            return await _userRepository.IsEmailUniqueAsync(email, excludeId);
        }

        #endregion

        #region User Role Operations

        public async Task<bool> AssignRoleToUserAsync(long userId, long roleId)
        {
            try
            {
                await _roleRepository.AddUserToRoleAsync(userId, roleId);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RemoveRoleFromUserAsync(long userId, long roleId)
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

        public async Task<List<RoleDto>> GetUserRolesAsync(long userId)
        {
            var entities = await _roleRepository.GetUserRolesAsync(userId);
            return _mapper.Map<List<RoleDto>>(entities);
        }

        public async Task<UserDto?> GetUserWithRolesAsync(long userId)
        {
            var entity = await _userRepository.GetUserWithRolesAsync(userId);
            return entity != null ? _mapper.Map<UserDto>(entity) : null;
        }

        #endregion

        #region Controller Specific Methods

        public async Task<List<UserDto>> GetAllUsersAsync(string? search = null, bool? isActive = null, long? roleId = null, int page = 1, int pageSize = 10)
        {
            // Get users with roles included
            var users = await _userRepository.GetAllWithRolesAsync();
            var userDtos = _mapper.Map<List<UserDto>>(users);
            
            // Apply pagination
            return userDtos.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }

        public async Task<int> GetUserCountAsync(string? search = null, bool? isActive = null, long? roleId = null)
        {
            var users = await _userRepository.GetAllWithRolesAsync();
            return users.Count;
        }

        public async Task<UserDto?> GetUserByIdAsync(long id)
        {
            var user = await _userRepository.GetUserWithRolesAsync(id);
            return user != null ? _mapper.Map<UserDto>(user) : null;
        }

        public async Task<UserDto> CreateUserAsync(object createUserDto)
        {
            // Basic implementation - can be enhanced with proper DTO conversion
            var dto = new UserDto(); // Convert from object later
            return await AddAsync(dto);
        }

        public async Task<UserDto?> UpdateUserAsync(long id, object updateUserDto)
        {
            // Basic implementation - can be enhanced with proper DTO conversion
            var dto = new UserDto { Id = id }; // Convert from object later
            return await UpdateAsync(dto);
        }

        public async Task<bool> DeleteUserAsync(long id)
        {
            return await DeleteAsync(id);
        }

        public async Task<bool> ActivateUserAsync(long id)
        {
            // Implementation needed
            return true;
        }

        public async Task<bool> DeactivateUserAsync(long id)
        {
            // Implementation needed
            return true;
        }

        #endregion
    }
}
