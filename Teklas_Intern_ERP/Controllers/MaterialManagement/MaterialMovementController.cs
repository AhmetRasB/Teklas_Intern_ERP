using Microsoft.AspNetCore.Mvc;
using Teklas_Intern_ERP.Business.MaterialManagement;
using Teklas_Intern_ERP.Entities.MaterialManagement;
using Microsoft.EntityFrameworkCore;
using Teklas_Intern_ERP.DataAccess;

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
            _manager = new MaterialMovementManager(context);
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_manager.GetAll());

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var movement = _manager.GetById(id);
            if (movement == null) return NotFound();
            return Ok(movement);
        }

        [HttpPost]
        public IActionResult Add([FromBody] MaterialMovement movement)
        {
            var created = _manager.Add(movement);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] MaterialMovement movement)
        {
            movement.Id = id;
            var updated = _manager.Update(movement);
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