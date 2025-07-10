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
    public interface IUserRepository : IRepository<User>
{
    // Authentication Operations
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByUsernameOrEmailAsync(string usernameOrEmail);
    
    // User Management Operations
    Task<bool> IsUsernameUniqueAsync(string username, long? excludeId = null);
    Task<bool> IsEmailUniqueAsync(string email, long? excludeId = null);
    Task<List<User>> GetUsersByRoleAsync(long roleId);
    Task<List<User>> SearchUsersAsync(string searchTerm);
    
    // Profile Operations
    Task UpdateLastLoginAsync(long userId);
    Task UpdatePasswordAsync(long userId, string passwordHash);
    Task<User?> GetUserWithRolesAsync(long userId);
    Task<List<User>> GetAllWithRolesAsync();
}} 