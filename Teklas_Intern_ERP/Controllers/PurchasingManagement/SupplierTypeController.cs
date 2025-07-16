using Microsoft.AspNetCore.Mvc;
using Teklas_Intern_ERP.Business.Interfaces;
using Teklas_Intern_ERP.DTOs.PurchasingManagement;

namespace Teklas_Intern_ERP.Controllers.PurchasingManagement
{
    [ApiController]
    [Route("api/[controller]")]
    public class SupplierTypeController : ControllerBase
    {
        private readonly ISupplierTypeService _service;

        public SupplierTypeController(ISupplierTypeService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get all supplier types
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SupplierTypeDto>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        /// <summary>
        /// Get supplier type by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<SupplierTypeDto>> GetById(long id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        /// <summary>
        /// Create new supplier type
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<SupplierTypeDto>> Create([FromBody] SupplierTypeDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Update supplier type
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<SupplierTypeDto>> Update(long id, [FromBody] SupplierTypeDto dto)
        {
            try
            {
                var result = await _service.UpdateAsync(id, dto);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Delete supplier type (soft delete)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Restore deleted supplier type
        /// </summary>
        [HttpPost("{id}/restore")]
        public async Task<ActionResult> Restore(long id)
        {
            var result = await _service.RestoreAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Search supplier types
        /// </summary>
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<SupplierTypeDto>>> Search([FromQuery] string q)
        {
            if (string.IsNullOrWhiteSpace(q))
                return BadRequest("Search term is required");

            var result = await _service.SearchAsync(q);
            return Ok(result);
        }
    }
} 