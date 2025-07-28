using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Teklas_Intern_ERP.DTOs.UserManagement;
using Teklas_Intern_ERP.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Teklas_Intern_ERP.Controllers.UserManagement
{
    [ApiController]
    [Route("api/user-table-column-preferences")]
    [Authorize]
    public class UserTableColumnPreferenceController : ControllerBase
    {
        private readonly IUserTableColumnPreferenceService _service;

        public UserTableColumnPreferenceController(IUserTableColumnPreferenceService service)
        {
            _service = service;
        }

        // GET: api/user-table-column-preferences?tableKey=MaterialCardTable
        [HttpGet]
        public async Task<IActionResult> GetPreference([FromQuery] string tableKey)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userIdClaim, out var userId))
                    return Unauthorized();
                var pref = await _service.GetPreferenceAsync(userId, tableKey);
                if (pref == null)
                    return NotFound();
                return Ok(pref);
            }
            catch (FluentValidation.ValidationException ex)
            {
                return BadRequest(new { error = "Validation failed", details = ex.Message });
            }
            catch (System.Exception ex)
            {
                // Log ex.ToString() in production
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        // POST: api/user-table-column-preferences
        [HttpPost]
        public async Task<IActionResult> SetPreference([FromBody] UserTableColumnPreferenceDto dto)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userIdClaim, out var userId))
                    return Unauthorized();
                // Always use authenticated user's ID
                dto.UserId = userId;
                var result = await _service.SetPreferenceAsync(dto);
                if (result)
                    return Ok();
                return StatusCode(500, new { error = "Could not save preferences." });
            }
            catch (FluentValidation.ValidationException ex)
            {
                return BadRequest(new { error = "Validation failed", details = ex.Message });
            }
            catch (System.Exception ex)
            {
                // Log ex.ToString() in production
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }
    }
} 