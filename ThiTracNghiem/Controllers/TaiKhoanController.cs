using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ThiTracNghiem.Data;
using ThiTracNghiem.Models;

namespace ThiTracNghiem.Controllers
{
    public class TaiKhoanController : Controller
    {
        private readonly AppDbContext _context;

        public TaiKhoanController(AppDbContext context)
        {
            _context = context;
        }

        // GET: TaiKhoan
        public async Task<IActionResult> Index()
        {
            return View(await _context.TaiKhoans.ToListAsync());
        }

        // GET: TaiKhoan/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taiKhoan = await _context.TaiKhoans
                .FirstOrDefaultAsync(m => m.TenTaiKhoan == id);
            if (taiKhoan == null)
            {
                return NotFound();
            }

            return View(taiKhoan);
        }

        // GET: TaiKhoan/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TaiKhoan/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TenTaiKhoan,MatKhau,Email,NgaySinh,GioiTinh,SoDienThoai,VaiTro,TrangThaiKhoa,ThoiGianTao,AnhDaiDienUrl")] TaiKhoan taiKhoan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(taiKhoan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(taiKhoan);
        }

        // GET: TaiKhoan/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taiKhoan = await _context.TaiKhoans.FindAsync(id);
            if (taiKhoan == null)
            {
                return NotFound();
            }
            return View(taiKhoan);
        }

        // POST: TaiKhoan/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("TenTaiKhoan,MatKhau,Email,NgaySinh,GioiTinh,SoDienThoai,VaiTro,TrangThaiKhoa,ThoiGianTao,AnhDaiDienUrl")] TaiKhoan taiKhoan)
        {
            if (id != taiKhoan.TenTaiKhoan)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taiKhoan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaiKhoanExists(taiKhoan.TenTaiKhoan))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(taiKhoan);
        }

        // GET: TaiKhoan/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taiKhoan = await _context.TaiKhoans
                .FirstOrDefaultAsync(m => m.TenTaiKhoan == id);
            if (taiKhoan == null)
            {
                return NotFound();
            }

            return View(taiKhoan);
        }

        // POST: TaiKhoan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var taiKhoan = await _context.TaiKhoans.FindAsync(id);
            if (taiKhoan != null)
            {
                _context.TaiKhoans.Remove(taiKhoan);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaiKhoanExists(string id)
        {
            return _context.TaiKhoans.Any(e => e.TenTaiKhoan == id);
        }
        // GET: /TaiKhoan/DangKy
        public IActionResult DangKy()
        {
            return View();
        }

        // POST: /TaiKhoan/DangKy
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DangKy(TaiKhoan model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Kiểm tra tài khoản đã tồn tại
            var existing = await _context.TaiKhoans.FindAsync(model.TenTaiKhoan);
            if (existing != null)
            {
                ModelState.AddModelError("", "Tên tài khoản đã tồn tại.");
                return View(model);
            }

            model.VaiTro = "user"; // mặc định là học viên
            model.ThoiGianTao = DateTime.Now;
            model.TrangThaiKhoa = false;

            if (_context.TaiKhoans.Any(tk => tk.TenTaiKhoan == model.TenTaiKhoan))
            {
                ModelState.AddModelError("TenTaiKhoan", "Tên tài khoản đã tồn tại");
                return View(model);
            }

            if (_context.TaiKhoans.Any(tk => tk.Email == model.Email))
            {
                ModelState.AddModelError("Email", "Email đã được sử dụng");
                return View(model);
            }

            _context.TaiKhoans.Add(model);
            _context.SaveChanges();

            ViewBag.ThongBao = "Đăng ký thành công! Bạn có thể đăng nhập ngay.";

            return View(model);
        }

        // GET: /TaiKhoan/DangNhap
        public IActionResult DangNhap() => View();

        // POST: /TaiKhoan/DangNhap
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DangNhap(string tenTaiKhoan, string matKhau)
        {
            var user = await _context.TaiKhoans.FindAsync(tenTaiKhoan);

            if (user == null || user.MatKhau != matKhau || user.TrangThaiKhoa)
            {
                ViewBag.Loi = "Tài khoản không đúng hoặc đã bị khóa.";
                return View();
            }

            HttpContext.Session.SetString("UserName", user.TenTaiKhoan);
            HttpContext.Session.SetString("VaiTro", user.VaiTro);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult DangXuat()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("DangNhap");
        }

        public IActionResult ThongTinCaNhan()
        {
            var tenTaiKhoan = HttpContext.Session.GetString("UserName");

            if (string.IsNullOrEmpty(tenTaiKhoan)) return RedirectToAction("DangNhap");

            var tk = _context.TaiKhoans.FirstOrDefault(t => t.TenTaiKhoan == tenTaiKhoan);
            if (tk == null) return NotFound();

            var viewModel = new ThongTinCaNhanViewModel
            {
                TenTaiKhoan = tk.TenTaiKhoan,
                Email = tk.Email,
                NgaySinh = tk.NgaySinh,
                GioiTinh = tk.GioiTinh,
                SoDienThoai = tk.SoDienThoai
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult ThongTinCaNhan(ThongTinCaNhanViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var tk = _context.TaiKhoans.FirstOrDefault(t => t.TenTaiKhoan == model.TenTaiKhoan);
            if (tk == null) return NotFound();

            tk.Email = model.Email;
            tk.NgaySinh = model.NgaySinh;
            tk.GioiTinh = model.GioiTinh;
            tk.SoDienThoai = model.SoDienThoai;

            if (!string.IsNullOrEmpty(model.MatKhauMoi))
            {
                tk.MatKhau = model.MatKhauMoi; // nên mã hóa nhưng đơn giản thì để vậy
            }

            _context.SaveChanges();

            ViewBag.ThongBao = "Cập nhật thành công!";
            return View(model);
        }

    }
}
