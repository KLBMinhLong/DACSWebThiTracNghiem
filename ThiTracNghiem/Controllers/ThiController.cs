using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThiTracNghiem.Models;
using ThiTracNghiem.Data;
using System.Linq;
using System;

public class ThiController : Controller
{
    private readonly AppDbContext _context;

    public ThiController(AppDbContext context)
    {
        _context = context;
    }

    // Hiển thị danh sách đề thi mở
    public IActionResult Index()
    {
        var deThis = _context.DeThis
            .Where(d => d.TrangThaiMo)
            .ToList();
        return View(deThis);
    }

    // Bắt đầu làm bài (tạo lịch sử, random câu hỏi)
    public IActionResult LamBai(int id)
    {
        var tenTaiKhoan = HttpContext.Session.GetString("UserName");
        if (string.IsNullOrEmpty(tenTaiKhoan)) return RedirectToAction("DangNhap", "TaiKhoan");

        var deThi = _context.DeThis.FirstOrDefault(d => d.Id == id);
        if (deThi == null) return NotFound();

        // Kiểm tra đã có lịch sử thi chưa (tránh tạo lại nếu reload)
        var lichSu = _context.LichSuLamBais
            .Include(x => x.ChiTietLamBais)
            .FirstOrDefault(l => l.TenTaiKhoan == tenTaiKhoan && l.DeThiId == id && l.TrangThai == "DangLam");


        if (lichSu == null)
        {
            // Tạo mới lịch sử thi
            lichSu = new LichSuLamBai
            {
                TenTaiKhoan = tenTaiKhoan,
                DeThiId = id,
                NgayBatDau = DateTime.Now,
                TrangThai = "DangLam"
            };
            _context.LichSuLamBais.Add(lichSu);
            _context.SaveChanges();

            // Random câu hỏi theo chủ đề
            var cauHois = _context.CauHois
                .Where(c => c.ChuDeId == deThi.ChuDeId)
                .OrderBy(x => Guid.NewGuid())
                .Take(deThi.SoLuongCauHoi)
                .ToList();

            foreach (var ch in cauHois)
            {
                var chiTiet = new ChiTietLamBai
                {
                    LichSuLamBaiId = lichSu.Id,
                    CauHoiId = ch.Id
                };
                _context.ChiTietLamBais.Add(chiTiet);
            }
            _context.SaveChanges();
        }

        var gioHetHan = lichSu.NgayBatDau.AddMinutes(deThi.ThoiGianLamBai);
        if (DateTime.Now > gioHetHan)
        {
            // Tự động chuyển sang trang kết quả
            if (lichSu.TrangThai != "HoanThanh")
            {
                lichSu.TrangThai = "HoanThanh";
                lichSu.NgayNopBai = gioHetHan;
                _context.SaveChanges();
            }

            return RedirectToAction("KetQuaDaNop", new { lichSuId = lichSu.Id });
        }
        // Load lại dữ liệu lịch sử và câu hỏi để hiển thị
        var chiTietCauHoi = _context.ChiTietLamBais
            .Include(x => x.CauHoi)
            .Where(x => x.LichSuLamBaiId == lichSu.Id)
            .ToList();

        ViewBag.LichSuId = lichSu.Id;
        ViewBag.ThoiGianConLai = (deThi.ThoiGianLamBai * 60) - (int)(DateTime.Now - lichSu.NgayBatDau).TotalSeconds;
        return View(chiTietCauHoi);
    }

    // Gọi AJAX để lưu đáp án từng câu
    [HttpPost]
    public IActionResult LuuDapAn(int lichSuId, int cauHoiId, string dapAnChon)
    {
        var lichSu = _context.LichSuLamBais.FirstOrDefault(x => x.Id == lichSuId);
        if (lichSu == null || lichSu.TrangThai == "HoanThanh")
        {
            return BadRequest();
        }

        var chiTiet = _context.ChiTietLamBais
            .FirstOrDefault(x => x.LichSuLamBaiId == lichSuId && x.CauHoiId == cauHoiId);

        if (chiTiet != null)
        {
            chiTiet.DapAnChon = dapAnChon;
            _context.SaveChanges();
            return Ok();
        }

        return NotFound();
    }

    // Nộp bài
    [HttpPost]
    public IActionResult NopBai(int lichSuId)
    {
        var lichSu = _context.LichSuLamBais
            .Include(x => x.ChiTietLamBais)
            .ThenInclude(x => x.CauHoi)
            .FirstOrDefault(x => x.Id == lichSuId);

        if (lichSu == null) return NotFound();

        if (lichSu.TrangThai == "HoanThanh")
        {
            // Không cho chấm lại nếu đã hoàn thành
            return RedirectToAction("Index", "Home"); // hoặc hiển thị thông báo
        }
        int soCauDung = 0;
        int tongCauHoi = lichSu.ChiTietLamBais.Count;
        var chiTietTraLoiList = new List<ChiTietCauTraLoi>();

        foreach (var ct in lichSu.ChiTietLamBais)
        {
            var ch = ct.CauHoi;
            bool dung = !string.IsNullOrEmpty(ct.DapAnChon) && ct.DapAnChon == ch.DapAnDung;
            if (dung) soCauDung++;

            chiTietTraLoiList.Add(new ChiTietCauTraLoi
            {
                CauHoi = ch.NoiDung,
                HinhAnhUrl = ch.HinhAnhUrl,
                AudioUrl = ch.AudioUrl,
                DapAnA = ch.DapAnA,
                DapAnB = ch.DapAnB,
                DapAnC = ch.DapAnC,
                DapAnD = ch.DapAnD,
                DapAnChon = ct.DapAnChon,
                DapAnDung = ch.DapAnDung,
                DungHaySai = dung
            });
        }

        double diem = Math.Round(((double)soCauDung / tongCauHoi) * 10, 2);
        var thoiGianPhut = (int)(lichSu.NgayNopBai.HasValue
            ? (lichSu.NgayNopBai.Value - lichSu.NgayBatDau).TotalMinutes
            : (DateTime.Now - lichSu.NgayBatDau).TotalMinutes);

        // Cập nhật lịch sử
        lichSu.TrangThai = "HoanThanh";
        lichSu.Diem = diem;
        lichSu.NgayNopBai = DateTime.Now;
        _context.SaveChanges();

        var ketQuaVM = new KetQuaViewModel
        {
            TongCauHoi = tongCauHoi,
            SoCauDung = soCauDung,
            DiemSo = diem,
            ThoiGianPhut = thoiGianPhut,
            ChiTietTraLoi = chiTietTraLoiList
        };

        return View("KetQua", ketQuaVM);
    }

    public IActionResult KetQuaDaNop(int lichSuId)
    {
        var lichSu = _context.LichSuLamBais
            .Include(x => x.ChiTietLamBais)
            .ThenInclude(x => x.CauHoi)
            .FirstOrDefault(x => x.Id == lichSuId);

        if (lichSu == null || lichSu.TrangThai != "HoanThanh")
            return RedirectToAction("Index");

        int tongCauHoi = lichSu.ChiTietLamBais.Count;
        int soCauDung = lichSu.ChiTietLamBais.Count(ct => ct.DapAnChon == ct.CauHoi.DapAnDung);
        double diem = Math.Round(((double)soCauDung / tongCauHoi) * 10, 2);
        lichSu.Diem = diem;
        _context.SaveChanges();
        
        var chiTietTraLoi = lichSu.ChiTietLamBais.Select(ct => new ChiTietCauTraLoi
        {
            CauHoi = ct.CauHoi.NoiDung,
            HinhAnhUrl = ct.CauHoi.HinhAnhUrl,
            AudioUrl = ct.CauHoi.AudioUrl,
            DapAnA = ct.CauHoi.DapAnA,
            DapAnB = ct.CauHoi.DapAnB,
            DapAnC = ct.CauHoi.DapAnC,
            DapAnD = ct.CauHoi.DapAnD,
            DapAnChon = ct.DapAnChon,
            DapAnDung = ct.CauHoi.DapAnDung,
            DungHaySai = ct.DapAnChon == ct.CauHoi.DapAnDung
        }).ToList();

        var thoiGianPhut = (int)(lichSu.NgayNopBai.HasValue
            ? (lichSu.NgayNopBai.Value - lichSu.NgayBatDau).TotalMinutes
            : (DateTime.Now - lichSu.NgayBatDau).TotalMinutes);

        var vm = new KetQuaViewModel
        {
            TongCauHoi = tongCauHoi,
            SoCauDung = soCauDung,
            DiemSo = diem,
            ThoiGianPhut = thoiGianPhut,
            ChiTietTraLoi = chiTietTraLoi
        };

        return View("KetQua", vm);
    }
}
