using System.Collections.Generic;
using System.Threading.Tasks;
using Teklas_Intern_ERP.DTOs;

namespace Teklas_Intern_ERP.Business.Interfaces
{
    public interface IUserService
    {
        // Basic CRUD Operations
        Task<List<UserDto>> GetAllAsync();
        Task<UserDto?> GetByIdAsync(long id);
        Task<UserDto> AddAsync(UserDto dto);
        Task<UserDto> UpdateAsync(UserDto dto);
        Task<bool> DeleteAsync(long id);
        Task<List<UserDto>> GetDeletedAsync();
        Task<bool> RestoreAsync(long id);
        Task<bool> PermanentDeleteAsync(long id);
        
        // Controller specific methods
        Task<List<UserDto>> GetAllUsersAsync(string? search = null, bool? isActive = null, long? roleId = null, int page = 1, int pageSize = 10);
        Task<int> GetUserCountAsync(string? search = null, bool? isActive = null, long? roleId = null);
        Task<UserDto?> GetUserByIdAsync(long id);
        Task<UserDto> CreateUserAsync(object createUserDto);
        Task<UserDto?> UpdateUserAsync(long id, object updateUserDto);
        Task<bool> DeleteUserAsync(long id);
        Task<bool> ActivateUserAsync(long id);
        Task<bool> DeactivateUserAsync(long id);
        
        // User Management Operations
        Task<UserDto?> GetByUsernameAsync(string username);
        Task<UserDto?> GetByEmailAsync(string email);
        Task<List<UserDto>> GetUsersByRoleAsync(long roleId);
        Task<List<UserDto>> SearchUsersAsync(string searchTerm);
        Task<bool> IsUsernameUniqueAsync(string username, long? excludeId = null);
        Task<bool> IsEmailUniqueAsync(string email, long? excludeId = null);
        
        // User Role Operations
        Task<bool> AssignRoleToUserAsync(long userId, long roleId);
        Task<bool> RemoveRoleFromUserAsync(long userId, long roleId);
        Task<List<RoleDto>> GetUserRolesAsync(long userId);
        Task<UserDto?> GetUserWithRolesAsync(long userId);
    }
} 