using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QL_DoAnThucTap.DataContexts;
using QL_DoAnThucTap.Handler;
using QL_DoAnThucTap.Models.EF;
using X.PagedList;

namespace QL_DoAnThucTap.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AccountController : Controller
    {
        private AppDbContext _dbContext = new AppDbContext();

        [Authorize(Roles = "001")]
        public IActionResult Index(string Searchtext, int? page)
        {
            var pageSize = 5;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<Account> items = _dbContext.accounts.OrderByDescending(x => x.Id);
            if (!string.IsNullOrEmpty(Searchtext))
            {
                items = items.Where(x => x.Email.Contains(Searchtext, StringComparison.OrdinalIgnoreCase));
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }
        [Authorize(Roles = "001")]
        public IActionResult Create()
        {
            var lstRole = _dbContext.roles.ToList();
            ViewBag.Roles = new SelectList(lstRole , "Id", "Name");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "001")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Account model)
        {
            if (string.IsNullOrWhiteSpace(model.Password)
             || string.IsNullOrWhiteSpace(model.Email)
                )
            {
                var errorMessage = "Vui lòng điền đầy đủ thông tin!";
                return Json(new { success = false, error = errorMessage });
            }
            if (!Validate.IsValidEmail(model.Email))
            {
                var errorMessage = "Không tìm thầy Email!";
                return Json(new { success = false, error = errorMessage });
            }
            if ( _dbContext.accounts.FirstOrDefault(x => x.Email.Equals(model.Email)) != null)
            {
                var errorMessage = "Email đã được sử dụng!";
                return Json(new { success = false, error = errorMessage });
            }
            var role = _dbContext.roles.SingleOrDefault(x => x.Id == model.RoleId);
            if (role == null)
            {
                var errorMessage = "Quyền người dùng không tồn tại!";
                return Json(new { success = false, error = errorMessage });
            }
            else
            {
                model.IsActive = false;
                _dbContext.accounts.Add(model);
                _dbContext.SaveChanges();
                var accountUrl = Url.Action("Index", "account");
                return Json(new { success = true, redirectUrl = accountUrl });
            }
        }

        public IActionResult ChangeAccount(int id)
        {
            var account = _dbContext.accounts.Find(id);
            var lstRole = _dbContext.roles.ToList();
            ViewBag.Roles = new SelectList(lstRole, "Id", "Name");
            return View(account);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangeAccount(Account model)
        {
            var account = _dbContext.accounts.SingleOrDefault(x => x.Id == model.Id);
            var role = _dbContext.topics.SingleOrDefault(x => x.Id == model.RoleId);
            if (role == null)
            {   
                var errorMessage = "Quyền người dùng không tồn tại!";
                return Json(new { success = false, error = errorMessage });
            }
            if (ModelState.IsValid)
            {
                _dbContext.accounts.Attach(model);
                model.Email = account.Email;
                model.IsActive = account.IsActive;  
                _dbContext.Entry(model).Property(x => x.Email).IsModified = true;
                _dbContext.Entry(model).Property(x => x.Password).IsModified = true;
                _dbContext.Entry(model).Property(x => x.IsActive).IsModified = true;
                _dbContext.Entry(model).Property(x => x.RoleId).IsModified = true;
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var account = _dbContext.accounts.Find(id);
            if (account != null)
            {
                _dbContext.accounts.Remove(account);
                _dbContext.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
        [HttpPost]
        public IActionResult DeleteAll(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var items = ids.Split(',');
                if (items != null && items.Any())
                {
                    foreach (var item in items)
                    {
                        var obj = _dbContext.accounts.Find(Convert.ToInt32(item));
                        _dbContext.accounts.Remove(obj);
                        _dbContext.SaveChanges();
                    }
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}
