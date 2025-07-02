using Microsoft.AspNetCore.Mvc;
using Teklas_Intern_ERP.Business.WarehouseManagement;
using Teklas_Intern_ERP.Entities.WarehouseManagement;

namespace Teklas_Intern_ERP.Controllers.WarehouseManagement
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Warehouse Management")]
    public class StockEntryController : ControllerBase
    {
        private readonly StockEntryManager _manager = new StockEntryManager();

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _manager.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var entry = await _manager.GetByIdAsync(id);
            if (entry == null) return NotFound();
            return Ok(entry);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] StockEntry entry)
        {
            var created = await _manager.AddAsync(entry);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] StockEntry entry)
        {
            entry.Id = id;
            var updated = await _manager.UpdateAsync(entry);
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