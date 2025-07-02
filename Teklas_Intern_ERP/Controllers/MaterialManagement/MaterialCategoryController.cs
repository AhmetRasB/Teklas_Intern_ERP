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
    public class MaterialCategoryController : ControllerBase
    {
        private readonly MaterialCategoryManager _manager;
        public MaterialCategoryController(AppDbContext context)
        {
            _manager = new MaterialCategoryManager(new MaterialCategoryRepository(context));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _manager.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _manager.GetByIdAsync(id);
            if (category == null) return NotFound();
            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] MaterialCategory category)
        {
            var created = await _manager.AddAsync(category);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MaterialCategory category)
        {
            category.Id = id;
            var updated = await _manager.UpdateAsync(category);
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