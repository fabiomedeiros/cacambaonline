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
    public class EnderecosDestinatariosController : Controller
    {
        private readonly MeuDbContext _context;

        public EnderecosDestinatariosController(MeuDbContext context)
        {
            _context = context;
        }

        // GET: EnderecosDestinatarios
        public async Task<IActionResult> Index()
        {
            var meuDbContext = _context.EnderecosDestinatario.Include(e => e.Destinatarios);
            return View(await meuDbContext.ToListAsync());
        }

        // GET: EnderecosDestinatarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.EnderecosDestinatario == null)
            {
                return NotFound();
            }

            var enderecosDestinatario = await _context.EnderecosDestinatario
                .Include(e => e.Destinatarios)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (enderecosDestinatario == null)
            {
                return NotFound();
            }

            return View(enderecosDestinatario);
        }

        // GET: EnderecosDestinatarios/Create
        public IActionResult Create()
        {
            ViewData["DestinatariosId"] = new SelectList(_context.Destinatarios, "Id", "Id");
            return View();
        }

        // POST: EnderecosDestinatarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CEP,Cidade,Estado,Bairro,Logradouro,Numero,Complemento,Latitude,Longitude,DestinatariosId")] EnderecosDestinatario enderecosDestinatario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(enderecosDestinatario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DestinatariosId"] = new SelectList(_context.Destinatarios, "Id", "Id", enderecosDestinatario.DestinatariosId);
            return View(enderecosDestinatario);
        }

        // GET: EnderecosDestinatarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.EnderecosDestinatario == null)
            {
                return NotFound();
            }

            var enderecosDestinatario = await _context.EnderecosDestinatario.FindAsync(id);
            if (enderecosDestinatario == null)
            {
                return NotFound();
            }
            ViewData["DestinatariosId"] = new SelectList(_context.Destinatarios, "Id", "Id", enderecosDestinatario.DestinatariosId);
            return View(enderecosDestinatario);
        }

        // POST: EnderecosDestinatarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CEP,Cidade,Estado,Bairro,Logradouro,Numero,Complemento,Latitude,Longitude,DestinatariosId")] EnderecosDestinatario enderecosDestinatario)
        {
            if (id != enderecosDestinatario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(enderecosDestinatario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnderecosDestinatarioExists(enderecosDestinatario.Id))
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
            ViewData["DestinatariosId"] = new SelectList(_context.Destinatarios, "Id", "Id", enderecosDestinatario.DestinatariosId);
            return View(enderecosDestinatario);
        }

        // GET: EnderecosDestinatarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.EnderecosDestinatario == null)
            {
                return NotFound();
            }

            var enderecosDestinatario = await _context.EnderecosDestinatario
                .Include(e => e.Destinatarios)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (enderecosDestinatario == null)
            {
                return NotFound();
            }

            return View(enderecosDestinatario);
        }

        // POST: EnderecosDestinatarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.EnderecosDestinatario == null)
            {
                return Problem("Entity set 'MeuDbContext.EnderecosDestinatario'  is null.");
            }
            var enderecosDestinatario = await _context.EnderecosDestinatario.FindAsync(id);
            if (enderecosDestinatario != null)
            {
                _context.EnderecosDestinatario.Remove(enderecosDestinatario);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EnderecosDestinatarioExists(int id)
        {
          return _context.EnderecosDestinatario.Any(e => e.Id == id);
        }
    }
}
