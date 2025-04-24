using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ThiTracNghiem.Models;
using ThiTracNghiem.Data;

public class DeThiController : Controller
{
    private readonly AppDbContext _context;

    public DeThiController(AppDbContext context)
    {
        _context = context;
    }

    // tìm kiêm theo tên dề thi
    public IActionResult Index(string searchString)
    {
        var query = _context.DeThis.Include(d => d.ChuDe).AsQueryable();

        if (!string.IsNullOrEmpty(searchString))
        {
            query = query.Where(d => d.TenDeThi.Contains(searchString));
        }

        ViewBag.CurrentFilter = searchString; // để giữ lại giá trị input khi submit
        return View(query.ToList());
    }


    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.ChuDeId = new SelectList(_context.ChuDes, "Id", "TenChuDe");
        return View();
    }

    [HttpPost]
    public IActionResult Create(DeThiCreateViewModel model)
    {
        if (!ModelState.IsValid || model.CauHoiIds.Count == 0)
        {
            ModelState.AddModelError("", "Vui lòng chọn ít nhất một câu hỏi.");
            ViewBag.ChuDeId = new SelectList(_context.ChuDes, "Id", "TenChuDe", model.ChuDeId);
            ViewBag.CauHois = _context.CauHois.Where(c => c.ChuDeId == model.ChuDeId).ToList();
            return View(model);
        }

        var deThi = new DeThi
        {
            TenDeThi = model.TenDeThi,
            ThoiGianLamBai = model.ThoiGianLamBai,
            TrangThaiMo = model.TrangThaiMo,
            ChuDeId = model.ChuDeId,
            NgayTao = DateTime.Now
        };

        _context.DeThis.Add(deThi);
        _context.SaveChanges();

        foreach (var cauHoiId in model.CauHoiIds)
        {
            _context.ChiTietDeThis.Add(new ChiTietDeThi
            {
                DeThiId = deThi.Id,
                CauHoiId = cauHoiId
            });
        }

        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    public IActionResult Details(int id)
    {
        var deThi = _context.DeThis
            .Include(d => d.ChuDe)
            .Include(d => d.ChiTietCauHoi)
                .ThenInclude(ct => ct.CauHoi)
            .FirstOrDefault(d => d.Id == id);

        if (deThi == null) return NotFound();
        return View(deThi);
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var deThi = _context.DeThis
            .Include(d => d.ChiTietCauHoi)
            .FirstOrDefault(d => d.Id == id);

        if (deThi == null) return NotFound();

        var model = new DeThiCreateViewModel
        {
            TenDeThi = deThi.TenDeThi,
            ThoiGianLamBai = deThi.ThoiGianLamBai,
            ChuDeId = deThi.ChuDeId,
            TrangThaiMo = deThi.TrangThaiMo,
            CauHoiIds = deThi.ChiTietCauHoi.Select(c => c.CauHoiId).ToList()
        };

        ViewBag.ChuDeId = new SelectList(_context.ChuDes, "Id", "TenChuDe", model.ChuDeId);
        ViewBag.CauHois = _context.CauHois.Where(c => c.ChuDeId == model.ChuDeId).ToList();

        ViewBag.DeThiId = deThi.Id;

        return View(model);
    }

    [HttpPost]
    public IActionResult Edit(int id, DeThiCreateViewModel model)
    {
        var deThi = _context.DeThis.Include(d => d.ChiTietCauHoi).FirstOrDefault(d => d.Id == id);
        if (deThi == null) return NotFound();

        if (!ModelState.IsValid || model.CauHoiIds.Count == 0)
        {
            ViewBag.ChuDeId = new SelectList(_context.ChuDes, "Id", "TenChuDe", model.ChuDeId);
            ViewBag.CauHois = _context.CauHois.Where(c => c.ChuDeId == model.ChuDeId).ToList();
            ViewBag.DeThiId = id;
            ModelState.AddModelError("", "Vui lòng chọn ít nhất một câu hỏi.");
            return View(model);
        }

        // Cập nhật đề thi
        deThi.TenDeThi = model.TenDeThi;
        deThi.ThoiGianLamBai = model.ThoiGianLamBai;
        deThi.TrangThaiMo = model.TrangThaiMo;
        deThi.ChuDeId = model.ChuDeId;

        // Xoá câu hỏi cũ
        _context.ChiTietDeThis.RemoveRange(deThi.ChiTietCauHoi);

        // Thêm câu hỏi mới
        foreach (var cauHoiId in model.CauHoiIds)
        {
            _context.ChiTietDeThis.Add(new ChiTietDeThi
            {
                DeThiId = id,
                CauHoiId = cauHoiId
            });
        }

        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Delete(int id)
    {
        var deThi = _context.DeThis
            .Include(d => d.ChiTietCauHoi)
            .FirstOrDefault(d => d.Id == id);

        if (deThi == null) return NotFound();

        _context.ChiTietDeThis.RemoveRange(deThi.ChiTietCauHoi);
        _context.DeThis.Remove(deThi);
        _context.SaveChanges();

        return RedirectToAction("Index");
    }

}
