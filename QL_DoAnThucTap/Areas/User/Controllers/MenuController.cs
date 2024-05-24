using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QL_DoAnThucTap.DataContexts;

namespace QL_DoAnThucTap.Areas.User.Controllers
{
    public class MenuController : Controller
    {
        private AppDbContext _dbContext = new AppDbContext();
        public IActionResult Index()
        {
            return View();
        }
        public ActionResult MenuTopics()
        {
            var items = _dbContext.topics.ToList();
            return PartialView("_MenuTopics", items);
        }
    }
}
