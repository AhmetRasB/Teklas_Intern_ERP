using Microsoft.AspNetCore.Mvc;
using Teklas_Intern_ERP.Business.MaterialManagement;
using Teklas_Intern_ERP.Entities.MaterialManagement;

namespace Teklas_Intern_ERP.Controllers.MaterialManagement
{
    [ApiController]
    [Route("api/[controller]")]
    public class MaterialBatchController : ControllerBase
    {
        private readonly MaterialBatchManager _manager;
        public MaterialBatchController(MaterialBatchManager manager)
        {
            _manager = manager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _manager.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _manager.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add(MaterialBatch batch)
        {
            var result = await _manager.AddAsync(batch);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(MaterialBatch batch)
        {
            var result = await _manager.UpdateAsync(batch);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _manager.DeleteAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
} 