using Microsoft.AspNetCore.Mvc;
using Teklas_Intern_ERP.Business.MaterialManagement;
using Teklas_Intern_ERP.Entities.MaterialManagement;
using Teklas_Intern_ERP.DataAccess;

namespace Teklas_Intern_ERP.Controllers.MaterialManagement
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Material Management")]
    public class MaterialCardController : ControllerBase
    {
        private readonly MaterialCardManager _manager;
        public MaterialCardController(MaterialCardManager manager)
        {
            _manager = manager;
        }

        // GET: api/MaterialCard
        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _manager.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var card = await _manager.GetByIdAsync(id);
            if (card == null) return NotFound();
            return Ok(card);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] MaterialCard card)
        {
            var created = await _manager.AddAsync(card);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MaterialCard card)
        {
            card.Id = id;
            var updated = await _manager.UpdateAsync(card);
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