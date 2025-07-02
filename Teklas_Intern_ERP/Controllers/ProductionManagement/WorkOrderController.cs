using Microsoft.AspNetCore.Mvc;
using Teklas_Intern_ERP.Business.ProductionManagement;
using Teklas_Intern_ERP.Entities.ProductionManagement;
using Teklas_Intern_ERP.DataAccess;

namespace Teklas_Intern_ERP.Controllers.ProductionManagement
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Production Management")]
    public class WorkOrderController : ControllerBase
    {
        private readonly WorkOrderManager _manager;
        public WorkOrderController(AppDbContext context)
        {
            _manager = new WorkOrderManager(context);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _manager.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _manager.GetByIdAsync(id);
            if (order == null) return NotFound();
            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] WorkOrder order)
        {
            var created = await _manager.AddAsync(order);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] WorkOrder order)
        {
            order.Id = id;
            var updated = await _manager.UpdateAsync(order);
            if (!updated) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _manager.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
} 