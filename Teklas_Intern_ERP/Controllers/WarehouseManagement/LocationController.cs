using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Teklas_Intern_ERP.Business.Interfaces;
using Teklas_Intern_ERP.DTOs.WarehouseManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using ValidationException = FluentValidation.ValidationException;

namespace Teklas_Intern_ERP.Controllers.WarehouseManagement
{
    /// <summary>
    /// Location Management API - Enterprise Resource Planning
    /// Basic CRUD operations for warehouse location management
    /// </summary>
    [ApiController]
    [Route("api/locations")]
    [ApiExplorerSettings(GroupName = "Warehouse Management")]
    [Produces("application/json")]
    [Authorize]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _service;

        public LocationController(ILocationService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get all locations
        /// </summary>
        /// <returns>List of locations</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<LocationDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<LocationDto>>> GetAll()
        {
            try
            {
                var locations = await _service.GetAllAsync();
                return Ok(locations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Get location by ID
        /// </summary>
        /// <param name="id">Location ID</param>
        /// <returns>Location details</returns>
        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(LocationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LocationDto>> GetById(long id)
        {
            try
            {
                var location = await _service.GetByIdAsync(id);
                if (location == null) 
                    return NotFound(new { error = "Location not found", id });
                
                return Ok(location);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Create a new location
        /// </summary>
        /// <param name="dto">Location data</param>
        /// <returns>Created location</returns>
        [HttpPost]
        [ProducesResponseType(typeof(LocationDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LocationDto>> Create([FromBody] LocationDto dto)
        {
            try
            {
                var location = await _service.AddAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = location.Id }, location);
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
        /// Update an existing location
        /// </summary>
        /// <param name="id">Location ID</param>
        /// <param name="dto">Updated location data</param>
        /// <returns>Updated location</returns>
        [HttpPut("{id:long}")]
        [ProducesResponseType(typeof(LocationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LocationDto>> Update(long id, [FromBody] LocationDto dto)
        {
            try
            {
                dto.Id = id;
                var location = await _service.UpdateAsync(dto);
                return Ok(location);
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
        /// Soft delete a location
        /// </summary>
        /// <param name="id">Location ID</param>
        /// <returns>Success status</returns>
        [HttpDelete("{id:long}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                var deleted = await _service.DeleteAsync(id);
                if (!deleted) 
                    return NotFound(new { error = "Location not found", id });
                
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = "Cannot delete location", details = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Restore a soft-deleted location
        /// </summary>
        /// <param name="id">Location ID</param>
        /// <returns>Success status</returns>
        [HttpPut("{id:long}/restore")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Restore(long id)
        {
            try
            {
                var restored = await _service.RestoreAsync(id);
                if (!restored) 
                    return NotFound(new { error = "Location not found", id });
                
                return Ok(new { message = "Location restored successfully", id });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = "Cannot restore location", details = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Permanently delete a location
        /// </summary>
        /// <param name="id">Location ID</param>
        /// <returns>Success status</returns>
        [HttpDelete("{id:long}/permanent")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PermanentDelete(long id)
        {
            try
            {
                var deleted = await _service.PermanentDeleteAsync(id);
                if (!deleted) 
                    return NotFound(new { error = "Location not found", id });
                
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = "Cannot delete location", details = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Get all deleted locations
        /// </summary>
        /// <returns>List of deleted locations</returns>
        [HttpGet("deleted")]
        [ProducesResponseType(typeof(List<LocationDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<LocationDto>>> GetDeleted()
        {
            try
            {
                var locations = await _service.GetDeletedAsync();
                return Ok(locations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Get locations by warehouse
        /// </summary>
        /// <param name="warehouseId">Warehouse ID</param>
        /// <returns>List of locations for the warehouse</returns>
        [HttpGet("warehouse/{warehouseId:long}")]
        [ProducesResponseType(typeof(List<LocationDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<LocationDto>>> GetByWarehouse(long warehouseId)
        {
            try
            {
                var locations = await _service.GetLocationsByWarehouseAsync(warehouseId);
                return Ok(locations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Search locations
        /// </summary>
        /// <param name="searchTerm">Search term</param>
        /// <returns>List of matching locations</returns>
        [HttpGet("search")]
        [ProducesResponseType(typeof(List<LocationDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<LocationDto>>> Search([FromQuery] string searchTerm)
        {
            try
            {
                var locations = await _service.SearchLocationsAsync(searchTerm);
                return Ok(locations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Get locations by type
        /// </summary>
        /// <param name="locationType">Location type</param>
        /// <returns>List of locations by type</returns>
        [HttpGet("type/{locationType}")]
        [ProducesResponseType(typeof(List<LocationDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<LocationDto>>> GetByType(string locationType)
        {
            try
            {
                var locations = await _service.GetLocationsByTypeAsync(locationType);
                return Ok(locations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Check if location code is unique
        /// </summary>
        /// <param name="code">Location code</param>
        /// <param name="excludeId">Exclude ID for updates</param>
        /// <returns>Uniqueness status</returns>
        [HttpGet("check-code")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> CheckCodeUnique([FromQuery] string code, [FromQuery] long? excludeId = null)
        {
            try
            {
                var isUnique = await _service.IsLocationCodeUniqueAsync(code, excludeId);
                return Ok(isUnique);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }
    }
} 