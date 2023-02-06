using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using cacambaonline.Data;
using cacambaonline.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace cacambaonline.Controllers
{
    public class InfracoesController : Controller
    {
        private readonly MeuDbContext _context;

        public InfracoesController(MeuDbContext context)
        {
            _context = context;
        }

        // GET: Infracoes
        [Authorize(Roles = "Adm,Fiscal,Gestor,Transportador")]
        public async Task<IActionResult> Index()
        {
            var meuDbContext = _context.Infracoes;

            //var meuDbContexttransportador = _context.Notificacoes.Include(n => n.Ctr).Where(x => x.Ctr.UsuarioId == User.FindFirstValue(ClaimTypes.NameIdentifier));

            

            if (User.IsInRole("Transportador"))
            {
                return View(await meuDbContext.ToListAsync());
            }

            return View(await meuDbContext.ToListAsync());
        }

        // GET: Infracoes/Details/5
        [Authorize(Roles = "Adm,Fiscal,Gestor,Transportador")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Infracoes == null)
            {
                return NotFound();
            }

            var infracoes = await _context.Infracoes
                //.Include(i => i.Ctr)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (infracoes == null)
            {
                return NotFound();
            }

            return View(infracoes);
        }

        // GET: Infracoes/Create
        public IActionResult Create()
        {
           // ViewData["CTRId"] = new SelectList(_context.CTR, "Id", "Id");
            return View();
        }

        // POST: Infracoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Descricao,Obs,Prazo,CTRId,UsuarioFiscalId")] Infracoes infracoes)
        {
            if (ModelState.IsValid)
            {
                _context.Add(infracoes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
          //  ViewData["CTRId"] = new SelectList(_context.CTR, "Id", "Id", infracoes.CTRId);
            return View(infracoes);
        }

        // GET: Infracoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Infracoes == null)
            {
                return NotFound();
            }

            var infracoes = await _context.Infracoes.FindAsync(id);
            if (infracoes == null)
            {
                return NotFound();
            }
         //   ViewData["CTRId"] = new SelectList(_context.CTR, "Id", "Id", infracoes.CTRId);
            return View(infracoes);
        }

        // POST: Infracoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descricao,Obs,Prazo,CTRId,UsuarioFiscalId")] Infracoes infracoes)
        {
            if (id != infracoes.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(infracoes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InfracoesExists(infracoes.Id))
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
            //ViewData["CTRId"] = new SelectList(_context.CTR, "Id", "Id", infracoes.CTRId);
            return View(infracoes);
        }

        // GET: Infracoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Infracoes == null)
            {
                return NotFound();
            }

            var infracoes = await _context.Infracoes
                
                .FirstOrDefaultAsync(m => m.Id == id);
            if (infracoes == null)
            {
                return NotFound();
            }

            return View(infracoes);
        }

        // POST: Infracoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Infracoes == null)
            {
                return Problem("Entity set 'MeuDbContext.Infracoes'  is null.");
            }
            var infracoes = await _context.Infracoes.FindAsync(id);
            if (infracoes != null)
            {
                _context.Infracoes.Remove(infracoes);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InfracoesExists(int id)
        {
          return _context.Infracoes.Any(e => e.Id == id);
        }
    }
}
