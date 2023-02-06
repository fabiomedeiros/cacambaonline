using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using cacambaonline.Data;
using cacambaonline.Models;

namespace cacambaonline.Controllers
{
    public class LogCTRsController : Controller
    {
        private readonly MeuDbContext _context;

        public LogCTRsController(MeuDbContext context)
        {
            _context = context;
        }

        // GET: LogCTRs
        public async Task<IActionResult> Index()
        {
            var meuDbContext = _context.LogCTR.Include(l => l.CTR);
            return View(await meuDbContext.ToListAsync());
        }

        // GET: LogCTRs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.LogCTR == null)
            {
                return NotFound();
            }

            var logCTR = await _context.LogCTR
                .Include(l => l.CTR)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (logCTR == null)
            {
                return NotFound();
            }

            return View(logCTR);
        }

        // GET: LogCTRs/Create
        public IActionResult Create()
        {
            ViewData["CTRId"] = new SelectList(_context.CTR, "Id", "Id");
            return View();
        }

        // POST: LogCTRs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Data,UsuarioId,CTRId,Operacao")] LogCTR logCTR)
        {
            if (ModelState.IsValid)
            {
                _context.Add(logCTR);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CTRId"] = new SelectList(_context.CTR, "Id", "Id", logCTR.CTRId);
            return View(logCTR);
        }

        // GET: LogCTRs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.LogCTR == null)
            {
                return NotFound();
            }

            var logCTR = await _context.LogCTR.FindAsync(id);
            if (logCTR == null)
            {
                return NotFound();
            }
            ViewData["CTRId"] = new SelectList(_context.CTR, "Id", "Id", logCTR.CTRId);
            return View(logCTR);
        }

        // POST: LogCTRs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Data,UsuarioId,CTRId,Operacao")] LogCTR logCTR)
        {
            if (id != logCTR.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(logCTR);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LogCTRExists(logCTR.Id))
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
            ViewData["CTRId"] = new SelectList(_context.CTR, "Id", "Id", logCTR.CTRId);
            return View(logCTR);
        }

        // GET: LogCTRs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.LogCTR == null)
            {
                return NotFound();
            }

            var logCTR = await _context.LogCTR
                .Include(l => l.CTR)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (logCTR == null)
            {
                return NotFound();
            }

            return View(logCTR);
        }

        // POST: LogCTRs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.LogCTR == null)
            {
                return Problem("Entity set 'MeuDbContext.LogCTR'  is null.");
            }
            var logCTR = await _context.LogCTR.FindAsync(id);
            if (logCTR != null)
            {
                _context.LogCTR.Remove(logCTR);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LogCTRExists(int id)
        {
          return _context.LogCTR.Any(e => e.Id == id);
        }
    }
}
