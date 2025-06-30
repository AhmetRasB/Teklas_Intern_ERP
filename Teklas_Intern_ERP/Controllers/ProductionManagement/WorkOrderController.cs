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
        public IActionResult GetAll() => Ok(_manager.GetAll());

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var order = _manager.GetById(id);
            if (order == null) return NotFound();
            return Ok(order);
        }

        [HttpPost]
        public IActionResult Add([FromBody] WorkOrder order)
        {
            var created = _manager.Add(order);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] WorkOrder order)
        {
            order.Id = id;
            var updated = _manager.Update(order);
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