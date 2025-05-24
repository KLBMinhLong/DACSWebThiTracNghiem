using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThiTracNghiem.Data;
using Microsoft.AspNetCore.Authorization;

namespace ThiTracNghiem.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> QuanLyNguoiDung(string searchString, int page = 1, int pageSize = 10)
        {
            var role = HttpContext.Session.GetString("VaiTro");
            if (role == null || role.ToLower() != "admin")
            {
                return RedirectToAction("DangNhap", "TaiKhoan");
            }

            // Lưu trạng thái tìm kiếm
            ViewBag.CurrentFilter = searchString;

            // Lấy danh sách tài khoản
            var query = _context.TaiKhoans.AsQueryable();

            // Áp dụng tìm kiếm nếu có
            if (!string.IsNullOrEmpty(searchString))
            {
                // Tìm theo tên tài khoản (chứa chuỗi tìm kiếm)
                query = query.Where(u => u.TenTaiKhoan.Contains(searchString) ||
                                         u.Email.Contains(searchString));
            }

            // Đếm tổng số bản ghi sau khi lọc
            int totalRecords = await query.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            // Lấy dữ liệu phân trang
            var users = await query
                .OrderBy(u => u.TenTaiKhoan)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Truyền thông tin phân trang cho view
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(users);
        }


        // Hiển thị form tạo tài khoản mới
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
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
            model.MatKhau = MaHoaHelper.MaHoaSHA256(model.MatKhau); // Mã hóa mật khẩu
            _context.TaiKhoans.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("QuanLyNguoiDung");
        }

        // GET: Admin/EditUser
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
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

        // GET: Admin/DetailUser
        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> DetailUser(string id)
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

            // Lấy thêm dữ liệu liên quan (số bài thi, điểm trung bình...)
            var baiThiCount = await _context.LichSuLamBais.CountAsync(kt => kt.TenTaiKhoan == id);
            var diemTB = 0.0;
            if (baiThiCount > 0)
            {
                diemTB = await _context.LichSuLamBais
                    .Where(kt => kt.TenTaiKhoan == id)
                    .AverageAsync(kt => kt.Diem) ?? 0.0;
            }

            ViewBag.SoBaiThi = baiThiCount;
            ViewBag.DiemTrungBinh = Math.Round(diemTB, 2);

            return View(user);
        }

    }
}
