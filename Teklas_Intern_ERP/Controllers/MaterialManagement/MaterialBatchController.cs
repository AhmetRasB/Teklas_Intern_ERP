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
        public IActionResult GetAll() => Ok(_manager.GetAll());

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var result = _manager.GetById(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public IActionResult Add(MaterialBatch batch)
        {
            var result = _manager.Add(batch);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut]
        public IActionResult Update(MaterialBatch batch)
        {
            var result = _manager.Update(batch);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _manager.Delete(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
} 