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
    /// Stock Entry Management API - Enterprise Resource Planning
    /// Basic CRUD operations for stock entry management
    /// </summary>
    [ApiController]
    [Route("api/stockentries")]
    [ApiExplorerSettings(GroupName = "Warehouse Management")]
    [Produces("application/json")]
    [Authorize]
    public class StockEntryController : ControllerBase
    {
        private readonly IStockEntryService _service;

        public StockEntryController(IStockEntryService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get all stock entries
        /// </summary>
        /// <returns>List of stock entries</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<StockEntryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<StockEntryDto>>> GetAll()
        {
            try
            {
                var stockEntries = await _service.GetAllAsync();
                return Ok(stockEntries);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Get stock entry by ID
        /// </summary>
        /// <param name="id">Stock entry ID</param>
        /// <returns>Stock entry details</returns>
        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(StockEntryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StockEntryDto>> GetById(long id)
        {
            try
            {
                var stockEntry = await _service.GetByIdAsync(id);
                if (stockEntry == null) 
                    return NotFound(new { error = "Stock entry not found", id });
                
                return Ok(stockEntry);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Create a new stock entry
        /// </summary>
        /// <param name="dto">Stock entry data</param>
        /// <returns>Created stock entry</returns>
        [HttpPost]
        [ProducesResponseType(typeof(StockEntryDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StockEntryDto>> Create([FromBody] StockEntryDto dto)
        {
            try
            {
                var stockEntry = await _service.AddAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = stockEntry.Id }, stockEntry);
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
        /// Update an existing stock entry
        /// </summary>
        /// <param name="id">Stock entry ID</param>
        /// <param name="dto">Updated stock entry data</param>
        /// <returns>Updated stock entry</returns>
        [HttpPut("{id:long}")]
        [ProducesResponseType(typeof(StockEntryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StockEntryDto>> Update(long id, [FromBody] StockEntryDto dto)
        {
            try
            {
                dto.Id = id;
                var stockEntry = await _service.UpdateAsync(dto);
                return Ok(stockEntry);
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
        /// Soft delete a stock entry
        /// </summary>
        /// <param name="id">Stock entry ID</param>
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
                    return NotFound(new { error = "Stock entry not found", id });
                
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = "Cannot delete stock entry", details = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Restore a soft-deleted stock entry
        /// </summary>
        /// <param name="id">Stock entry ID</param>
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
                    return NotFound(new { error = "Stock entry not found", id });
                
                return Ok(new { message = "Stock entry restored successfully", id });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = "Cannot restore stock entry", details = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Permanently delete a stock entry
        /// </summary>
        /// <param name="id">Stock entry ID</param>
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
                    return NotFound(new { error = "Stock entry not found", id });
                
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = "Cannot delete stock entry", details = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Get all deleted stock entries
        /// </summary>
        /// <returns>List of deleted stock entries</returns>
        [HttpGet("deleted")]
        [ProducesResponseType(typeof(List<StockEntryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<StockEntryDto>>> GetDeleted()
        {
            try
            {
                var stockEntries = await _service.GetDeletedAsync();
                return Ok(stockEntries);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Get stock entries by warehouse
        /// </summary>
        /// <param name="warehouseId">Warehouse ID</param>
        /// <returns>List of stock entries for the warehouse</returns>
        [HttpGet("warehouse/{warehouseId:long}")]
        [ProducesResponseType(typeof(List<StockEntryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<StockEntryDto>>> GetByWarehouse(long warehouseId)
        {
            try
            {
                var stockEntries = await _service.GetStockEntriesByWarehouseAsync(warehouseId);
                return Ok(stockEntries);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Get stock entries by location
        /// </summary>
        /// <param name="locationId">Location ID</param>
        /// <returns>List of stock entries for the location</returns>
        [HttpGet("location/{locationId:long}")]
        [ProducesResponseType(typeof(List<StockEntryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<StockEntryDto>>> GetByLocation(long locationId)
        {
            try
            {
                var stockEntries = await _service.GetStockEntriesByLocationAsync(locationId);
                return Ok(stockEntries);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Get stock entries by material
        /// </summary>
        /// <param name="materialId">Material ID</param>
        /// <returns>List of stock entries for the material</returns>
        [HttpGet("material/{materialId:long}")]
        [ProducesResponseType(typeof(List<StockEntryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<StockEntryDto>>> GetByMaterial(long materialId)
        {
            try
            {
                var stockEntries = await _service.GetStockEntriesByMaterialAsync(materialId);
                return Ok(stockEntries);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Get stock entries by type
        /// </summary>
        /// <param name="entryType">Entry type</param>
        /// <returns>List of stock entries by type</returns>
        [HttpGet("type/{entryType}")]
        [ProducesResponseType(typeof(List<StockEntryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<StockEntryDto>>> GetByType(string entryType)
        {
            try
            {
                var stockEntries = await _service.GetStockEntriesByTypeAsync(entryType);
                return Ok(stockEntries);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Search stock entries
        /// </summary>
        /// <param name="searchTerm">Search term</param>
        /// <returns>List of matching stock entries</returns>
        [HttpGet("search")]
        [ProducesResponseType(typeof(List<StockEntryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<StockEntryDto>>> Search([FromQuery] string searchTerm)
        {
            try
            {
                var stockEntries = await _service.SearchStockEntriesAsync(searchTerm);
                return Ok(stockEntries);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Check if entry number is unique
        /// </summary>
        /// <param name="entryNumber">Entry number</param>
        /// <param name="excludeId">Exclude ID for updates</param>
        /// <returns>Uniqueness status</returns>
        [HttpGet("check-entry-number")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> CheckEntryNumberUnique([FromQuery] string entryNumber, [FromQuery] long? excludeId = null)
        {
            try
            {
                var isUnique = await _service.IsEntryNumberUniqueAsync(entryNumber, excludeId);
                return Ok(isUnique);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }
    }
} 