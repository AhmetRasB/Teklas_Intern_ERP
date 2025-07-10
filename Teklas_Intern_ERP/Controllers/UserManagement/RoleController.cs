using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Teklas_Intern_ERP.Business.Interfaces;
using Teklas_Intern_ERP.DTOs;
using System;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using ValidationException = FluentValidation.ValidationException;

namespace Teklas_Intern_ERP.Controllers.UserManagement
{
    /// <summary>
    /// Role Management API - Enterprise Resource Planning
    /// Role CRUD operations and user assignments
    /// </summary>
    [ApiController]
    [Route("api/roles")]
    [ApiExplorerSettings(GroupName = "User Management")]
    [Authorize]
    [Produces("application/json")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        /// <summary>
        /// Get all roles with filtering and pagination
        /// </summary>
        /// <param name="search">Search term</param>
        /// <param name="isActive">Active status filter</param>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>List of roles</returns>
        [HttpGet]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllRoles(
            [FromQuery] string? search = null,
            [FromQuery] bool? isActive = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var roles = await _roleService.GetAllRolesAsync(search, isActive, page, pageSize);
                var totalCount = await _roleService.GetRoleCountAsync(search, isActive);

                return Ok(new
                {
                    success = true,
                    data = roles,
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
        /// Get role by ID
        /// </summary>
        /// <param name="id">Role ID</param>
        /// <returns>Role details</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RoleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRole(long id)
        {
            try
            {
                var role = await _roleService.GetRoleByIdAsync(id);
                if (role == null)
                {
                    return NotFound(new { error = "Rol bulunamadı." });
                }

                return Ok(new { success = true, data = role });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Create new role
        /// </summary>
        /// <param name="createRoleDto">Role creation data</param>
        /// <returns>Created role</returns>
        [HttpPost]
        [ProducesResponseType(typeof(RoleDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleDto createRoleDto)
        {
            try
            {
                var role = await _roleService.CreateRoleAsync(createRoleDto);
                return StatusCode(201, new
                {
                    success = true,
                    message = "Rol başarıyla oluşturuldu.",
                    data = role
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
        /// Update role
        /// </summary>
        /// <param name="id">Role ID</param>
        /// <param name="updateRoleDto">Role update data</param>
        /// <returns>Updated role</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(RoleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateRole(long id, [FromBody] UpdateRoleDto updateRoleDto)
        {
            try
            {
                var role = await _roleService.UpdateRoleAsync(id, updateRoleDto);
                if (role == null)
                {
                    return NotFound(new { error = "Rol bulunamadı." });
                }

                return Ok(new
                {
                    success = true,
                    message = "Rol başarıyla güncellendi.",
                    data = role
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
        /// Delete role (soft delete)
        /// </summary>
        /// <param name="id">Role ID</param>
        /// <returns>Delete result</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteRole(long id)
        {
            try
            {
                var result = await _roleService.DeleteRoleAsync(id);
                if (!result)
                {
                    return NotFound(new { error = "Rol bulunamadı." });
                }

                return Ok(new { success = true, message = "Rol başarıyla silindi." });
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
        /// Get role users
        /// </summary>
        /// <param name="id">Role ID</param>
        /// <returns>List of users with this role</returns>
        [HttpGet("{id}/users")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRoleUsers(long id)
        {
            try
            {
                var users = await _roleService.GetRoleUsersAsync(id);
                return Ok(new { success = true, data = users });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Search roles by criteria
        /// </summary>
        /// <param name="searchTerm">Search term</param>
        /// <returns>List of matching roles</returns>
        [HttpGet("search/{searchTerm}")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SearchRoles(string searchTerm)
        {
            try
            {
                var roles = await _roleService.SearchRolesAsync(searchTerm);
                return Ok(new { success = true, data = roles });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Get all system roles
        /// </summary>
        /// <returns>List of system roles</returns>
        [HttpGet("system")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSystemRoles()
        {
            try
            {
                var roles = await _roleService.GetSystemRolesAsync();
                return Ok(new { success = true, data = roles });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Get all assignable roles (non-system roles)
        /// </summary>
        /// <returns>List of assignable roles</returns>
        [HttpGet("assignable")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAssignableRoles()
        {
            try
            {
                var roles = await _roleService.GetAssignableRolesAsync();
                return Ok(new { success = true, data = roles });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Activate role
        /// </summary>
        /// <param name="id">Role ID</param>
        /// <returns>Activation result</returns>
        [HttpPost("{id}/activate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ActivateRole(long id)
        {
            try
            {
                var result = await _roleService.ActivateRoleAsync(id);
                if (!result)
                {
                    return NotFound(new { error = "Rol bulunamadı." });
                }

                return Ok(new { success = true, message = "Rol başarıyla aktifleştirildi." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Deactivate role
        /// </summary>
        /// <param name="id">Role ID</param>
        /// <returns>Deactivation result</returns>
        [HttpPost("{id}/deactivate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeactivateRole(long id)
        {
            try
            {
                var result = await _roleService.DeactivateRoleAsync(id);
                if (!result)
                {
                    return NotFound(new { error = "Rol bulunamadı." });
                }

                return Ok(new { success = true, message = "Rol başarıyla deaktifleştirildi." });
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
    }

    #region Additional DTOs for Role Operations

    public sealed class CreateRoleDto
    {
        public string? Name { get; set; }
        public string? DisplayName { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsSystemRole { get; set; } = false;
    }

    public sealed class UpdateRoleDto
    {
        public string? Name { get; set; }
        public string? DisplayName { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
    }

    #endregion
} 