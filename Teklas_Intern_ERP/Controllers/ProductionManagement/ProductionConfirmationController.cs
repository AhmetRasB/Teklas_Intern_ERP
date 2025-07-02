using Microsoft.AspNetCore.Mvc;
using Teklas_Intern_ERP.Business.ProductionManagement;
using Teklas_Intern_ERP.DataAccess;
using Teklas_Intern_ERP.DataAccess.ProductionManagement;
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
            _manager = new ProductionConfirmationManager(new ProductionConfirmationRepository(context));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _manager.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var confirmation = await _manager.GetByIdAsync(id);
            if (confirmation == null) return NotFound();
            return Ok(confirmation);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ProductionConfirmation confirmation)
        {
            var created = await _manager.AddAsync(confirmation);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductionConfirmation confirmation)
        {
            confirmation.Id = id;
            var updated = await _manager.UpdateAsync(confirmation);
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