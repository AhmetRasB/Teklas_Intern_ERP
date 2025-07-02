using Microsoft.AspNetCore.Mvc;
using Teklas_Intern_ERP.Business.MaterialManagement;
using Teklas_Intern_ERP.Entities.MaterialManagement;
using Microsoft.EntityFrameworkCore;
using Teklas_Intern_ERP.DataAccess;
using Teklas_Intern_ERP.DataAccess.MaterialManagement;

namespace Teklas_Intern_ERP.Controllers.MaterialManagement
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Material Management")]
    public class MaterialMovementController : ControllerBase
    {
        private readonly MaterialMovementManager _manager;
        public MaterialMovementController(AppDbContext context)
        {
            _manager = new MaterialMovementManager(new MaterialMovementRepository(context));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _manager.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var movement = await _manager.GetByIdAsync(id);
            if (movement == null) return NotFound();
            return Ok(movement);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] MaterialMovement movement)
        {
            var created = await _manager.AddAsync(movement);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MaterialMovement movement)
        {
            movement.Id = id;
            var updated = await _manager.UpdateAsync(movement);
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