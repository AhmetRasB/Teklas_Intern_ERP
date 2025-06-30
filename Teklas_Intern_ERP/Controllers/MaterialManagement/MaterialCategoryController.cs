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
    public class MaterialCategoryController : ControllerBase
    {
        private readonly MaterialCategoryManager _manager;
        public MaterialCategoryController(AppDbContext context)
        {
            _manager = new MaterialCategoryManager(context);
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_manager.GetAll());

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var category = _manager.GetById(id);
            if (category == null) return NotFound();
            return Ok(category);
        }

        [HttpPost]
        public IActionResult Add([FromBody] MaterialCategory category)
        {
            var created = _manager.Add(category);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] MaterialCategory category)
        {
            category.Id = id;
            var updated = _manager.Update(category);
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