using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using QL_DoAnThucTap.DataContexts;
using QL_DoAnThucTap.Models.EF;
using X.PagedList;

namespace QL_DoAnThucTap.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ProgressController : Controller
    {
        AppDbContext _dbContext = new AppDbContext();
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProgressController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index(int project, int? page, string status = "all")
        {
            var pageSize = 6;
            if (page == null)
            {
                page = 1;
            }

            IQueryable<Progress> query = _dbContext.progresses.Where(x => x.ProjectId == project);

            // Lọc dữ liệu dựa trên trạng thái
            if (status == "completed")
            {
                query = query.Where(x => x.IsActive);
            }
            else if (status == "notCompleted")
            {
                query = query.Where(x => !x.IsActive);
            }

            var items = query.OrderByDescending(x => x.StartDate).ToPagedList(page ?? 1, pageSize);

            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            ViewBag.Status = status;
            return View(items);
        }
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(int projectId, Progress model)
        {
            model.ProjectId = projectId;
            _dbContext.progresses.Add(model);
            _dbContext.SaveChanges();
            var projectUrl = Url.Action("Index", "Progress", new { project = projectId });
            return Json(new { success = true, redirectUrl = projectUrl });
        }
        public IActionResult Edit(int id)
        {
            var progress = _dbContext.progresses.Find(id);
            return View(progress);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Progress model)
        {
            var progress = _dbContext.progresses.SingleOrDefault(x => x.Id == model.Id);
            if(progress == null)
            {
                var errorMessage = "Tiến trình không tồn tại!";
                return Json(new { success = false, error = errorMessage });
            }
            progress.Title = model.Title;
            progress.StartDate = model.StartDate;
            progress.EndDate = model.EndDate;
            _dbContext.progresses.Update(progress);
            _dbContext.SaveChanges();
            var projectUrl = Url.Action("Index", "Progress", new { project = model.ProjectId });
            return Json(new { success = true, redirectUrl = projectUrl });

        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var progress = _dbContext.progresses.Find(id);
            if (progress != null)
            {
                var details = _dbContext.details.Where(x => x.ProgressId == progress.Id).ToList();
                if (details != null)
                {
                    foreach (var detail in details)
                    {
                        if (detail != null)
                        {
                            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, detail.FileUrl);
                            FileInfo file = new FileInfo(filePath);
                            if (file.Exists)
                            {
                                file.Delete();
                            }
                        }
                    }
                }
                _dbContext.details.RemoveRange(details);
                _dbContext.SaveChanges();
                _dbContext.progresses.Remove(progress);
                _dbContext.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}
