using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QL_DoAnThucTap.DataContexts;
using QL_DoAnThucTap.Models;
using QL_DoAnThucTap.Models.EF;
using System.Reflection.Metadata;
using System.Security.Principal;
using X.PagedList;
using static NuGet.Packaging.PackagingConstants;

namespace QL_DoAnThucTap.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class DetailController : Controller
    {
        AppDbContext _dbContext = new AppDbContext();
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DetailController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index(int progress, string Searchtext, int? page)
        {
            var pageSize = 5;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<Detail> details = _dbContext.details
                                            .Include(d => d.Progress)
                                            .Include(d=>d.Progress.Project)
                                            .Include(d => d.Progress.Feedbacks)
                                            .Include(d => d.Progress.Project.Student)
                                            .Include(d => d.Progress.Project.Teacher)
                                            .Where(d => d.ProgressId == progress)
                                            .OrderByDescending(d => d.Id);
            if (!string.IsNullOrEmpty(Searchtext))
            {
                details = details.Where(x => x.Progress.Project.Student.Name.Contains(Searchtext, StringComparison.OrdinalIgnoreCase));
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            details = details.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(details);
        }
        [HttpPost]
        public IActionResult Add(int id, List<IFormFile> filesArray)
        {
            var progress = _dbContext.progresses.Include(p => p.Project).SingleOrDefault(x => x.Id == id);
            List<Detail> lstDetail = new List<Detail>();
            foreach (var file in filesArray)
            {
                if (file != null && file.Length > 0)
                {
                    string folderPath = "Data/" + progress.Project.Name + "/" + progress.Title + "/";
                    Detail detail = new Detail
                    {

                        FileUrl = UploadFile(folderPath, file),
                        CreateDate = DateTime.Now,
                        IsActive = true,
                        ProgressId = id
                    };
                    lstDetail.Add(detail);
                }
            }
            _dbContext.details.AddRange(lstDetail);
            progress.IsActive = true;
            _dbContext.progresses.Update(progress);
            _dbContext.SaveChanges();
            var detailUrl = Url.Action("Index", "Detail", new { progress = id });
            return Json(new { success = true, redirectUrl = detailUrl });
        }

        private string UploadFile(string folderPath, IFormFile file)
        {
            string fileName = file.FileName;
            string extension = Path.GetExtension(fileName);
            string name = Path.GetFileNameWithoutExtension(fileName);

            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, folderPath, fileName);
            int count = 1;
            while (System.IO.File.Exists(filePath))
            {
                fileName = $"{name} ({count++}){extension}";
                filePath = Path.Combine(_webHostEnvironment.WebRootPath, folderPath, fileName);
            }

            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            return Path.Combine(folderPath, fileName);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var detail = _dbContext.details.Find(id);
            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, detail.FileUrl);
            FileInfo file = new FileInfo(filePath);
            if (detail != null && file.Exists)
            {
                _dbContext.details.Remove(detail);
                _dbContext.SaveChanges();
                file.Delete();
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
                        var obj = _dbContext.details.Find(Convert.ToInt32(item));
                        _dbContext.details.Remove(obj);
                        _dbContext.SaveChanges();
                    }
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
        [HttpPost]
        public IActionResult FeedBack(int id, string commentText)
        {
            if (!string.IsNullOrEmpty(commentText))
            {
                var newFeedback = new Feedback
                {
                    ProgressId = id,
                    FeedBack = commentText,
                    Createtime = DateTime.Now
                };

                try
                {
                    _dbContext.feedbacks.Add(newFeedback);
                    _dbContext.SaveChanges();
                    var detailUrl = Url.Action("Index", "Detail", new { progress = id });
                    return Json(new { success = true, redirectUrl = detailUrl });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, error = ex.Message });
                }
            }
            else
            {
                return Json(new { success = false, error = "Comment is empty." });
            }
        }
    }
}
