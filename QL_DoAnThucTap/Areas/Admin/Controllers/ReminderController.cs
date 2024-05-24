using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QL_DoAnThucTap.DataContexts;
using QL_DoAnThucTap.Models.EF;
using X.PagedList;

namespace QL_DoAnThucTap.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ReminderController : Controller
    {
        private AppDbContext _dbContext = new AppDbContext();
        public IActionResult Index(string Searchtext, int? page)
        {
            var pageSize = 5;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<Reminder> items = _dbContext.reminders.Include(x => x.Teacher).OrderByDescending(x => x.Id);
            if (!string.IsNullOrEmpty(Searchtext))
            {
                items = items.Where(x => x.Title.Contains(Searchtext, StringComparison.OrdinalIgnoreCase));
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }
        public IActionResult Add()
        {
            var lstTopic = _dbContext.topics.ToList();
            var lstStudent = _dbContext.students.ToList();
            var lstTeacher = _dbContext.teachers.ToList();
            ViewBag.Topics = new SelectList(lstTopic, "Id", "Name");
            ViewBag.Students = new SelectList(lstStudent, "Id", "Name");
            ViewBag.Teachers = new SelectList(lstTeacher, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Project model)
        {
            var topic = _dbContext.topics.SingleOrDefault(x => x.Id == model.TopicId);
            var student = _dbContext.students.SingleOrDefault(x => x.Id == model.StudentId);
            var teacher = _dbContext.teachers.SingleOrDefault(x => x.Id == model.TeacherId);
            if (topic == null)
            {
                var errorMessage = "Tài khoản không tồn tại!";
                return Json(new { success = false, error = errorMessage });
            }
            if (student == null)
            {
                var errorMessage = "Học sinh không tồn tại!";
                return Json(new { success = false, error = errorMessage });
            }
            if (teacher == null)
            {
                var errorMessage = "Giáo viên không tồn tại!";
                return Json(new { success = false, error = errorMessage });
            }
            else if (ModelState.IsValid)
            {
                model.CreateDate = DateTime.Now;
                model.UpdateDate = DateTime.Now;
                _dbContext.projects.Add(model);
                _dbContext.SaveChanges();
                var projectUrl = Url.Action("Index", "project");
                return Json(new { success = true, redirectUrl = projectUrl });
            }
            var addUrl = Url.Action("Add", "project");
            return Json(new { success = true, redirectUrl = addUrl });
        }

        public IActionResult Edit(int id)
        {
            var project = _dbContext.projects.Find(id);
            var lstTopic = _dbContext.topics.ToList();
            var lstStudent = _dbContext.students.ToList();
            var lstTeacher = _dbContext.teachers.ToList();
            ViewBag.Topics = new SelectList(lstTopic, "Id", "Name");
            ViewBag.Students = new SelectList(lstStudent, "Id", "Name");
            ViewBag.Teachers = new SelectList(lstTeacher, "Id", "Name");
            return View(project);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Project model)
        {
            var project = _dbContext.projects.SingleOrDefault(x => x.Id == model.Id);
            var topic = _dbContext.topics.SingleOrDefault(x => x.Id == model.TopicId);
            var student = _dbContext.students.SingleOrDefault(x => x.Id == model.StudentId);
            var teacher = _dbContext.teachers.SingleOrDefault(x => x.Id == model.TeacherId);
            if (topic == null)
            {
                var errorMessage = "Tài khoản không tồn tại!";
                return Json(new { success = false, error = errorMessage });
            }
            if (student == null)
            {
                var errorMessage = "Học sinh không tồn tại!";
                return Json(new { success = false, error = errorMessage });
            }
            if (teacher == null)
            {
                var errorMessage = "Giáo viên không tồn tại!";
                return Json(new { success = false, error = errorMessage });
            }
            else if (project != null)
            {
                project.Name = model.Name;
                project.UpdateDate = DateTime.Now;
                project.TopicId = model.TopicId;
                project.TeacherId = model.TeacherId;
                project.StudentId = model.StudentId;
                _dbContext.projects.Update(project);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var project = _dbContext.projects.Find(id);
            if (project != null)
            {
                _dbContext.projects.Remove(project);
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
                        var obj = _dbContext.projects.Find(Convert.ToInt32(item));
                        _dbContext.projects.Remove(obj);
                        _dbContext.SaveChanges();
                    }
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}
