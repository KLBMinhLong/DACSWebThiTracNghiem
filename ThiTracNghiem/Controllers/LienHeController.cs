
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThiTracNghiem.Data;
using ThiTracNghiem.Models;

namespace ThiTracNghiem.Controllers
{
    public class LienHeController : Controller
    {
        private readonly AppDbContext _context;

        public LienHeController(AppDbContext context)
        {
            _context = context;
        }

        // Giao diện người dùng gửi liên hệ
        public IActionResult GuiLienHe()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GuiLienHe(string HoTen, string Email, string NoiDung)
        {
            var user = HttpContext.Session.GetString("UserName") ?? "Khách";

            var lh = new LienHe
            {
                TenTaiKhoan = user,
                HoTen = HoTen,
                Email = Email,
                NoiDung = NoiDung,
                NgayGui = DateTime.Now,
                TrangThaiXuLy = "Chưa xử lý"
            };

            _context.LienHes.Add(lh);
            _context.SaveChanges();

            TempData["ThongBao"] = "Gửi liên hệ thành công. Cảm ơn bạn!";
            return RedirectToAction("GuiLienHe");
        }

        // Giao diện quản lý liên hệ
        [Authorize(Roles = "admin")]
        public IActionResult QuanLyLienHe(string trangThai, DateTime? tuNgay, DateTime? denNgay)
        {
            var ds = _context.LienHes.AsQueryable();

            if (!string.IsNullOrEmpty(trangThai))
                ds = ds.Where(l => l.TrangThaiXuLy == trangThai);

            if (tuNgay.HasValue)
                ds = ds.Where(l => l.NgayGui >= tuNgay.Value.Date);

            if (denNgay.HasValue)
                ds = ds.Where(l => l.NgayGui <= denNgay.Value.Date.AddDays(1));

            return View(ds.OrderByDescending(l => l.NgayGui).ToList());
        }

        // Đánh dấu đã xử lý
        [Authorize(Roles = "admin")]
        public IActionResult DoiTrangThai(int id)
        {
            var lh = _context.LienHes.Find(id);
            if (lh != null)
            {
                lh.TrangThaiXuLy = (lh.TrangThaiXuLy == "Chưa xử lý") ? "Đã xử lý" : "Chưa xử lý";
                _context.SaveChanges();
            }
            return RedirectToAction("QuanLyLienHe");
        }
    }
}