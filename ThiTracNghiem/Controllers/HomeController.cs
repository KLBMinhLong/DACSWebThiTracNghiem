using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ThiTracNghiem.Data;
using ThiTracNghiem.Models;

namespace ThiTracNghiem.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    [RequireLogin]
    public IActionResult Index()
    {
        var danhSachDeThi = _context.DeThis
        .Where(d => d.TrangThaiMo)
        .OrderByDescending(d => d.NgayTao)
        .ToList();

    return View(danhSachDeThi);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
