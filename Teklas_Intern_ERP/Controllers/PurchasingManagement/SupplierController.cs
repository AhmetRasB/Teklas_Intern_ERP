using Microsoft.AspNetCore.Mvc;

namespace Teklas_Intern_ERP.Controllers.PurchasingManagement
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Purchasing Management")]
    public class SupplierController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll() => Ok("Supplier list");
    }
} 