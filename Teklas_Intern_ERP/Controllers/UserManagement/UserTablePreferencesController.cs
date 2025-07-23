using Microsoft.AspNetCore.Mvc;

namespace Teklas_Intern_ERP.Controllers.UserManagement;

public class UserTablePreferencesController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}