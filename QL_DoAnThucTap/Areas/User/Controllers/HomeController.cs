using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using QL_DoAnThucTap.DataContexts;
using Microsoft.EntityFrameworkCore;
using QL_DoAnThucTap.Models.EF;
using X.PagedList;
namespace QL_DoAnThucTap.Areas.User.Controllers
{
    // GET: Admin/Home
    [Area("User")]
    [Authorize]
    public class HomeController : Controller
    {
        AppDbContext _dbContext = new AppDbContext();
        public IActionResult Index(int? page)
        {
            var pageSize = 6;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<Reminder> items = _dbContext.reminders.OrderByDescending(x => x.Createtime).ToList();
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }

        public IActionResult Privacy()
        {
            return View();
        }

    }
}