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
    public class StudentController : Controller
    {
        private AppDbContext _dbContext = new AppDbContext();

        public IActionResult Index(string Searchtext, int? page)
        {
            var pageSize = 5;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<Student> items = _dbContext.students.OrderByDescending(x => x.Id);
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
            var lstaccount = _dbContext.accounts.Where(x=>x.Role.Code == "003" && x.IsActive == false).ToList();
            var lstclass = _dbContext.classes.ToList();
            ViewBag.Accounts = new SelectList(lstaccount, "Id", "Email");
            ViewBag.Classes = new SelectList(lstclass, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Student model)
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
            if (_dbContext.students.FirstOrDefault(x => x.Phone.Equals(model.Phone)) != null)
            {
                var errorMessage = "Số điện thoại đã được sử dụng!";
                return Json(new { success = false, error = errorMessage });
            }
            var account = _dbContext.accounts.SingleOrDefault(x => x.Id == model.AccountId);
            var classes = _dbContext.classes.SingleOrDefault(x => x.Id == model.ClassId);
            if (account == null)
            {
                var errorMessage = "Tài khoản không tồn tại!";
                return Json(new { success = false, error = errorMessage });
            }
            if (classes == null)
            {
                var errorMessage = "Lớp không tồn tại!";
                return Json(new { success = false, error = errorMessage });
            }
            else
            {
                account.IsActive = true;
                _dbContext.accounts.Update(account);
                _dbContext.SaveChanges();
                model.Code = account.Email.Substring(0, account.Email.IndexOf('@'));
                _dbContext.students.Add(model);
                _dbContext.SaveChanges();
                var BaiVietUrl = Url.Action("Index", "Student");
                return Json(new { success = true, redirectUrl = BaiVietUrl });
            }
        }

        public IActionResult Edit(int id)
        {
            var student = _dbContext.students.Find(id);
            var lstaccount = _dbContext.accounts.Where(x => x.Role.Code == "003").ToList();
            var lstclass = _dbContext.classes.ToList();
            ViewBag.Accounts = new SelectList(lstaccount, "Id", "Email");
            ViewBag.Classes = new SelectList(lstclass, "Id", "Name");
            return View(student);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Student model)
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
            if (_dbContext.students.FirstOrDefault(x => x.Phone.Equals(model.Phone) && x.Id != model.Id) != null)
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
                _dbContext.students.Attach(model);
                model.Code = account.Email.Substring(0, account.Email.IndexOf('@'));
                _dbContext.Entry(model).Property(x => x.Name).IsModified = true;
                _dbContext.Entry(model).Property(x => x.Code).IsModified = true;
                _dbContext.Entry(model).Property(x => x.Birthday).IsModified = true;
                _dbContext.Entry(model).Property(x => x.Gender).IsModified = true;
                _dbContext.Entry(model).Property(x => x.Address).IsModified = true;
                _dbContext.Entry(model).Property(x => x.Phone).IsModified = true;
                _dbContext.Entry(model).Property(x => x.ClassId).IsModified = true;
                _dbContext.Entry(model).Property(x => x.AccountId).IsModified = true;
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var student = _dbContext.students.Find(id);
            if (student != null)
            {
                _dbContext.students.Remove(student);
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
                        var obj = _dbContext.students.Find(Convert.ToInt32(item));
                        _dbContext.students.Remove(obj);
                        _dbContext.SaveChanges();
                    }
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}
