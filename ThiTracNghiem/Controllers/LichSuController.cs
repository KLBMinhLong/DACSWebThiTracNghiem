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

    // Hiển thị danh sách lịch sử làm bài
    public IActionResult Index()
    {
        var tenTaiKhoan = HttpContext.Session.GetString("UserName");
        if (string.IsNullOrEmpty(tenTaiKhoan))
            return RedirectToAction("DangNhap", "TaiKhoan");

        var lichSu = _context.LichSuLamBais
            .Include(x => x.DeThi)
            .Where(x => x.TenTaiKhoan == tenTaiKhoan)
            .OrderByDescending(x => x.NgayBatDau)
            .ToList();

        return View(lichSu);
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
