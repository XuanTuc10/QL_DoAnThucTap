using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QL_DoAnThucTap.DataContexts;
using QL_DoAnThucTap.Models.EF;
using X.PagedList;

namespace QL_DoAnThucTap.Areas.User.Controllers
{
    public class ReminderController : Controller
    {
        AppDbContext _dbContext = new AppDbContext();
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ReminderController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index(int id)
        {
            IEnumerable<Reminder> items = _dbContext.reminders.Where(x=>x.Id == id);
            return View(items);
        }
    }
}
