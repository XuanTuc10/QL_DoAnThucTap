using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QL_DoAnThucTap.DataContexts;
using System.Security.Claims;

namespace QL_DoAnThucTap.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class HomeController : Controller
    {
        AppDbContext _dbcontext = new AppDbContext();
        // GET: Admin/Home
        public IActionResult Index()
        {
            return View();
        }
    }
}