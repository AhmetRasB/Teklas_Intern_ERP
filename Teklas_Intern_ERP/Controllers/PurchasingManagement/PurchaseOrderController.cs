using Microsoft.AspNetCore.Mvc;

namespace Teklas_Intern_ERP.Controllers.PurchasingManagement
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Purchasing Management")]
    public class PurchaseOrderController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll() => Ok("PurchaseOrder list");
    }
} 