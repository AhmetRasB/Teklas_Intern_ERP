using Microsoft.AspNetCore.Mvc;
using Teklas_Intern_ERP.Business.WarehouseManagement;
using Teklas_Intern_ERP.Entities.WarehouseManagement;
using Microsoft.EntityFrameworkCore;
using Teklas_Intern_ERP.DataAccess;

namespace Teklas_Intern_ERP.Controllers.WarehouseManagement
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Warehouse Management")]
    public class LocationController : ControllerBase
    {
        private readonly LocationManager _manager;
        public LocationController(AppDbContext context)
        {
            _manager = new LocationManager(context);
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_manager.GetAll());

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var location = _manager.GetById(id);
            if (location == null) return NotFound();
            return Ok(location);
        }

        [HttpPost]
        public IActionResult Add([FromBody] Location location)
        {
            var created = _manager.Add(location);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Location location)
        {
            location.Id = id;
            var updated = _manager.Update(location);
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