using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Teklas_Intern_ERP.Entities.UserManagement;
using Teklas_Intern_ERP.DataAccess.Repositories;

namespace Teklas_Intern_ERP.DataAccess.UserManagement
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) { }

        #region Authentication Operations

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetByUsernameOrEmailAsync(string usernameOrEmail)
        {
            return await _dbSet.FirstOrDefaultAsync(u => 
                u.Username == usernameOrEmail || u.Email == usernameOrEmail);
        }

        #endregion

        #region User Management Operations

        public async Task<bool> IsUsernameUniqueAsync(string username, long? excludeId = null)
        {
            var query = _dbSet.Where(u => u.Username == username);
            if (excludeId.HasValue)
                query = query.Where(u => u.Id != excludeId.Value);

            return !await query.AnyAsync();
        }

        public async Task<bool> IsEmailUniqueAsync(string email, long? excludeId = null)
        {
            var query = _dbSet.Where(u => u.Email == email);
            if (excludeId.HasValue)
                query = query.Where(u => u.Id != excludeId.Value);

            return !await query.AnyAsync();
        }

        public async Task<List<User>> GetUsersByRoleAsync(long roleId)
        {
            return await _context.Set<UserRole>()
                                 .Include(ur => ur.User)
                                 .Where(ur => ur.RoleId == roleId)
                                 .Select(ur => ur.User)
                                 .ToListAsync();
        }

        public async Task<List<User>> SearchUsersAsync(string searchTerm)
        {
            return await _dbSet.Where(u => 
                u.Username.Contains(searchTerm) ||
                u.Email.Contains(searchTerm) ||
                u.FirstName.Contains(searchTerm) ||
                u.LastName.Contains(searchTerm))
                .ToListAsync();
        }

        #endregion

        #region Profile Operations

        public async Task UpdateLastLoginAsync(long userId)
        {
            var user = await _dbSet.FindAsync(userId);
            if (user != null)
            {
                user.LastLoginDate = DateTime.UtcNow;
                _context.Entry(user).Property(u => u.LastLoginDate).IsModified = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdatePasswordAsync(long userId, string passwordHash)
        {
            var user = await _dbSet.FindAsync(userId);
            if (user != null)
            {
                user.PasswordHash = passwordHash;
                _context.Entry(user).Property(u => u.PasswordHash).IsModified = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<User?> GetUserWithRolesAsync(long userId)
        {
            return await _dbSet.Include(u => u.UserRoles)
                               .ThenInclude(ur => ur.Role)
                               .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<List<User>> GetAllWithRolesAsync()
        {
            return await _dbSet.Include(u => u.UserRoles)
                               .ThenInclude(ur => ur.Role)
                               .Where(u => !u.IsDeleted)
                               .ToListAsync();
        }

        #endregion
    }
} 