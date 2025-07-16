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
    /// Warehouse Management API - Enterprise Resource Planning
    /// Basic CRUD operations for warehouse management
    /// </summary>
    [ApiController]
    [Route("api/warehouses")]
    [ApiExplorerSettings(GroupName = "Warehouse Management")]
    [Produces("application/json")]
    [Authorize]
    public class WarehouseController : ControllerBase
    {
        private readonly IWarehouseService _service;

        public WarehouseController(IWarehouseService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get all warehouses
        /// </summary>
        /// <returns>List of warehouses</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<WarehouseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<WarehouseDto>>> GetAll()
        {
            try
            {
                var warehouses = await _service.GetAllAsync();
                return Ok(warehouses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Get warehouse by ID
        /// </summary>
        /// <param name="id">Warehouse ID</param>
        /// <returns>Warehouse details</returns>
        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(WarehouseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<WarehouseDto>> GetById(long id)
        {
            try
            {
                var warehouse = await _service.GetByIdAsync(id);
                if (warehouse == null) 
                    return NotFound(new { error = "Warehouse not found", id });
                
                return Ok(warehouse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Create a new warehouse
        /// </summary>
        /// <param name="dto">Warehouse data</param>
        /// <returns>Created warehouse</returns>
        [HttpPost]
        [ProducesResponseType(typeof(WarehouseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<WarehouseDto>> Create([FromBody] WarehouseDto dto)
        {
            try
            {
                var warehouse = await _service.AddAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = warehouse.Id }, warehouse);
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
        /// Update an existing warehouse
        /// </summary>
        /// <param name="id">Warehouse ID</param>
        /// <param name="dto">Updated warehouse data</param>
        /// <returns>Updated warehouse</returns>
        [HttpPut("{id:long}")]
        [ProducesResponseType(typeof(WarehouseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<WarehouseDto>> Update(long id, [FromBody] WarehouseDto dto)
        {
            try
            {
                dto.Id = id;
                var warehouse = await _service.UpdateAsync(dto);
                return Ok(warehouse);
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
        /// Soft delete a warehouse
        /// </summary>
        /// <param name="id">Warehouse ID</param>
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
                    return NotFound(new { error = "Warehouse not found", id });
                
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = "Cannot delete warehouse", details = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Restore a soft-deleted warehouse
        /// </summary>
        /// <param name="id">Warehouse ID</param>
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
                    return NotFound(new { error = "Warehouse not found", id });
                
                return Ok(new { message = "Warehouse restored successfully", id });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = "Cannot restore warehouse", details = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Permanently delete a warehouse
        /// </summary>
        /// <param name="id">Warehouse ID</param>
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
                    return NotFound(new { error = "Warehouse not found", id });
                
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = "Cannot delete warehouse", details = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Get all deleted warehouses
        /// </summary>
        /// <returns>List of deleted warehouses</returns>
        [HttpGet("deleted")]
        [ProducesResponseType(typeof(List<WarehouseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<WarehouseDto>>> GetDeleted()
        {
            try
            {
                var warehouses = await _service.GetDeletedAsync();
                return Ok(warehouses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Search warehouses
        /// </summary>
        /// <param name="searchTerm">Search term</param>
        /// <returns>List of matching warehouses</returns>
        [HttpGet("search")]
        [ProducesResponseType(typeof(List<WarehouseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<WarehouseDto>>> Search([FromQuery] string searchTerm)
        {
            try
            {
                var warehouses = await _service.SearchWarehousesAsync(searchTerm);
                return Ok(warehouses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Get warehouses by type
        /// </summary>
        /// <param name="warehouseType">Warehouse type</param>
        /// <returns>List of warehouses by type</returns>
        [HttpGet("type/{warehouseType}")]
        [ProducesResponseType(typeof(List<WarehouseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<WarehouseDto>>> GetByType(string warehouseType)
        {
            try
            {
                var warehouses = await _service.GetWarehousesByTypeAsync(warehouseType);
                return Ok(warehouses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Check if warehouse code is unique
        /// </summary>
        /// <param name="code">Warehouse code</param>
        /// <param name="excludeId">Exclude ID for updates</param>
        /// <returns>Uniqueness status</returns>
        [HttpGet("check-code")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> CheckCodeUnique([FromQuery] string code, [FromQuery] long? excludeId = null)
        {
            try
            {
                var isUnique = await _service.IsWarehouseCodeUniqueAsync(code, excludeId);
                return Ok(isUnique);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }
    }
} 