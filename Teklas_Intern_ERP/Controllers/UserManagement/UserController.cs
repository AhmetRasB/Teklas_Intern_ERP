using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Teklas_Intern_ERP.Business.Interfaces;
using Teklas_Intern_ERP.DTOs;
using System;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using ValidationException = FluentValidation.ValidationException;

namespace Teklas_Intern_ERP.Controllers.UserManagement
{
    /// <summary>
    /// User Management API - Enterprise Resource Planning
    /// User CRUD operations and role assignments
    /// </summary>
    [ApiController]
    [Route("api/users")]
    [ApiExplorerSettings(GroupName = "User Management")]
    [Authorize]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Get all users with filtering and pagination
        /// </summary>
        /// <param name="search">Search term</param>
        /// <param name="isActive">Active status filter</param>
        /// <param name="roleId">Role filter</param>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>List of users</returns>
        [HttpGet]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllUsers(
            [FromQuery] string? search = null,
            [FromQuery] bool? isActive = null,
            [FromQuery] long? roleId = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var users = await _userService.GetAllUsersAsync(search, isActive, roleId, page, pageSize);
                var totalCount = await _userService.GetUserCountAsync(search, isActive, roleId);

                return Ok(new
                {
                    success = true,
                    data = users,
                    pagination = new
                    {
                        currentPage = page,
                        pageSize = pageSize,
                        totalCount = totalCount,
                        totalPages = (int)Math.Ceiling((double)totalCount / pageSize)
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Get user by ID
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>User details</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUser(long id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound(new { error = "Kullanıcı bulunamadı." });
                }

                return Ok(new { success = true, data = user });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Create new user
        /// </summary>
        /// <param name="createUserDto">User creation data</param>
        /// <returns>Created user</returns>
        [HttpPost]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto createUserDto)
        {
            try
            {
                var user = await _userService.CreateUserAsync(createUserDto);
                return StatusCode(201, new
                {
                    success = true,
                    message = "Kullanıcı başarıyla oluşturuldu.",
                    data = user
                });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { error = "Validation failed", details = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = "Business rule violation", details = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Update user
        /// </summary>
        /// <param name="id">User ID</param>
        /// <param name="updateUserDto">User update data</param>
        /// <returns>Updated user</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUser(long id, [FromBody] UpdateUserDto updateUserDto)
        {
            try
            {
                var user = await _userService.UpdateUserAsync(id, updateUserDto);
                if (user == null)
                {
                    return NotFound(new { error = "Kullanıcı bulunamadı." });
                }

                return Ok(new
                {
                    success = true,
                    message = "Kullanıcı başarıyla güncellendi.",
                    data = user
                });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { error = "Validation failed", details = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = "Business rule violation", details = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Delete user (soft delete)
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>Delete result</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteUser(long id)
        {
            try
            {
                var result = await _userService.DeleteUserAsync(id);
                if (!result)
                {
                    return NotFound(new { error = "Kullanıcı bulunamadı." });
                }

                return Ok(new { success = true, message = "Kullanıcı başarıyla silindi." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = "Business rule violation", details = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Assign role to user
        /// </summary>
        /// <param name="id">User ID</param>
        /// <param name="roleId">Role ID</param>
        /// <returns>Assignment result</returns>
        [HttpPost("{id}/assign-role/{roleId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AssignRoleToUser(long id, long roleId)
        {
            try
            {
                var result = await _userService.AssignRoleToUserAsync(id, roleId);
                if (!result)
                {
                    return NotFound(new { error = "Kullanıcı veya rol bulunamadı." });
                }

                return Ok(new { success = true, message = "Rol başarıyla atandı." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = "Business rule violation", details = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Assign multiple roles to user (bulk assignment)
        /// </summary>
        /// <param name="id">User ID</param>
        /// <param name="assignRolesDto">Role assignment data</param>
        /// <returns>Assignment result</returns>
        [HttpPost("{id}/assign-roles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AssignRolesToUser(long id, [FromBody] AssignRolesDto assignRolesDto)
        {
            try
            {
                // Check if user exists
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound(new { error = "Kullanıcı bulunamadı." });
                }

                // Get current user roles
                var currentRoles = await _userService.GetUserRolesAsync(id);
                var currentRoleNames = currentRoles.Select(r => r.Name).ToList();

                // Get all available roles from role service
                var _roleService = HttpContext.RequestServices.GetRequiredService<IRoleService>();
                var allRoles = await _roleService.GetAllRolesAsync();
                
                // Remove roles that are not in the new assignment
                foreach (var currentRole in currentRoles)
                {
                    if (!assignRolesDto.RoleNames.Contains(currentRole.Name))
                    {
                        await _userService.RemoveRoleFromUserAsync(id, currentRole.Id);
                    }
                }

                // Add new roles
                foreach (var roleName in assignRolesDto.RoleNames)
                {
                    if (!currentRoleNames.Contains(roleName))
                    {
                        var role = allRoles.FirstOrDefault(r => r.Name == roleName);
                        if (role != null)
                        {
                            await _userService.AssignRoleToUserAsync(id, role.Id);
                        }
                    }
                }

                return Ok(new { success = true, message = "Roller başarıyla atandı." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = "Business rule violation", details = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Remove role from user
        /// </summary>
        /// <param name="id">User ID</param>
        /// <param name="roleId">Role ID</param>
        /// <returns>Removal result</returns>
        [HttpDelete("{id}/remove-role/{roleId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveRoleFromUser(long id, long roleId)
        {
            try
            {
                var result = await _userService.RemoveRoleFromUserAsync(id, roleId);
                if (!result)
                {
                    return NotFound(new { error = "Kullanıcı, rol veya atama bulunamadı." });
                }

                return Ok(new { success = true, message = "Rol başarıyla kaldırıldı." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = "Business rule violation", details = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Get user roles
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>List of user roles</returns>
        [HttpGet("{id}/roles")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserRoles(long id)
        {
            try
            {
                var roles = await _userService.GetUserRolesAsync(id);
                return Ok(new { success = true, data = roles });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Search users by criteria
        /// </summary>
        /// <param name="searchTerm">Search term</param>
        /// <returns>List of matching users</returns>
        [HttpGet("search/{searchTerm}")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SearchUsers(string searchTerm)
        {
            try
            {
                var users = await _userService.SearchUsersAsync(searchTerm);
                return Ok(new { success = true, data = users });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Activate user
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>Activation result</returns>
        [HttpPost("{id}/activate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ActivateUser(long id)
        {
            try
            {
                var result = await _userService.ActivateUserAsync(id);
                if (!result)
                {
                    return NotFound(new { error = "Kullanıcı bulunamadı." });
                }

                return Ok(new { success = true, message = "Kullanıcı başarıyla aktifleştirildi." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Deactivate user
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>Deactivation result</returns>
        [HttpPost("{id}/deactivate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeactivateUser(long id)
        {
            try
            {
                var result = await _userService.DeactivateUserAsync(id);
                if (!result)
                {
                    return NotFound(new { error = "Kullanıcı bulunamadı." });
                }

                return Ok(new { success = true, message = "Kullanıcı başarıyla deaktifleştirildi." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }
    }

    #region Additional DTOs for User Operations

    public sealed class CreateUserDto
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public sealed class UpdateUserDto
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public bool? IsActive { get; set; }
    }

    public sealed class AssignRolesDto
    {
        public List<string> RoleNames { get; set; } = new();
    }

    #endregion
} 