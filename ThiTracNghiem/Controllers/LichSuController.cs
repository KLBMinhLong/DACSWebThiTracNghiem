using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThiTracNghiem.Data;

namespace ThiTracNghiem.Controllers
{
    public class LichSuController : Controller
    {
        private readonly AppDbContext _context;

        public LichSuController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
           var tenDangNhap = HttpContext.Session.GetString("TenTaiKhoan");

            // if (string.IsNullOrEmpty(username))
            //     return RedirectToAction("DangNhap", "TaiKhoan");

            var lichSu = _context.LichSuLamBais
                .Where(l => l.TenTaiKhoan == tenDangNhap)
                .OrderByDescending(l => l.NgayNopBai)
                .ToList();


            return View(lichSu);
        }
    }
}
