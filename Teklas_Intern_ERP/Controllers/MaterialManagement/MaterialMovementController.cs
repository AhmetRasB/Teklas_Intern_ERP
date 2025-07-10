using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Teklas_Intern_ERP.Business.Interfaces;
using Teklas_Intern_ERP.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using ValidationException = FluentValidation.ValidationException;

namespace Teklas_Intern_ERP.Controllers.MaterialManagement
{
    /// <summary>
    /// Material Movement Management API - Enterprise Resource Planning
    /// Stock management operations for material movements
    /// </summary>
    [ApiController]
    [Route("api/MaterialMovement")]
    [ApiExplorerSettings(GroupName = "Material Management")]
    [Produces("application/json")]
    [Authorize]
    public class MaterialMovementController : ControllerBase
    {
        private readonly IMaterialMovementService _service;

        public MaterialMovementController(IMaterialMovementService service)
        {
            _service = service;
        }

        #region Basic CRUD Operations

        /// <summary>
        /// Get all material movements
        /// </summary>
        /// <returns>List of material movements</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<MaterialMovementDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<MaterialMovementDto>>> GetAll()
        {
            try
            {
                var movements = await _service.GetAllAsync();
                return Ok(movements);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Get material movement by ID
        /// </summary>
        /// <param name="id">Movement ID</param>
        /// <returns>Movement details</returns>
        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(MaterialMovementDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<MaterialMovementDto>> GetById(long id)
        {
            try
            {
                var movement = await _service.GetByIdAsync(id);
                if (movement == null) 
                    return NotFound(new { error = "Movement not found", id });
                
                return Ok(movement);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Create a new material movement
        /// </summary>
        /// <param name="dto">Movement data</param>
        /// <returns>Created movement</returns>
        [HttpPost]
        [ProducesResponseType(typeof(MaterialMovementDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<MaterialMovementDto>> Create([FromBody] MaterialMovementDto dto)
        {
            try
            {
                var movement = await _service.AddAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = movement.Id }, movement);
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
        /// Update an existing material movement
        /// </summary>
        /// <param name="id">Movement ID</param>
        /// <param name="dto">Updated movement data</param>
        /// <returns>Updated movement</returns>
        [HttpPut("{id:long}")]
        [ProducesResponseType(typeof(MaterialMovementDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<MaterialMovementDto>> Update(long id, [FromBody] MaterialMovementDto dto)
        {
            try
            {
                dto.Id = id;
                var movement = await _service.UpdateAsync(dto);
                return Ok(movement);
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
        /// Soft delete a material movement
        /// </summary>
        /// <param name="id">Movement ID</param>
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
                    return NotFound(new { error = "Movement not found", id });
                
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = "Cannot delete movement", details = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        #endregion

        #region Soft Delete Management

        /// <summary>
        /// Get all soft deleted material movements
        /// </summary>
        /// <returns>List of deleted movements</returns>
        [HttpGet("deleted")]
        [ProducesResponseType(typeof(List<MaterialMovementDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<MaterialMovementDto>>> GetDeleted()
        {
            try
            {
                var movements = await _service.GetDeletedAsync();
                return Ok(movements);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Restore a soft deleted material movement
        /// </summary>
        /// <param name="id">Movement ID</param>
        /// <returns>Success status</returns>
        [HttpPost("{id:long}/restore")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Restore(long id)
        {
            try
            {
                var restored = await _service.RestoreAsync(id);
                if (!restored) 
                    return NotFound(new { error = "Movement not found", id });
                
                return Ok(new { message = "Movement restored successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Permanently delete a material movement
        /// </summary>
        /// <param name="id">Movement ID</param>
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
                    return NotFound(new { error = "Movement not found", id });
                
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        #endregion

        #region Stock Management Operations

        /// <summary>
        /// Process stock in movement
        /// </summary>
        /// <param name="dto">Stock in data</param>
        /// <returns>Created movement</returns>
        [HttpPost("stock-in")]
        [ProducesResponseType(typeof(MaterialMovementDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<MaterialMovementDto>> StockIn([FromBody] MaterialMovementDto dto)
        {
            try
            {
                var movement = await _service.ProcessStockInAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = movement.Id }, movement);
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
        /// Process stock out movement
        /// </summary>
        /// <param name="dto">Stock out data</param>
        /// <returns>Created movement</returns>
        [HttpPost("stock-out")]
        [ProducesResponseType(typeof(MaterialMovementDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<MaterialMovementDto>> StockOut([FromBody] MaterialMovementDto dto)
        {
            try
            {
                var movement = await _service.ProcessStockOutAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = movement.Id }, movement);
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
        /// Process transfer movement
        /// </summary>
        /// <param name="dto">Transfer data</param>
        /// <returns>Created movement</returns>
        [HttpPost("transfer")]
        [ProducesResponseType(typeof(MaterialMovementDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<MaterialMovementDto>> Transfer([FromBody] MaterialMovementDto dto)
        {
            try
            {
                var movement = await _service.ProcessTransferAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = movement.Id }, movement);
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
        /// Process adjustment movement
        /// </summary>
        /// <param name="dto">Adjustment data</param>
        /// <returns>Created movement</returns>
        [HttpPost("adjustment")]
        [ProducesResponseType(typeof(MaterialMovementDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<MaterialMovementDto>> Adjustment([FromBody] MaterialMovementDto dto)
        {
            try
            {
                var movement = await _service.ProcessAdjustmentAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = movement.Id }, movement);
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
        /// Get current stock balance for a material
        /// </summary>
        /// <param name="materialCardId">Material card ID</param>
        /// <returns>Current stock balance</returns>
        [HttpGet("stock-balance/{materialCardId:long}")]
        [ProducesResponseType(typeof(decimal), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<decimal>> GetStockBalance(long materialCardId)
        {
            try
            {
                var balance = await _service.GetCurrentStockBalanceAsync(materialCardId);
                return Ok(balance);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        #endregion

        #region Status Management

        /// <summary>
        /// Confirm a pending movement
        /// </summary>
        /// <param name="id">Movement ID</param>
        /// <returns>Success status</returns>
        [HttpPut("{id:long}/confirm")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Confirm(long id)
        {
            try
            {
                var confirmed = await _service.ConfirmMovementAsync(id);
                if (!confirmed) 
                    return NotFound(new { error = "Movement not found", id });
                
                return Ok(new { message = "Movement confirmed successfully", id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Cancel a movement
        /// </summary>
        /// <param name="id">Movement ID</param>
        /// <returns>Success status</returns>
        [HttpPut("{id:long}/cancel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Cancel(long id)
        {
            try
            {
                var cancelled = await _service.CancelMovementAsync(id);
                if (!cancelled) 
                    return NotFound(new { error = "Movement not found", id });
                
                return Ok(new { message = "Movement cancelled successfully", id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Complete a movement
        /// </summary>
        /// <param name="id">Movement ID</param>
        /// <returns>Success status</returns>
        [HttpPut("{id:long}/complete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Complete(long id)
        {
            try
            {
                var completed = await _service.CompleteMovementAsync(id);
                if (!completed) 
                    return NotFound(new { error = "Movement not found", id });
                
                return Ok(new { message = "Movement completed successfully", id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        #endregion

        #region Search and Filter Operations

        /// <summary>
        /// Get movements by material card
        /// </summary>
        /// <param name="materialCardId">Material card ID</param>
        /// <returns>List of movements</returns>
        [HttpGet("by-material/{materialCardId:long}")]
        [ProducesResponseType(typeof(List<MaterialMovementDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<MaterialMovementDto>>> GetByMaterialCard(long materialCardId)
        {
            try
            {
                var movements = await _service.GetMovementsByMaterialCardAsync(materialCardId);
                return Ok(movements);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Get movements by date range
        /// </summary>
        /// <param name="startDate">Start date</param>
        /// <param name="endDate">End date</param>
        /// <returns>List of movements</returns>
        [HttpGet("by-date")]
        [ProducesResponseType(typeof(List<MaterialMovementDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<MaterialMovementDto>>> GetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                if (startDate > endDate)
                    return BadRequest(new { error = "Start date cannot be greater than end date" });

                var movements = await _service.GetMovementsByDateRangeAsync(startDate, endDate);
                return Ok(movements);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Get movements by type
        /// </summary>
        /// <param name="movementType">Movement type (IN, OUT, TRANSFER, ADJUSTMENT)</param>
        /// <returns>List of movements</returns>
        [HttpGet("by-type/{movementType}")]
        [ProducesResponseType(typeof(List<MaterialMovementDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<MaterialMovementDto>>> GetByType(string movementType)
        {
            try
            {
                var movements = await _service.GetMovementsByTypeAsync(movementType.ToUpper());
                return Ok(movements);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Get pending movements
        /// </summary>
        /// <returns>List of pending movements</returns>
        [HttpGet("pending")]
        [ProducesResponseType(typeof(List<MaterialMovementDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<MaterialMovementDto>>> GetPending()
        {
            try
            {
                var movements = await _service.GetPendingMovementsAsync();
                return Ok(movements);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Get recent movements
        /// </summary>
        /// <param name="take">Number of movements to take (default: 50)</param>
        /// <returns>List of recent movements</returns>
        [HttpGet("recent")]
        [ProducesResponseType(typeof(List<MaterialMovementDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<MaterialMovementDto>>> GetRecent([FromQuery] int take = 50)
        {
            try
            {
                var movements = await _service.GetRecentMovementsAsync(take);
                return Ok(movements);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Search movements
        /// </summary>
        /// <param name="searchTerm">Search term</param>
        /// <returns>List of matching movements</returns>
        [HttpGet("search")]
        [ProducesResponseType(typeof(List<MaterialMovementDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<MaterialMovementDto>>> Search([FromQuery] string searchTerm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                    return BadRequest(new { error = "Search term is required" });

                var movements = await _service.SearchMovementsAsync(searchTerm);
                return Ok(movements);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        #endregion

        #region Report Operations

        /// <summary>
        /// Get stock in report
        /// </summary>
        /// <param name="startDate">Start date</param>
        /// <param name="endDate">End date</param>
        /// <returns>Stock in movements report</returns>
        [HttpGet("reports/stock-in")]
        [ProducesResponseType(typeof(List<MaterialMovementDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<MaterialMovementDto>>> GetStockInReport([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                if (startDate > endDate)
                    return BadRequest(new { error = "Start date cannot be greater than end date" });

                var movements = await _service.GetStockInReportAsync(startDate, endDate);
                return Ok(movements);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Get stock out report
        /// </summary>
        /// <param name="startDate">Start date</param>
        /// <param name="endDate">End date</param>
        /// <returns>Stock out movements report</returns>
        [HttpGet("reports/stock-out")]
        [ProducesResponseType(typeof(List<MaterialMovementDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<MaterialMovementDto>>> GetStockOutReport([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                if (startDate > endDate)
                    return BadRequest(new { error = "Start date cannot be greater than end date" });

                var movements = await _service.GetStockOutReportAsync(startDate, endDate);
                return Ok(movements);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Get transfer report
        /// </summary>
        /// <param name="startDate">Start date</param>
        /// <param name="endDate">End date</param>
        /// <returns>Transfer movements report</returns>
        [HttpGet("reports/transfers")]
        [ProducesResponseType(typeof(List<MaterialMovementDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<MaterialMovementDto>>> GetTransferReport([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                if (startDate > endDate)
                    return BadRequest(new { error = "Start date cannot be greater than end date" });

                var movements = await _service.GetTransferReportAsync(startDate, endDate);
                return Ok(movements);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Get inventory adjustment report
        /// </summary>
        /// <param name="startDate">Start date</param>
        /// <param name="endDate">End date</param>
        /// <returns>Adjustment movements report</returns>
        [HttpGet("reports/adjustments")]
        [ProducesResponseType(typeof(List<MaterialMovementDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<MaterialMovementDto>>> GetAdjustmentReport([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                if (startDate > endDate)
                    return BadRequest(new { error = "Start date cannot be greater than end date" });

                var movements = await _service.GetInventoryAdjustmentReportAsync(startDate, endDate);
                return Ok(movements);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        #endregion
    }
} 