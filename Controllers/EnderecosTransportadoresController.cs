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
    public class EnderecosTransportadoresController : Controller
    {
        private readonly MeuDbContext _context;

        public EnderecosTransportadoresController(MeuDbContext context)
        {
            _context = context;
        }

        // GET: EnderecosTransportadores
        public async Task<IActionResult> Index()
        {
            var meuDbContext = _context.EnderecosTransportador.Include(e => e.Transportadores);
            return View(await meuDbContext.ToListAsync());
        }

        // GET: EnderecosTransportadores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.EnderecosTransportador == null)
            {
                return NotFound();
            }

            var enderecosTransportador = await _context.EnderecosTransportador
                .Include(e => e.Transportadores)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (enderecosTransportador == null)
            {
                return NotFound();
            }

            return View(enderecosTransportador);
        }

        // GET: EnderecosTransportadores/Create
        public IActionResult Create()
        {
            ViewData["TransportadoresId"] = new SelectList(_context.Transportadores, "Id", "Id");
            return View();
        }

        // POST: EnderecosTransportadores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CEP,Cidade,Estado,Bairro,Logradouro,Numero,Complemento,Latitude,Longitude,TransportadoresId")] EnderecosTransportador enderecosTransportador)
        {
            if (ModelState.IsValid)
            {
                _context.Add(enderecosTransportador);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TransportadoresId"] = new SelectList(_context.Transportadores, "Id", "Id", enderecosTransportador.TransportadoresId);
            return View(enderecosTransportador);
        }

        // GET: EnderecosTransportadores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.EnderecosTransportador == null)
            {
                return NotFound();
            }

            var enderecosTransportador = await _context.EnderecosTransportador.FindAsync(id);
            if (enderecosTransportador == null)
            {
                return NotFound();
            }
            ViewData["TransportadoresId"] = new SelectList(_context.Transportadores, "Id", "Id", enderecosTransportador.TransportadoresId);
            return View(enderecosTransportador);
        }

        // POST: EnderecosTransportadores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CEP,Cidade,Estado,Bairro,Logradouro,Numero,Complemento,Latitude,Longitude,TransportadoresId")] EnderecosTransportador enderecosTransportador)
        {
            if (id != enderecosTransportador.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(enderecosTransportador);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnderecosTransportadorExists(enderecosTransportador.Id))
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
            ViewData["TransportadoresId"] = new SelectList(_context.Transportadores, "Id", "Id", enderecosTransportador.TransportadoresId);
            return View(enderecosTransportador);
        }

        // GET: EnderecosTransportadores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.EnderecosTransportador == null)
            {
                return NotFound();
            }

            var enderecosTransportador = await _context.EnderecosTransportador
                .Include(e => e.Transportadores)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (enderecosTransportador == null)
            {
                return NotFound();
            }

            return View(enderecosTransportador);
        }

        // POST: EnderecosTransportadores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.EnderecosTransportador == null)
            {
                return Problem("Entity set 'MeuDbContext.EnderecosTransportador'  is null.");
            }
            var enderecosTransportador = await _context.EnderecosTransportador.FindAsync(id);
            if (enderecosTransportador != null)
            {
                _context.EnderecosTransportador.Remove(enderecosTransportador);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EnderecosTransportadorExists(int id)
        {
          return _context.EnderecosTransportador.Any(e => e.Id == id);
        }
    }
}
