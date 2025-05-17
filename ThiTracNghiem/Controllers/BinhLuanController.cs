using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ThiTracNghiem.Models;
using ThiTracNghiem.Data;

public class BinhLuanController : Controller
{
    private readonly AppDbContext _context;

    public BinhLuanController(AppDbContext context)
    {
        _context = context;
    }

    // GET: BinhLuan
    public async Task<IActionResult> Index(int deThiId)
    {
        var binhLuans = await _context.BinhLuans
            .Include(b => b.DeThi)
            .Where(b => b.DeThiId == deThiId)
            .ToListAsync();

        ViewBag.DeThiId = deThiId;
        return View(binhLuans);
    }

    [HttpPost]
    public IActionResult Create(int deThiId, string noiDung)
    {
        var user = HttpContext.Session.GetString("UserName");
        if (string.IsNullOrEmpty(user)) return Unauthorized();

        var binhLuan = new BinhLuan
        {
            DeThiId = deThiId,
            TenTaiKhoan = user,
            NoiDung = noiDung,
            ThoiGian = DateTime.Now
        };

        _context.BinhLuans.Add(binhLuan);
        _context.SaveChanges();

        ViewBag.UserName = HttpContext.Session.GetString("UserName");
        return PartialView("_DanhSachBinhLuan", GetBinhLuanTheoDeThi(deThiId));
    }

    [HttpPost]
    public IActionResult Delete(int id)
    {
        var user = HttpContext.Session.GetString("UserName");
        var bl = _context.BinhLuans.FirstOrDefault(b => b.Id == id && b.TenTaiKhoan == user);
        if (bl == null) return Unauthorized();

        int deThiId = bl.DeThiId;
        _context.BinhLuans.Remove(bl);
        _context.SaveChanges();

        ViewBag.UserName = HttpContext.Session.GetString("UserName");
        return PartialView("_DanhSachBinhLuan", GetBinhLuanTheoDeThi(deThiId));
    }

    public IActionResult DanhSach(int deThiId)
    {
        ViewBag.UserName = HttpContext.Session.GetString("UserName");
        return PartialView("_DanhSachBinhLuan", GetBinhLuanTheoDeThi(deThiId));
    }

    private List<BinhLuan> GetBinhLuanTheoDeThi(int deThiId)
    {
        return _context.BinhLuans
            .Where(b => b.DeThiId == deThiId)
            .OrderByDescending(b => b.ThoiGian)
            .ToList();
    }
}

