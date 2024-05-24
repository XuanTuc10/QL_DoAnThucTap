using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QL_DoAnThucTap.DataContexts;
using QL_DoAnThucTap.Models.EF;
using X.PagedList;

namespace QL_DoAnThucTap.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class TopicController : Controller
    {
        private AppDbContext _dbContext = new AppDbContext();

        public IActionResult Index(string Searchtext, int? page)
        {
            var pageSize = 5;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<Topic> items = _dbContext.topics.OrderByDescending(x => x.Id);
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
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Topic model)
        {
            if (string.IsNullOrWhiteSpace(model.Name))
            {
                var errorMessage = "Vui lòng điền đầy đủ thông tin!";
                return Json(new { success = false, error = errorMessage });
            }
            else
            {
                _dbContext.topics.Add(model);
                _dbContext.SaveChanges();
                var topicUrl = Url.Action("Index", "Topic");
                return Json(new { success = true, redirectUrl = topicUrl });
            }
        }

        public IActionResult Edit(int id)
        {
            var topic = _dbContext.topics.Find(id);
            return View(topic);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Topic model)
        {
            if (string.IsNullOrWhiteSpace(model.Name))
            {
                var errorMessage = "Vui lòng điền đầy đủ thông tin!";
                return Json(new { success = false, error = errorMessage });
            }
            else
            {
                _dbContext.topics.Attach(model);
                _dbContext.Entry(model).Property(x => x.Name).IsModified = true;
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var topic = _dbContext.topics.Find(id);
            if (topic != null)
            {
                _dbContext.topics.Remove(topic);
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
                        var obj = _dbContext.topics.Find(Convert.ToInt32(item));
                        _dbContext.topics.Remove(obj);
                        _dbContext.SaveChanges();
                    }
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}
