using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace QL_DoAnThucTap.Areas.User.Controllers
{
    [Area("User")]
    [Authorize]
    public class StudentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
