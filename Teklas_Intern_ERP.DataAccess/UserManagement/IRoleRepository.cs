using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Teklas_Intern_ERP.Entities.UserManagement;
using Teklas_Intern_ERP.DataAccess.Repositories;
using Teklas_Intern_ERP.DTOs;

namespace Teklas_Intern_ERP.DataAccess.UserManagement
{
public interface IRoleRepository : IRepository<Role>
{
    // Role Management Operations
    Task<bool> IsRoleNameUniqueAsync(string name, long? excludeId = null);
    Task<List<Role>> GetUserRolesAsync(long userId);
    Task<Role?> GetByNameAsync(string name);
    Task<List<Role>> GetSystemRolesAsync();
    Task<List<Role>> GetByPriorityAsync(int minPriority);
    
    // User-Role Operations
    Task AddUserToRoleAsync(long userId, long roleId);
    Task RemoveUserFromRoleAsync(long userId, long roleId);
    Task<bool> UserHasRoleAsync(long userId, string roleName);
    Task<List<string>> GetUserPermissionsAsync(long userId);
}

} 