using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Teklas_Intern_ERP.Entities.UserManagement;
using Teklas_Intern_ERP.DataAccess.Repositories;

namespace Teklas_Intern_ERP.DataAccess.UserManagement
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(AppDbContext context) : base(context) { }

        #region Role Management Operations

        public async Task<bool> IsRoleNameUniqueAsync(string name, long? excludeId = null)
        {
            var query = _dbSet.Where(r => r.Name == name);
            if (excludeId.HasValue)
                query = query.Where(r => r.Id != excludeId.Value);

            return !await query.AnyAsync();
        }

        public async Task<List<Role>> GetUserRolesAsync(long userId)
        {
            return await _context.Set<UserRole>()
                                 .Include(ur => ur.Role)
                                 .Where(ur => ur.UserId == userId)
                                 .Select(ur => ur.Role)
                                 .ToListAsync();
        }

        public async Task<Role?> GetByNameAsync(string name)
        {
            return await _dbSet.FirstOrDefaultAsync(r => r.Name == name);
        }

        public async Task<List<Role>> GetSystemRolesAsync()
        {
            return await _dbSet.Where(r => r.IsSystemRole == true).ToListAsync();
        }

        public async Task<List<Role>> GetByPriorityAsync(int minPriority)
        {
            return await _dbSet.Where(r => r.Priority >= minPriority)
                               .OrderByDescending(r => r.Priority)
                               .ToListAsync();
        }

        #endregion

        #region User-Role Operations

        public async Task AddUserToRoleAsync(long userId, long roleId)
        {
            // Check if relationship already exists
            var existingUserRole = await _context.Set<UserRole>()
                .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);

            if (existingUserRole == null)
            {
                var userRole = new UserRole
                {
                    UserId = userId,
                    RoleId = roleId,
                    AssignedDate = DateTime.UtcNow,
                    CreateUserId = 1, // TODO: Get from current user context
                    Status = Entities.StatusType.Active
                };

                await _context.Set<UserRole>().AddAsync(userRole);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveUserFromRoleAsync(long userId, long roleId)
        {
            var userRole = await _context.Set<UserRole>()
                .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);

            if (userRole != null)
            {
                // Soft delete
                userRole.IsDeleted = true;
                userRole.DeleteDate = DateTime.UtcNow;
                userRole.DeleteUserId = 1; // TODO: Get from current user context
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> UserHasRoleAsync(long userId, string roleName)
        {
            return await _context.Set<UserRole>()
                .Include(ur => ur.Role)
                .AnyAsync(ur => ur.UserId == userId && 
                               ur.Role.Name == roleName && 
                               !ur.IsDeleted);
        }

        public async Task<List<string>> GetUserPermissionsAsync(long userId)
        {
            var userRoles = await _context.Set<UserRole>()
                .Include(ur => ur.Role)
                .Where(ur => ur.UserId == userId && !ur.IsDeleted)
                .Select(ur => ur.Role)
                .ToListAsync();

            var permissions = new List<string>();
            foreach (var role in userRoles)
            {
                if (!string.IsNullOrEmpty(role.Permissions))
                {
                    // Parse JSON permissions (you can use System.Text.Json here)
                    // For now, simple split by comma
                    var rolePermissions = role.Permissions.Split(',')
                        .Select(p => p.Trim())
                        .Where(p => !string.IsNullOrEmpty(p));
                    
                    permissions.AddRange(rolePermissions);
                }
            }

            return permissions.Distinct().ToList();
        }

        #endregion
    }
} 