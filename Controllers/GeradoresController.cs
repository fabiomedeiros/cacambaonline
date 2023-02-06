using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using cacambaonline.Data;
using cacambaonline.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace cacambaonline.Controllers
{
    [Authorize(Roles = "Adm,Gestor,Transportador")]
    public class GeradoresController : Controller
    {
        private readonly MeuDbContext _context;

        public GeradoresController(MeuDbContext context)
        {
            _context = context;
        }

        // GET: Geradores
        public async Task<IActionResult> Index()
        {
              return View(await _context.Geradores.ToListAsync());
        }

        // GET: Geradores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Geradores == null)
            {
                return NotFound();
            }

            var geradores = await _context.Geradores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (geradores == null)
            {
                return NotFound();
            }

            return View(geradores);
        }

        // GET: Geradores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Geradores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Cpf,Telefone,Email")] Geradores geradores)
        {
            if (ModelState.IsValid)
            {
                _context.Add(geradores);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(geradores);
        }


        // GET: Geradores/Edit/5
        [Authorize(Roles = "Adm,Gestor")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Geradores == null)
            {
                return NotFound();
            }

            var geradores = await _context.Geradores.FindAsync(id);
            if (geradores == null)
            {
                return NotFound();
            }
            return View(geradores);
        }

        // POST: Geradores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Adm,Gestor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Cpf,Telefone,Email")] Geradores geradores)
        {
            if (id != geradores.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(geradores);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GeradoresExists(geradores.Id))
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
            return View(geradores);
        }

        // GET: Geradores/Delete/5
        [Authorize(Roles = "Adm,Gestor")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Geradores == null)
            {
                return NotFound();
            }

            var geradores = await _context.Geradores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (geradores == null)
            {
                return NotFound();
            }

            return View(geradores);
        }

        [Authorize(Roles = "Adm,Gestor")]
        // POST: Geradores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Geradores == null)
            {
                return Problem("Entity set 'MeuDbContext.Geradores'  is null.");
            }
            var geradores = await _context.Geradores.FindAsync(id);
            if (geradores != null)
            {
                _context.Geradores.Remove(geradores);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GeradoresExists(int id)
        {
          return _context.Geradores.Any(e => e.Id == id);
        }
    }
}
