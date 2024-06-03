using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QL_DoAnThucTap.DataContexts;
using QL_DoAnThucTap.Handler;
using QL_DoAnThucTap.Models.EF;

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
        [HttpPost]
        public IActionResult ChangePassword(int id, string currentPassword, string newPassword)
        {
            var account = _dbContext.accounts.SingleOrDefault(x => x.Id == id);
            if(!Validate.IsValidPassWord(newPassword))
            {
                return Json(new { success = false, error = "Mật khẩu phải từ 8 đến 16 ký tự, bao gồm ít nhất một chữ hoa và một ký tự đặc biệt." });
            }
            if (account.Password != currentPassword)
            {
                return Json(new { success = false, error = "Mật khẩu hiện tại không chính xác" });
            }
            account.Password = newPassword;
            _dbContext.accounts.Update(account);
            _dbContext.SaveChanges();
            var projectUrl = Url.Action("Index", "Account", new { account });
            return Json(new { success = true, redirectUrl = projectUrl });
        }
    }
}
