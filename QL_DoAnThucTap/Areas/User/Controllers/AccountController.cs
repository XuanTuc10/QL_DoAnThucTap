using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QL_DoAnThucTap.DataContexts;

namespace QL_DoAnThucTap.Areas.User.Controllers
{
    [Area("User")]
    [Authorize]
    public class AccountController : Controller
    {
        private AppDbContext _dbContext = new AppDbContext();
        public IActionResult Index()
        {
            var account = _dbContext.accounts.FirstOrDefault(x=>x.Email == User.Identity.Name);
            if (User.IsInRole("003"))
            {
                account = _dbContext.accounts.Include(x => x.Student).FirstOrDefault(x => x.Email == User.Identity.Name);
            }
            else
            {
                account = _dbContext.accounts.Include(x => x.Teacher).FirstOrDefault(x => x.Email == User.Identity.Name);
            }
                return View(account);
        }
    }
}
