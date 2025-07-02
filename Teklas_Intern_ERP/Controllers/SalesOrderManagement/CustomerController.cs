using Microsoft.AspNetCore.Mvc;
using Teklas_Intern_ERP.Business.SalesOrderManagement;
using Teklas_Intern_ERP.Entities.SalesOrderManagement;

namespace Teklas_Intern_ERP.Controllers.SalesOrderManagement
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerManager _manager;
        public CustomerController(CustomerManager manager)
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
        public async Task<IActionResult> Add(Customer customer)
        {
            var result = await _manager.AddAsync(customer);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(Customer customer)
        {
            var result = await _manager.UpdateAsync(customer);
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