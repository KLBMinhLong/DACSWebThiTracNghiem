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
            var user = await _context.TaiKhoans.FindAsync(model.TenTaiKhoan);
            if (user == null)
            {
                return NotFound();
            }

            var role = HttpContext.Session.GetString("VaiTro");
            if (role == null || role.ToLower() != "admin")
            {
                return RedirectToAction("DangNhap", "TaiKhoan");
            }

            // Kiểm tra nếu đang thay đổi vai trò từ admin sang user
            if (user.VaiTro == "admin" && model.VaiTro != "admin")
            {
                var adminCount = await _context.TaiKhoans.CountAsync(u => u.VaiTro == "admin" && u.TenTaiKhoan != model.TenTaiKhoan);
                if (adminCount < 1)
                {
                    TempData["ErrorMessage"] = "Không thể thay đổi vai trò! Hệ thống phải có ít nhất 1 admin.";
                    return View(model);
                }
            }

            // Kiểm tra nếu đang khóa admin
            if (user.VaiTro == "admin" && model.TrangThaiKhoa)
            {
                // Đếm số admin không bị khóa (không bao gồm user hiện tại)
                var activeAdminCount = await _context.TaiKhoans.CountAsync(u =>
                    u.VaiTro == "admin" &&
                    !u.TrangThaiKhoa &&
                    u.TenTaiKhoan != model.TenTaiKhoan);

                if (activeAdminCount < 1)
                {
                    TempData["ErrorMessage"] = "Không thể khóa admin! Hệ thống phải có ít nhất 1 admin đang hoạt động.";
                    return View(model);
                }
            }

            // Kiểm tra nếu admin đang sửa chính tài khoản của mình
            var currentUser = User.Identity.Name;
            if (user.TenTaiKhoan == currentUser)
            {
                // Không cho admin tự thay đổi vai trò của chính mình
                if (user.VaiTro == "admin" && model.VaiTro != "admin")
                {
                    TempData["ErrorMessage"] = "Bạn không thể thay đổi vai trò của chính mình!";
                    return View(model);
                }

                // Không cho admin tự khóa tài khoản của chính mình
                if (model.TrangThaiKhoa)
                {
                    TempData["ErrorMessage"] = "Bạn không thể khóa tài khoản của chính mình!";
                    return View(model);
                }
            }

            // Cập nhật thông tin
            user.Email = model.Email;
            user.GioiTinh = model.GioiTinh;
            user.SoDienThoai = model.SoDienThoai;
            user.VaiTro = model.VaiTro;
            user.TrangThaiKhoa = model.TrangThaiKhoa;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Cập nhật người dùng thành công!";
            return RedirectToAction("QuanLyNguoiDung");
        }

        // GET: Admin/DeleteUser - Hiển thị trang xác nhận xóa
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

            // Kiểm tra không cho xóa tài khoản hiện tại
            var currentUser = User.Identity.Name;
            if (id == currentUser)
            {
                TempData["ErrorMessage"] = "Bạn không thể xóa tài khoản đang sử dụng để đăng nhập!";
                return RedirectToAction("QuanLyNguoiDung");
            }

            // Đếm số lượng dữ liệu liên quan
            var soLichSuThi = await _context.LichSuLamBais.CountAsync(l => l.TenTaiKhoan == id);
            var soBinhLuan = await _context.BinhLuans.CountAsync(b => b.TenTaiKhoan == id);

            // Truyền thông tin cho view
            ViewBag.SoLichSuThi = soLichSuThi;
            ViewBag.SoBinhLuan = soBinhLuan;
            ViewBag.TongDuLieuLienQuan = soLichSuThi + soBinhLuan;

            return View(user);
        }

        // POST: Admin/DeleteUser - Xử lý xóa thực tế
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUserConfirmed(string id)
        {
            var role = HttpContext.Session.GetString("VaiTro");
            if (role == null || role.ToLower() != "admin")
            {
                return RedirectToAction("DangNhap", "TaiKhoan");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var user = await _context.TaiKhoans.FindAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                // Kiểm tra không cho xóa tài khoản hiện tại
                var currentUser = User.Identity.Name;
                if (id == currentUser)
                {
                    TempData["ErrorMessage"] = "Bạn không thể xóa tài khoản đang sử dụng để đăng nhập!";
                    return RedirectToAction("QuanLyNguoiDung");
                }

                // Kiểm tra nếu đây là admin duy nhất
                var activeAdminCount = await _context.TaiKhoans.CountAsync(u => 
                    u.VaiTro == "admin" && 
                    !u.TrangThaiKhoa && 
                    u.TenTaiKhoan != id);
                    
                if (user.VaiTro == "admin" && activeAdminCount < 1)
                {
                    TempData["ErrorMessage"] = "Không thể xóa admin cuối cùng đang hoạt động trong hệ thống!";
                    return RedirectToAction("QuanLyNguoiDung");
                }

                // 1. Xóa ChiTietLamBai trước (do có FK với LichSuLamBai)
                var lichSuIds = await _context.LichSuLamBais
                    .Where(l => l.TenTaiKhoan == id)
                    .Select(l => l.Id)
                    .ToListAsync();

                if (lichSuIds.Any())
                {
                    var chiTietLamBais = _context.ChiTietLamBais
                        .Where(ct => lichSuIds.Contains(ct.LichSuLamBaiId));
                    _context.ChiTietLamBais.RemoveRange(chiTietLamBais);
                }

                // 2. Xóa LichSuLamBai
                var lichSuLamBais = _context.LichSuLamBais.Where(l => l.TenTaiKhoan == id);
                _context.LichSuLamBais.RemoveRange(lichSuLamBais);

                // 3. Xóa BinhLuan
                var binhLuans = _context.BinhLuans.Where(b => b.TenTaiKhoan == id);
                _context.BinhLuans.RemoveRange(binhLuans);

                // 4. Cuối cùng mới xóa TaiKhoan
                _context.TaiKhoans.Remove(user);

                // Lưu tất cả thay đổi
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                TempData["SuccessMessage"] = $"Đã xóa thành công tài khoản '{user.TenTaiKhoan}' và tất cả dữ liệu liên quan!";
                return RedirectToAction("QuanLyNguoiDung");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                TempData["ErrorMessage"] = $"Có lỗi xảy ra khi xóa tài khoản: {ex.Message}";
                return RedirectToAction("QuanLyNguoiDung");
            }
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
