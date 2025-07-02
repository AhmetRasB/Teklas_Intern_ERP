using Microsoft.AspNetCore.Mvc;
using Teklas_Intern_ERP.Business.ProductionManagement;
using Teklas_Intern_ERP.Entities.ProductionManagement;
using Teklas_Intern_ERP.DataAccess;
using Teklas_Intern_ERP.DataAccess.ProductionManagement;

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
            _manager = new BillOfMaterialsManager(new BillOfMaterialsRepository(context));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _manager.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var bom = await _manager.GetByIdAsync(id);
            if (bom == null) return NotFound();
            return Ok(bom);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] BillOfMaterials bom)
        {
            var created = await _manager.AddAsync(bom);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BillOfMaterials bom)
        {
            bom.Id = id;
            var updated = await _manager.UpdateAsync(bom);
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