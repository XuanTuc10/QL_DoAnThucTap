using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using QL_DoAnThucTap.Models;
using QL_DoAnThucTap.DataContexts;
using System.Security.Claims;
using QL_DoAnThucTap.Models.EF;
using Microsoft.AspNetCore.Authorization;
namespace QL_DoAnThucTap.Controllers
{
    public class AccessController : Controller
    {
        AppDbContext _dbcontext = new AppDbContext();
        [AllowAnonymous]
        public IActionResult Signin()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Signin(string tenDangNhap, string matKhau)
        {
            var account = _dbcontext.accounts.FirstOrDefault(x => x.Email == tenDangNhap && x.Password == matKhau);
            if (account != null)
            {
                var decentralization = _dbcontext.roles.FirstOrDefault(x => x.Id == account.RoleId);
                var identity = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, account.Email),
                new Claim(ClaimTypes.Role, decentralization?.Code ?? ""),
                new Claim("AccountId", account.Id.ToString())
                }, "signin");
                var princial = new ClaimsPrincipal(identity);
                var singin = HttpContext.SignInAsync(princial);
                return RedirectToAction("Index", "Home", new { area = "User" });
            }
            else
            {
                ViewBag.error = "<div class='alert alert-danger'> Đăng nhập thất bại</div>";
            }
            return View();
        }   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Signin", "Access");
        }
    }
}
