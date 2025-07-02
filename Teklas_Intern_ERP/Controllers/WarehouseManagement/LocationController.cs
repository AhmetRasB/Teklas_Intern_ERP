using Microsoft.AspNetCore.Mvc;
using Teklas_Intern_ERP.Business.WarehouseManagement;
using Teklas_Intern_ERP.Entities.WarehouseManagement;
using Microsoft.EntityFrameworkCore;
using Teklas_Intern_ERP.DataAccess;
using Teklas_Intern_ERP.DataAccess.WarehouseManagement;

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
            _manager = new LocationManager(new LocationRepository(context));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _manager.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var location = await _manager.GetByIdAsync(id);
            if (location == null) return NotFound();
            return Ok(location);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Location location)
        {
            var created = await _manager.AddAsync(location);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Location location)
        {
            location.Id = id;
            var updated = await _manager.UpdateAsync(location);
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