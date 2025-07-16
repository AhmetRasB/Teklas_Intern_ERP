using Microsoft.AspNetCore.Mvc;
using Teklas_Intern_ERP.Business.Interfaces;
using Teklas_Intern_ERP.DTOs.SalesManagement;

namespace Teklas_Intern_ERP.Controllers.SalesManagement
{
    [ApiExplorerSettings(GroupName = "SalesManagement")]
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerOrderController : ControllerBase
    {
        private readonly ICustomerOrderService _service;

        public CustomerOrderController(ICustomerOrderService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get all customer orders
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerOrderDto>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        /// <summary>
        /// Get customer order by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerOrderDto>> GetById(long id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        /// <summary>
        /// Create new customer order
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<CustomerOrderDto>> Create([FromBody] CustomerOrderDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Update customer order
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<CustomerOrderDto>> Update(long id, [FromBody] CustomerOrderDto dto)
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
        /// Delete customer order (soft delete)
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
        /// Restore deleted customer order
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
        /// Search customer orders
        /// </summary>
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<CustomerOrderDto>>> Search([FromQuery] string q)
        {
            if (string.IsNullOrWhiteSpace(q))
                return BadRequest("Search term is required");

            var result = await _service.SearchAsync(q);
            return Ok(result);
        }
    }
} 