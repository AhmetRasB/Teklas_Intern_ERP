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
        public MaterialCardController(AppDbContext context)
        {
            _manager = new MaterialCardManager(context);
        }

        // GET: api/MaterialCard
        [HttpGet]
        public IActionResult GetAll() => Ok(_manager.GetAll());

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var card = _manager.GetById(id);
            if (card == null) return NotFound();
            return Ok(card);
        }

        [HttpPost]
        public IActionResult Add([FromBody] MaterialCard card)
        {
            var created = _manager.Add(card);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] MaterialCard card)
        {
            card.Id = id;
            var updated = _manager.Update(card);
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