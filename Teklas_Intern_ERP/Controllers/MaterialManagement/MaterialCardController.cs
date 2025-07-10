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
    /// Material Card Management API - Enterprise Resource Planning
    /// Basic CRUD operations for material management
    /// </summary>
    [ApiController]
    [Route("api/materials")]
    [ApiExplorerSettings(GroupName = "Material Management")]
    [Produces("application/json")]
    [Authorize]
    public class MaterialCardController : ControllerBase
    {
        private readonly IMaterialCardService _service;

        public MaterialCardController(IMaterialCardService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get all materials
        /// </summary>
        /// <returns>List of materials</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<MaterialCardDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<MaterialCardDto>>> GetAll()
        {
            try
            {
                var materials = await _service.GetAllAsync();
                return Ok(materials);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Get material by ID
        /// </summary>
        /// <param name="id">Material ID</param>
        /// <returns>Material details</returns>
        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(MaterialCardDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<MaterialCardDto>> GetById(long id)
        {
            try
            {
                var material = await _service.GetByIdAsync(id);
                if (material == null) 
                    return NotFound(new { error = "Material not found", id });
                
                return Ok(material);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Create a new material
        /// </summary>
        /// <param name="dto">Material data</param>
        /// <returns>Created material</returns>
        [HttpPost]
        [ProducesResponseType(typeof(MaterialCardDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<MaterialCardDto>> Create([FromBody] MaterialCardDto dto)
        {
            try
            {
                var material = await _service.AddAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = material.Id }, material);
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
        /// Update an existing material
        /// </summary>
        /// <param name="id">Material ID</param>
        /// <param name="dto">Updated material data</param>
        /// <returns>Updated material</returns>
        [HttpPut("{id:long}")]
        [ProducesResponseType(typeof(MaterialCardDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<MaterialCardDto>> Update(long id, [FromBody] MaterialCardDto dto)
        {
            try
            {
                dto.Id = id;
                var material = await _service.UpdateAsync(dto);
                return Ok(material);
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
        /// Soft delete a material
        /// </summary>
        /// <param name="id">Material ID</param>
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
                    return NotFound(new { error = "Material not found", id });
                
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = "Cannot delete material", details = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Restore a soft-deleted material
        /// </summary>
        /// <param name="id">Material ID</param>
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
                    return NotFound(new { error = "Material not found", id });
                
                return Ok(new { message = "Material restored successfully", id });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = "Cannot restore material", details = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Permanently delete a material
        /// </summary>
        /// <param name="id">Material ID</param>
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
                    return NotFound(new { error = "Material not found", id });
                
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = "Cannot delete material", details = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Get soft-deleted materials
        /// </summary>
        /// <returns>List of deleted materials</returns>
        [HttpGet("deleted")]
        [ProducesResponseType(typeof(List<MaterialCardDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<MaterialCardDto>>> GetDeleted()
        {
            try
            {
                var materials = await _service.GetDeletedAsync();
                return Ok(materials);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }
    }
} 