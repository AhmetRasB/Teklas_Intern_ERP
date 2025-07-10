using System.Collections.Generic;
using System.Threading.Tasks;
using Teklas_Intern_ERP.DTOs;

namespace Teklas_Intern_ERP.Business.Interfaces
{
    public interface IRoleService
    {
        // Basic CRUD Operations
        Task<List<RoleDto>> GetAllAsync();
        Task<RoleDto?> GetByIdAsync(long id);
        Task<RoleDto> AddAsync(RoleDto dto);
        Task<RoleDto> UpdateAsync(RoleDto dto);
        Task<bool> DeleteAsync(long id);
        Task<List<RoleDto>> GetDeletedAsync();
        Task<bool> RestoreAsync(long id);
        Task<bool> PermanentDeleteAsync(long id);
        
        // Controller specific methods
        Task<List<RoleDto>> GetAllRolesAsync(string? search = null, bool? isActive = null, int page = 1, int pageSize = 10);
        Task<int> GetRoleCountAsync(string? search = null, bool? isActive = null);
        Task<RoleDto?> GetRoleByIdAsync(long id);
        Task<RoleDto> CreateRoleAsync(object createRoleDto);
        Task<RoleDto?> UpdateRoleAsync(long id, object updateRoleDto);
        Task<bool> DeleteRoleAsync(long id);
        Task<List<UserDto>> GetRoleUsersAsync(long id);
        Task<List<RoleDto>> SearchRolesAsync(string searchTerm);
        Task<List<RoleDto>> GetAssignableRolesAsync();
        Task<bool> ActivateRoleAsync(long id);
        Task<bool> DeactivateRoleAsync(long id);
        
        // Role Management Operations
        Task<RoleDto?> GetByNameAsync(string name);
        Task<List<RoleDto>> GetSystemRolesAsync();
        Task<List<RoleDto>> GetByPriorityAsync(int minPriority);
        Task<bool> IsRoleNameUniqueAsync(string name, long? excludeId = null);
        
        // User Role Operations
        Task<List<UserDto>> GetUsersInRoleAsync(long roleId);
        Task<bool> AddUserToRoleAsync(long userId, long roleId);
        Task<bool> RemoveUserFromRoleAsync(long userId, long roleId);
        Task<bool> UserHasRoleAsync(long userId, string roleName);
        Task<List<string>> GetUserPermissionsAsync(long userId);
    }
} 