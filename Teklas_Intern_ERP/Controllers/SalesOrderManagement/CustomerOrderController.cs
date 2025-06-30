using Microsoft.AspNetCore.Mvc;

namespace Teklas_Intern_ERP.Controllers.SalesOrderManagement
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Sales & Order Management")]
    public class CustomerOrderController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll() => Ok("CustomerOrder list");
    }
} 