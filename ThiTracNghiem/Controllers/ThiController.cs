using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThiTracNghiem.Models;
using ThiTracNghiem.Data;

public class ThiController : Controller
{
    private readonly AppDbContext _context;

    public ThiController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var deThis = _context.DeThis
            .Where(d => d.TrangThaiMo) // Chỉ lấy đề thi đang mở
            .ToList();

        return View(deThis);
    }

    [HttpGet]
    public IActionResult LamBai(int id)
    {
        var deThi = _context.DeThis.FirstOrDefault(d => d.Id == id);
        if (deThi == null) return NotFound();

        var cauHois = _context.CauHois
            .Where(c => c.ChuDeId == deThi.ChuDeId)
            .OrderBy(x => Guid.NewGuid())
            .Take(deThi.SoLuongCauHoi)
            .ToList();

        var viewModel = new LamBaiViewModel
        {
            DeThi = deThi,
            CauHois = cauHois
        };

        return View(viewModel); // Gọi ra Views/Thi/LamBai.cshtml
    }

    [HttpPost]
    public IActionResult NopBai(int DeThiId, List<int> QuestionIds, List<string> Answers)
    {
        int correctCount = 0;
        var chiTietTraLoiList = new List<ChiTietCauTraLoi>();

        for (int i = 0; i < QuestionIds.Count; i++)
        {
            var cauHoi = _context.CauHois.FirstOrDefault(c => c.Id == QuestionIds[i]);
            if (cauHoi != null)
            {
                bool isCorrect = Answers[i] == cauHoi.DapAnDung;
                if (isCorrect) correctCount++;

                chiTietTraLoiList.Add(new ChiTietCauTraLoi
                {
                    CauHoi = cauHoi.NoiDung,
                    DapAnA = cauHoi.DapAnA,
                    DapAnB = cauHoi.DapAnB,
                    DapAnC = cauHoi.DapAnC,
                    DapAnD = cauHoi.DapAnD,
                    DapAnChon = Answers[i],
                    DapAnDung = cauHoi.DapAnDung,
                    DungHaySai = (Answers[i] == cauHoi.DapAnDung)
                });
            }
        }

        double diem = ((double)correctCount / QuestionIds.Count) * 10;

        var ketQua = new KetQuaViewModel
        {
            TongCauHoi = QuestionIds.Count,
            SoCauDung = correctCount,
            DiemSo = Math.Round(diem, 2),
            ChiTietTraLoi = chiTietTraLoiList
        };

        // lưu lịch sử làm bài
        var tenTaiKhoan = HttpContext.Session.GetString("UserName");

        if (!string.IsNullOrEmpty(tenTaiKhoan))
        {
            var lichSu = new LichSuLamBai
            {
                TenTaiKhoan = tenTaiKhoan,
                DeThiId = DeThiId,
                NgayBatDau = DateTime.Now, // giả định, có thể sửa lại đúng nếu lưu từ trước
                NgayNopBai = DateTime.Now,
                Diem = diem,
            };

            _context.LichSuLamBais.Add(lichSu);
            _context.SaveChanges();

            // Lấy Id vừa tạo của LichSuLamBai
            int lichSuId = lichSu.Id;

            // Duyệt qua từng câu hỏi và lưu vào bảng ChiTietLamBais
            for (int i = 0; i < QuestionIds.Count; i++)
            {
                var cauHoi = _context.CauHois.FirstOrDefault(c => c.Id == QuestionIds[i]);
                if (cauHoi != null)
                {
                    var chiTiet = new ChiTietLamBai
                    {
                        LichSuLamBaiId = lichSuId,
                        CauHoiId = cauHoi.Id,
                        DapAnChon = Answers[i],
                        DungHaySai = (Answers[i] == cauHoi.DapAnDung)
                    };

                    _context.ChiTietLamBais.Add(chiTiet);
                }
            }

            _context.SaveChanges();
        }
        else
        {
            Console.WriteLine("⚠ Không lấy được TenTaiKhoan từ Session.");
        }

        return View("KetQua", ketQua);

    }
}
