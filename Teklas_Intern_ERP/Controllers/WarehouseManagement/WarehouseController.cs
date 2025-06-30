using Microsoft.AspNetCore.Mvc;
using Teklas_Intern_ERP.Business.WarehouseManagement;
using Teklas_Intern_ERP.Entities.WarehouseManagement;
using Microsoft.EntityFrameworkCore;

namespace Teklas_Intern_ERP.Controllers.WarehouseManagement
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Warehouse Management")]
    public class WarehouseController : ControllerBase
    {
        private readonly WarehouseManager _manager = new WarehouseManager();

        [HttpGet]
        public IActionResult GetAll() => Ok(_manager.GetAll());

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var warehouse = _manager.GetById(id);
            if (warehouse == null) return NotFound();
            return Ok(warehouse);
        }

        [HttpPost]
        public IActionResult Add([FromBody] Warehouse warehouse)
        {
            var created = _manager.Add(warehouse);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Warehouse warehouse)
        {
            warehouse.Id = id;
            var updated = _manager.Update(warehouse);
            if (!updated) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var deleted = _manager.Delete(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
} 