using Microsoft.AspNetCore.Mvc;
using Teklas_Intern_ERP.Business.ProductionManagement;
using Teklas_Intern_ERP.Entities.ProductionManagement;
using Teklas_Intern_ERP.DataAccess;

namespace Teklas_Intern_ERP.Controllers.ProductionManagement
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Production Management")]
    public class BillOfMaterialsController : ControllerBase
    {
        private readonly BillOfMaterialsManager _manager;
        public BillOfMaterialsController(AppDbContext context)
        {
            _manager = new BillOfMaterialsManager(context);
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_manager.GetAll());

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var bom = _manager.GetById(id);
            if (bom == null) return NotFound();
            return Ok(bom);
        }

        [HttpPost]
        public IActionResult Add([FromBody] BillOfMaterials bom)
        {
            var created = _manager.Add(bom);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] BillOfMaterials bom)
        {
            bom.Id = id;
            var updated = _manager.Update(bom);
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