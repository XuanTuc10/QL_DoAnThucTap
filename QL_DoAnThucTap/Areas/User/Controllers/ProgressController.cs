using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QL_DoAnThucTap.DataContexts;
using QL_DoAnThucTap.Models.EF;
using X.PagedList;

namespace QL_DoAnThucTap.Areas.User.Controllers
{
    // GET: Admin/Home
    [Area("User")]
    [Authorize]
    public class ProgressController : Controller
    {
        AppDbContext _dbContext = new AppDbContext();
        public IActionResult Index(int project, int? page)
        {
            var pageSize = 6;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<Progress> items = _dbContext.progresses.Include(x=>x.Project).Where(x => x.ProjectId == project).OrderByDescending(x => x.StartDate).ToList();
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }
    }
}
