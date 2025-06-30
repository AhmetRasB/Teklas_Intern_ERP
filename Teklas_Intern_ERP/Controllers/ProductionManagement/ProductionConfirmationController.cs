using Microsoft.AspNetCore.Mvc;
using Teklas_Intern_ERP.Business.ProductionManagement;
using Teklas_Intern_ERP.DataAccess;
using Teklas_Intern_ERP.Entities.ProductionManagement;

namespace Teklas_Intern_ERP.Controllers.ProductionManagement
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Production Management")]
    public class ProductionConfirmationController : ControllerBase
    {
        private readonly ProductionConfirmationManager _manager;
        public ProductionConfirmationController(AppDbContext context)
        {
            _manager = new ProductionConfirmationManager(context);
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_manager.GetAll());

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var confirmation = _manager.GetById(id);
            if (confirmation == null) return NotFound();
            return Ok(confirmation);
        }

        [HttpPost]
        public IActionResult Add([FromBody] ProductionConfirmation confirmation)
        {
            var created = _manager.Add(confirmation);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] ProductionConfirmation confirmation)
        {
            confirmation.Id = id;
            var updated = _manager.Update(confirmation);
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