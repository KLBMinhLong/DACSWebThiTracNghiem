using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThiTracNghiem.Data;

namespace ThiTracNghiem.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {
            var role = HttpContext.Session.GetString("VaiTro");
            if (role == null || role.ToLower() != "admin")
            {
                return RedirectToAction("DangNhap", "TaiKhoan");
            }

            return View();
        }

        // ✅ Action mới để quản lý user
        public async Task<IActionResult> QuanLyNguoiDung()
        {
            var role = HttpContext.Session.GetString("VaiTro");
            if (role == null || role.ToLower() != "admin")
            {
                return RedirectToAction("DangNhap", "TaiKhoan");
            }

            var users = await _context.TaiKhoans.ToListAsync();
            return View(users);
        }
        // Hiển thị form tạo tài khoản mới
[HttpGet]
public IActionResult CreateUser()
{
    var role = HttpContext.Session.GetString("VaiTro");
    if (role == null || role.ToLower() != "admin")
    {
        return RedirectToAction("DangNhap", "TaiKhoan");
    }

    return View();
}

// Xử lý khi admin submit form
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> CreateUser(ThiTracNghiem.Models.TaiKhoan model)
{
    var role = HttpContext.Session.GetString("VaiTro");
    if (role == null || role.ToLower() != "admin")
    {
        return RedirectToAction("DangNhap", "TaiKhoan");
    }

    if (!ModelState.IsValid)
    {
        return View(model);
    }

    // Kiểm tra trùng tên tài khoản
    if (_context.TaiKhoans.Any(t => t.TenTaiKhoan == model.TenTaiKhoan))
    {
        ModelState.AddModelError("TenTaiKhoan", "Tên tài khoản đã tồn tại");
        return View(model);
    }

    // Kiểm tra trùng email
    if (_context.TaiKhoans.Any(t => t.Email == model.Email))
    {
        ModelState.AddModelError("Email", "Email đã được sử dụng");
        return View(model);
    }

    model.ThoiGianTao = DateTime.Now;

    _context.TaiKhoans.Add(model);
    await _context.SaveChangesAsync();

    return RedirectToAction("QuanLyNguoiDung");
}

    }
}
