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
        public IActionResult GetAll() => Ok(_manager.GetAll());

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var result = _manager.GetById(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public IActionResult Add(Customer customer)
        {
            var result = _manager.Add(customer);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut]
        public IActionResult Update(Customer customer)
        {
            var result = _manager.Update(customer);
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