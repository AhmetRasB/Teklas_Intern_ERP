using Microsoft.AspNetCore.Mvc;
using Teklas_Intern_ERP.Business.WarehouseManagement;
using Teklas_Intern_ERP.Entities.WarehouseManagement;
using Microsoft.EntityFrameworkCore;
using Teklas_Intern_ERP.DataAccess;
using Teklas_Intern_ERP.DataAccess.WarehouseManagement;

namespace Teklas_Intern_ERP.Controllers.WarehouseManagement
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Warehouse Management")]
    public class WarehouseController : ControllerBase
    {
        private readonly WarehouseManager _manager;
        public WarehouseController(AppDbContext context)
        {
            _manager = new WarehouseManager(new WarehouseRepository(context));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _manager.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var warehouse = await _manager.GetByIdAsync(id);
            if (warehouse == null) return NotFound();
            return Ok(warehouse);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Warehouse warehouse)
        {
            var created = await _manager.AddAsync(warehouse);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Warehouse warehouse)
        {
            warehouse.Id = id;
            var updated = await _manager.UpdateAsync(warehouse);
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