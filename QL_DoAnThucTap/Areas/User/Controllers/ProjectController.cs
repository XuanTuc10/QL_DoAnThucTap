using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QL_DoAnThucTap.DataContexts;
using QL_DoAnThucTap.Models.EF;
using X.PagedList;

namespace QL_DoAnThucTap.Areas.User.Controllers
{
    // GET: Admin/Home
    [Area("User")]
    [Authorize]
    public class ProjectController : Controller
    {
        AppDbContext _dbcontext = new AppDbContext();
        public IActionResult Index(int? page)
        {
            var pageSize = 5;
            if (page == null)
            {
                page = 1;
            }
            var code = User.Identity.Name.Substring(0, User.Identity.Name.IndexOf('@'));
            IEnumerable<Project> project = _dbcontext.projects
                            .Include(x => x.Teacher)
                            .Include(x => x.Student)
                            .Include(x => x.Topic)
                            .Include(x => x.Progresses)
                            .ThenInclude(x => x.Details)
                            .OrderByDescending(x => x.CreateDate).ToList();
            if (User.IsInRole("002"))
            {
                var teacher = _dbcontext.teachers.SingleOrDefault(x=>x.Code == code);
                project = _dbcontext.projects
                            .Include(x => x.Teacher)
                            .Include(x => x.Topic)
                            .Include(x => x.Progresses)
                            .ThenInclude(x => x.Details)
                            .Where(x=>x.Teacher.Id == teacher.Id)
                            .OrderByDescending(x => x.CreateDate)
                            .ToList();
                return View(project);
            }
            if (User.IsInRole("003"))
            {
                var student = _dbcontext.students.SingleOrDefault(x => x.Code == code);
                project = _dbcontext.projects
                            .Include(x => x.Student)
                            .Include(x => x.Topic)
                            .Include(x => x.Progresses)
                            .ThenInclude(x => x.Details)
                            .Where(x=>x.Student.Id == student.Id)
                            .OrderByDescending(x => x.CreateDate)
                            .ToList();
                return View(project);
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            project = project.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(project);
        }
        public ActionResult MenuTopics()
        {
            var items = _dbcontext.topics.ToList();
            return PartialView("MenuTopics", items);
        }
        public ActionResult Partial_ItemByCateID(int? id, int? page)
        {
            var pageSize = 5;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<Project> project = _dbcontext.projects.OrderBy(x => x.CreateDate).ToList();
            if (id != null)
            {
                project = project.Where(x => x.TopicId == id).ToList();
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            project = project.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return PartialView(project);
        }
    }
}
