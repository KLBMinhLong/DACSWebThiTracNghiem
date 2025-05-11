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

        //  Action để quản lý user
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
            model.MatKhau=MaHoaHelper.MaHoaSHA256(model.MatKhau); // Mã hóa mật khẩu
            _context.TaiKhoans.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("QuanLyNguoiDung");
        }
        // GET: Admin/EditUser
        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var role = HttpContext.Session.GetString("VaiTro");
            if (role == null || role.ToLower() != "admin")
            {
                return RedirectToAction("DangNhap", "TaiKhoan");
            }

            var user = await _context.TaiKhoans.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Admin/EditUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(ThiTracNghiem.Models.TaiKhoan model)
        {
            var role = HttpContext.Session.GetString("VaiTro");
            if (role == null || role.ToLower() != "admin")
            {
                return RedirectToAction("DangNhap", "TaiKhoan");
            }

            // if (!ModelState.IsValid)
            // {
            //     return View(model);
            // }

            var user = await _context.TaiKhoans.FindAsync(model.TenTaiKhoan);
            if (user == null)
            {
                return NotFound();
            }

            user.Email = model.Email;
            user.GioiTinh = model.GioiTinh;
            user.SoDienThoai = model.SoDienThoai;
            user.VaiTro = model.VaiTro;
            user.TrangThaiKhoa = model.TrangThaiKhoa;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Cập nhật người dùng thành công!";

            return RedirectToAction("QuanLyNguoiDung");
        }
        // GET: Admin/DeleteUser
        [HttpGet]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var role = HttpContext.Session.GetString("VaiTro");
            if (role == null || role.ToLower() != "admin")
            {
                return RedirectToAction("DangNhap", "TaiKhoan");
            }

            var user = await _context.TaiKhoans.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.TaiKhoans.Remove(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("QuanLyNguoiDung");
        }



    }
}
