using Microsoft.AspNetCore.Mvc;
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
    /// Material Category Management API - Enterprise Resource Planning
    /// Basic CRUD operations for category management
    /// </summary>
    [ApiController]
    [Route("api/categories")]
    [ApiExplorerSettings(GroupName = "Material Management")]
    [Produces("application/json")]
    public class MaterialCategoryController : ControllerBase
    {
        private readonly IMaterialCategoryService _service;

        public MaterialCategoryController(IMaterialCategoryService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get all categories
        /// </summary>
        /// <returns>List of categories</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<MaterialCategoryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<MaterialCategoryDto>>> GetAll()
        {
            try
            {
                var categories = await _service.GetAllAsync();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Get category by ID
        /// </summary>
        /// <param name="id">Category ID</param>
        /// <returns>Category details</returns>
        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(MaterialCategoryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<MaterialCategoryDto>> GetById(long id)
        {
            try
            {
                var category = await _service.GetByIdAsync(id);
                if (category == null) 
                    return NotFound(new { error = "Category not found", id });
                
                return Ok(category);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>   
        /// Create a new category
        /// </summary>
        /// <param name="dto">Category data</param>
        /// <returns>Created category</returns>
        [HttpPost]
        [ProducesResponseType(typeof(MaterialCategoryDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<MaterialCategoryDto>> Create([FromBody] MaterialCategoryDto dto)
        {
            try
            {
                var category = await _service.AddAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
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
        /// Update an existing category
        /// </summary>
        /// <param name="id">Category ID</param>
        /// <param name="dto">Updated category data</param>
        /// <returns>Updated category</returns>
        [HttpPut("{id:long}")]
        [ProducesResponseType(typeof(MaterialCategoryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<MaterialCategoryDto>> Update(long id, [FromBody] MaterialCategoryDto dto)
        {
            try
            {
                dto.Id = id;
                var category = await _service.UpdateAsync(dto);
                return Ok(category);
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
        /// Soft delete a category
        /// </summary>
        /// <param name="id">Category ID</param>
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
                    return NotFound(new { error = "Category not found", id });
                
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = "Cannot delete category", details = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Restore a soft-deleted category
        /// </summary>
        /// <param name="id">Category ID</param>
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
                    return NotFound(new { error = "Category not found", id });
                
                return Ok(new { message = "Category restored successfully", id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Permanently delete a category
        /// </summary>
        /// <param name="id">Category ID</param>
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
                    return NotFound(new { error = "Category not found", id });
                
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = "Cannot permanently delete category", details = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Get all deleted (soft-deleted) categories
        /// </summary>
        /// <returns>List of deleted categories</returns>
        [HttpGet("deleted")]
        [ProducesResponseType(typeof(List<MaterialCategoryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<MaterialCategoryDto>>> GetDeleted()
        {
            try
            {
                var categories = await _service.GetDeletedAsync();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }
    }
} 