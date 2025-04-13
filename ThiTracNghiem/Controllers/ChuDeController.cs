using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ThiTracNghiem.Data;

namespace ThiTracNghiem
{
    public class ChuDeController : Controller
    {
        private readonly AppDbContext _context;

        public ChuDeController(AppDbContext context)
        {
            _context = context;
        }

        // GET: ChuDe
        public async Task<IActionResult> Index()
        {
            return View(await _context.ChuDes.ToListAsync());
        }

        // GET: ChuDe/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chuDe = await _context.ChuDes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chuDe == null)
            {
                return NotFound();
            }

            return View(chuDe);
        }

        // GET: ChuDe/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ChuDe/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TenChuDe")] ChuDe chuDe)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chuDe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(chuDe);
        }

        // GET: ChuDe/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chuDe = await _context.ChuDes.FindAsync(id);
            if (chuDe == null)
            {
                return NotFound();
            }
            return View(chuDe);
        }

        // POST: ChuDe/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TenChuDe")] ChuDe chuDe)
        {
            if (id != chuDe.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chuDe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChuDeExists(chuDe.Id))
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
            return View(chuDe);
        }

        // GET: ChuDe/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chuDe = await _context.ChuDes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chuDe == null)
            {
                return NotFound();
            }

            return View(chuDe);
        }

        // POST: ChuDe/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chuDe = await _context.ChuDes.FindAsync(id);
            if (chuDe != null)
            {
                _context.ChuDes.Remove(chuDe);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChuDeExists(int id)
        {
            return _context.ChuDes.Any(e => e.Id == id);
        }
    }
}
