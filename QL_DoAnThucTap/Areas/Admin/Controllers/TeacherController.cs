using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QL_DoAnThucTap.DataContexts;
using QL_DoAnThucTap.Handler;
using QL_DoAnThucTap.Models.EF;
using X.PagedList;

namespace QL_DoAnThucTap.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class TeacherController : Controller
    {
        private AppDbContext _dbContext = new AppDbContext();

        public IActionResult Index(string Searchtext, int? page)
        {
            var pageSize = 5;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<Teacher> items = _dbContext.teachers.OrderByDescending(x => x.Id);
            if (!string.IsNullOrEmpty(Searchtext))
            {
                items = items.Where(x => x.Name.Contains(Searchtext, StringComparison.OrdinalIgnoreCase));
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }

        public IActionResult Add()
        {
            var lstaccount = _dbContext.accounts.Where(x => x.Role.Code == "002" && x.IsActive == false).ToList();
            ViewBag.Accounts = new SelectList(lstaccount, "Id", "Email");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Teacher model)
        {
            if (string.IsNullOrWhiteSpace(model.Name)
             || string.IsNullOrWhiteSpace(model.Birthday.ToString())
             || string.IsNullOrWhiteSpace(model.Address)
             || string.IsNullOrWhiteSpace(model.Phone)
                )
            {
                var errorMessage = "Vui lòng điền đầy đủ thông tin!";
                return Json(new { success = false, error = errorMessage });
            }
            if (!Validate.IsValidPhoneNumber(model.Phone))
            {
                var errorMessage = "Số điện thoại không chính xác!";
                return Json(new { success = false, error = errorMessage });
            }
            if (_dbContext.teachers.FirstOrDefault(x => x.Phone.Equals(model.Phone)) != null)
            {
                var errorMessage = "Số điện thoại đã được sử dụng!";
                return Json(new { success = false, error = errorMessage });
            }
            var account = _dbContext.accounts.SingleOrDefault(x => x.Id == model.AccountId);
            if (account == null)
            {
                var errorMessage = "Tài khoản không tồn tại!";
                return Json(new { success = false, error = errorMessage });
            }
            else
            {
                account.IsActive = true;
                _dbContext.accounts.Update(account);
                _dbContext.SaveChanges();
                model.Code = account.Email.Substring(0, account.Email.IndexOf('@'));
                _dbContext.teachers.Add(model);
                _dbContext.SaveChanges();
                var TeacherUrl = Url.Action("Index", "Teacher");
                return Json(new { success = true, redirectUrl = TeacherUrl });
            }
        }

        public IActionResult Edit(int id)
        {
            var teacher = _dbContext.teachers.Find(id);
            var lstaccount = _dbContext.accounts.Where(x => x.Role.Code == "002").ToList();
            ViewBag.Accounts = new SelectList(lstaccount, "Id", "Email");
            return View(teacher);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Teacher model)
        {
            if (string.IsNullOrWhiteSpace(model.Name)
             || string.IsNullOrWhiteSpace(model.Birthday.ToString())
             || string.IsNullOrWhiteSpace(model.Address)
             || string.IsNullOrWhiteSpace(model.Phone)
                )
            {
                var errorMessage = "Vui lòng điền đầy đủ thông tin!";
                return Json(new { success = false, error = errorMessage });
            }
            if (!Validate.IsValidPhoneNumber(model.Phone))
            {
                var errorMessage = "Số điện thoại không chính xác!";
                return Json(new { success = false, error = errorMessage });
            }
            if (_dbContext.teachers.FirstOrDefault(x => x.Phone.Equals(model.Phone) && x.Id != model.Id) != null)
            {
                var errorMessage = "Số điện thoại đã được sử dụng!";
                return Json(new { success = false, error = errorMessage });
            }
            var account = _dbContext.accounts.SingleOrDefault(x => x.Id == model.AccountId);
            if (account == null)
            {
                return View(model);
            }
            else
            {
                _dbContext.teachers.Attach(model);
                model.Code = account.Email.Substring(0, account.Email.IndexOf('@'));
                _dbContext.Entry(model).Property(x => x.Code).IsModified = true;
                _dbContext.Entry(model).Property(x => x.Name).IsModified = true;
                _dbContext.Entry(model).Property(x => x.Birthday).IsModified = true;
                _dbContext.Entry(model).Property(x => x.Gender).IsModified = true;
                _dbContext.Entry(model).Property(x => x.Address).IsModified = true;
                _dbContext.Entry(model).Property(x => x.Phone).IsModified = true;
                _dbContext.Entry(model).Property(x => x.AccountId).IsModified = true;
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var teacher = _dbContext.teachers.Find(id);
            if (teacher != null)
            {
                _dbContext.teachers.Remove(teacher);
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
                        var obj = _dbContext.teachers.Find(Convert.ToInt32(item));
                        _dbContext.teachers.Remove(obj);
                        _dbContext.SaveChanges();
                    }
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}
