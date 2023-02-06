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
    public class EnderecosGeradoresController : Controller
    {
        private readonly MeuDbContext _context;

        public EnderecosGeradoresController(MeuDbContext context)
        {
            _context = context;
        }

        // GET: EnderecosGeradores
        public async Task<IActionResult> Index()
        {
            var meuDbContext = _context.EnderecosGerador.Include(e => e.Geradores);
            return View(await meuDbContext.ToListAsync());
        }

        // GET: EnderecosGeradores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.EnderecosGerador == null)
            {
                return NotFound();
            }

            var enderecosGerador = await _context.EnderecosGerador
                .Include(e => e.Geradores)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (enderecosGerador == null)
            {
                return NotFound();
            }

            return View(enderecosGerador);
        }

        // GET: EnderecosGeradores/Create
        public IActionResult Create()
        {
            ViewData["GeradoresId"] = new SelectList(_context.Geradores, "Id", "Id");
            return View();
        }

        // POST: EnderecosGeradores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CEP,Cidade,Estado,Bairro,Logradouro,Numero,Complemento,Latitude,Longitude,GeradoresId")] EnderecosGerador enderecosGerador)
        {
            if (ModelState.IsValid)
            {
                _context.Add(enderecosGerador);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GeradoresId"] = new SelectList(_context.Geradores, "Id", "Id", enderecosGerador.GeradoresId);
            return View(enderecosGerador);
        }

        // GET: EnderecosGeradores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.EnderecosGerador == null)
            {
                return NotFound();
            }

            var enderecosGerador = await _context.EnderecosGerador.FindAsync(id);
            if (enderecosGerador == null)
            {
                return NotFound();
            }
            ViewData["GeradoresId"] = new SelectList(_context.Geradores, "Id", "Id", enderecosGerador.GeradoresId);
            return View(enderecosGerador);
        }

        // POST: EnderecosGeradores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CEP,Cidade,Estado,Bairro,Logradouro,Numero,Complemento,Latitude,Longitude,GeradoresId")] EnderecosGerador enderecosGerador)
        {
            if (id != enderecosGerador.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(enderecosGerador);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnderecosGeradorExists(enderecosGerador.Id))
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
            ViewData["GeradoresId"] = new SelectList(_context.Geradores, "Id", "Id", enderecosGerador.GeradoresId);
            return View(enderecosGerador);
        }

        // GET: EnderecosGeradores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.EnderecosGerador == null)
            {
                return NotFound();
            }

            var enderecosGerador = await _context.EnderecosGerador
                .Include(e => e.Geradores)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (enderecosGerador == null)
            {
                return NotFound();
            }

            return View(enderecosGerador);
        }

        // POST: EnderecosGeradores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.EnderecosGerador == null)
            {
                return Problem("Entity set 'MeuDbContext.EnderecosGerador'  is null.");
            }
            var enderecosGerador = await _context.EnderecosGerador.FindAsync(id);
            if (enderecosGerador != null)
            {
                _context.EnderecosGerador.Remove(enderecosGerador);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EnderecosGeradorExists(int id)
        {
          return _context.EnderecosGerador.Any(e => e.Id == id);
        }
    }
}
