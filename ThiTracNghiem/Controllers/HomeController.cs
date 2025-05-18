using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ThiTracNghiem.Data;
using ThiTracNghiem.Models;
using Microsoft.EntityFrameworkCore;

namespace ThiTracNghiem.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    [RequireLogin]
    public async Task<IActionResult> Index(int page = 1, int pageSize = 6)
    {
        var username = HttpContext.Session.GetString("UserName");

        var lichSuDangLam = _context.LichSuLamBais
            .Where(l => l.TenTaiKhoan == username && l.TrangThai == "DangLam")
            .ToList();

        ViewBag.LichSuDangLam = lichSuDangLam;

        var query = _context.DeThis
            .Where(d => d.TrangThaiMo)
            .OrderByDescending(d => d.NgayTao)
            .AsQueryable();

        int totalItems = await query.CountAsync();

        var danhSachDeThi = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);

        return View(danhSachDeThi);
    }

    public IActionResult ChiTietDeThi(int id)
    {
        var username = HttpContext.Session.GetString("UserName");

        var lichSuDangLam = _context.LichSuLamBais
            .Where(l => l.TenTaiKhoan == username && l.TrangThai == "DangLam")
            .ToList();

        ViewBag.LichSuDangLam = lichSuDangLam;

        var deThi = _context.DeThis
            .Include(d => d.ChuDe)
            .FirstOrDefault(d => d.Id == id);

        if (deThi == null) return NotFound();

        return View(deThi);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult AccessDenied()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
