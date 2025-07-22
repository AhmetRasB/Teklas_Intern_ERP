using Microsoft.AspNetCore.Mvc;
using Teklas_Intern_ERP.Business.Interfaces;
using Teklas_Intern_ERP.DTOs.PurchasingManagement;

namespace Teklas_Intern_ERP.Controllers.PurchasingManagement
{
    [ApiController]
    [Route("api/[controller]")]
    public class PurchaseOrderController : ControllerBase
    {
        private readonly IPurchaseOrderService _service;

        public PurchaseOrderController(IPurchaseOrderService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get all purchase orders
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PurchaseOrderDto>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        /// <summary>
        /// Get purchase order by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<PurchaseOrderDto>> GetById(long id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        /// <summary>
        /// Create new purchase order
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<PurchaseOrderDto>> Create([FromBody] PurchaseOrderDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Update purchase order
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<PurchaseOrderDto>> Update(long id, [FromBody] PurchaseOrderDto dto)
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
        /// Delete purchase order (soft delete)
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
        /// Restore deleted purchase order
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
        /// Search purchase orders
        /// </summary>
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<PurchaseOrderDto>>> Search([FromQuery] string q)
        {
            if (string.IsNullOrWhiteSpace(q))
                return BadRequest("Search term is required");

            var result = await _service.SearchAsync(q);
            return Ok(result);
        }

        /// <summary>
        /// Get all deleted (soft-deleted) purchase orders
        /// </summary>
        [HttpGet("deleted")]
        public async Task<ActionResult<IEnumerable<PurchaseOrderDto>>> GetDeleted()
        {
            try
            {
                var result = await _service.GetDeletedAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Permanently delete a purchase order
        /// </summary>
        [HttpDelete("{id}/permanent")]
        public async Task<ActionResult> PermanentDelete(long id)
        {
            try
            {
                var result = await _service.PermanentDeleteAsync(id);
                if (!result)
                    return NotFound(new { error = "Purchase order not found", id });
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }
    }
} 