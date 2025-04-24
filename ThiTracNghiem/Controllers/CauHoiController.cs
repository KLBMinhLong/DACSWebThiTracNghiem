using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ThiTracNghiem.Data;
using ClosedXML.Excel;


namespace ThiTracNghiem
{
    public class CauHoiController : Controller
    {
        private readonly AppDbContext _context;

        public CauHoiController(AppDbContext context)
        {
            _context = context;
        }

        // GET: CauHoi
        public async Task<IActionResult> Index(int? chuDeId, int page = 1, int pageSize = 10)
        {
            var cauHoiQuery = _context.CauHois
                .Include(c => c.ChuDe)
                .AsQueryable();

            // Nếu có lọc chủ đề
            if (chuDeId.HasValue && chuDeId > 0)
            {
                cauHoiQuery = cauHoiQuery.Where(c => c.ChuDeId == chuDeId);
            }

            int totalItems = await cauHoiQuery.CountAsync();

            var items = await cauHoiQuery
                .OrderBy(c => c.Id) // giữ thứ tự mặc định
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.ChuDes = await _context.ChuDes.ToListAsync();
            ViewBag.ChuDeId = chuDeId;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            return View(items);
        }



        // GET: CauHoi/Index


        //              var appDbContext = _context.CauHois.Include(c => c.ChuDe);

        //              // Thêm dòng này để lấy danh sách chủ đề trong file exel
        //             ViewBag.ChuDes = await _context.ChuDes.ToListAsync();
        //             return View(await appDbContext.ToListAsync());

        // GET: CauHoi/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cauHoi = await _context.CauHois
                .Include(c => c.ChuDe)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cauHoi == null)
            {
                return NotFound();
            }

            return View(cauHoi);
        }

        // GET: CauHoi/Create
        public IActionResult Create()
        {
            ViewData["ChuDeId"] = new SelectList(_context.ChuDes, "Id", "TenChuDe");
            return View();
        }

        // POST: CauHoi/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
       [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CauHoi cauHoi, IFormFile? HinhAnhFile, IFormFile? AudioFile)
        {
            if (ModelState.IsValid)
            {
                // Xử lý ảnh
                if (HinhAnhFile != null && HinhAnhFile.Length > 0)
                {
                    var fileName = Guid.NewGuid() + Path.GetExtension(HinhAnhFile.FileName);
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "images", fileName);
                    using var stream = new FileStream(path, FileMode.Create);
                    await HinhAnhFile.CopyToAsync(stream);
                    cauHoi.HinhAnhUrl = "/uploads/images/" + fileName;
                }
                else
                {
                    cauHoi.HinhAnhUrl = null;
                }

                // Xử lý audio
                if (AudioFile != null && AudioFile.Length > 0)
                {
                    var fileName = Guid.NewGuid() + Path.GetExtension(AudioFile.FileName);
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "audio", fileName);
                    using var stream = new FileStream(path, FileMode.Create);
                    await AudioFile.CopyToAsync(stream);
                    cauHoi.AudioUrl = "/uploads/audio/" + fileName;
                }
                else
                {
                    cauHoi.AudioUrl = null;
                }

                _context.Add(cauHoi);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["ChuDeId"] = new SelectList(_context.ChuDes, "Id", "TenChuDe", cauHoi.ChuDeId);
            return View(cauHoi);
        }


        // GET: CauHoi/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cauHoi = await _context.CauHois.FindAsync(id);
            if (cauHoi == null)
            {
                return NotFound();
            }
            ViewData["ChuDeId"] = new SelectList(_context.ChuDes, "Id", "TenChuDe", cauHoi.ChuDeId);
            return View(cauHoi);
        }

        // POST: CauHoi/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NoiDung,DapAnA,DapAnB,DapAnC,DapAnD,DapAnDung,ChuDeId")] CauHoi cauHoi)
        {
            if (id != cauHoi.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cauHoi);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CauHoiExists(cauHoi.Id))
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
            ViewData["ChuDeId"] = new SelectList(_context.ChuDes, "Id", "TenChuDe", cauHoi.ChuDeId);
            return View(cauHoi);
        }

        // GET: CauHoi/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cauHoi = await _context.CauHois
                .Include(c => c.ChuDe)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cauHoi == null)
            {
                return NotFound();
            }

            return View(cauHoi);
        }

        // POST: CauHoi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cauHoi = await _context.CauHois.FindAsync(id);
            if (cauHoi != null)
            {
                _context.CauHois.Remove(cauHoi);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CauHoiExists(int id)
        {
            return _context.CauHois.Any(e => e.Id == id);
        }
        // them file exel
        [HttpPost]
        public IActionResult ImportExcel(IFormFile file, int ChuDeId)
        {
            if (file == null || file.Length <= 0)
            {
                TempData["ErrorMessage"] = "Vui lòng chọn file Excel hợp lệ.";
                return RedirectToAction("Index");
            }

            if (!Path.GetExtension(file.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                TempData["ErrorMessage"] = "Chỉ hỗ trợ định dạng file .xlsx";
                return RedirectToAction("Index");
            }

            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);
                using (var workbook = new XLWorkbook(stream))
                {
                    var worksheet = workbook.Worksheet(1);
                    var rows = worksheet.RangeUsed()?.RowsUsed();

                    if (rows != null)
                    {
                        foreach (var row in rows.Skip(1))
                        {
                            var cauHoi = new CauHoi
                            {
                                NoiDung = row.Cell(1).GetValue<string>(),
                                DapAnA = row.Cell(2).GetValue<string>(),
                                DapAnB = row.Cell(3).GetValue<string>(),
                                DapAnC = row.Cell(4).GetValue<string>(),
                                DapAnD = row.Cell(5).GetValue<string>(),
                                DapAnDung = row.Cell(6).GetValue<string>(),
                                ChuDeId = ChuDeId
                            };

                            _context.CauHois.Add(cauHoi);
                        }
                    }
                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "Import câu hỏi thành công!";
                }
            }

            return RedirectToAction("Index");
        }

    }
}
