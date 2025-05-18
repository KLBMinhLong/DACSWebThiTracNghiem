using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThiTracNghiem.Models;
using ThiTracNghiem.Data;

public class LichSuThiController : Controller
{
    private readonly AppDbContext _context;

    public LichSuThiController(AppDbContext context)
    {
        _context = context;
    }

    // Hiển thị danh sách lịch sử làm bài, phân trang
   public async Task<IActionResult> Index(int page = 1, int pageSize = 5)
    {
        var tenTaiKhoan = HttpContext.Session.GetString("UserName");
        if (string.IsNullOrEmpty(tenTaiKhoan))
            return RedirectToAction("DangNhap", "TaiKhoan");

        var query = _context.LichSuLamBais
            .Include(x => x.DeThi)
            .Where(x => x.TenTaiKhoan == tenTaiKhoan)
            .OrderByDescending(x => x.NgayBatDau)
            .AsQueryable();

        int totalItems = await query.CountAsync();
        var lichSu = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);

        return View(lichSu); // Views/LichSuLamBai/Index.cshtml hoặc tương đương
    }


    // Xem chi tiết 1 lần làm bài
    public IActionResult XemChiTiet(int id)
    {
        var lichSu = _context.LichSuLamBais
            .Include(x => x.DeThi)
            .Include(x => x.ChiTietLamBais!)
                .ThenInclude(c => c.CauHoi)
            .FirstOrDefault(x => x.Id == id);

        if (lichSu == null)
            return NotFound();

        return View("KetQua", lichSu); // dùng lại View kết quả cũ
    }
}
