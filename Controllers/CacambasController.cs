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
using System.Security.Claims;
using cacambaonline.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;

namespace cacambaonline.Controllers
{
   
    public class CacambasController : Controller
    {
        private readonly MeuDbContext _context;
        private readonly IdentityContext _identitycontext;

        public CacambasController(MeuDbContext context, IdentityContext identitycontext)
        {
            _context = context;
            _identitycontext = identitycontext;
        }

        // GET: Cacambas
        [Authorize(Roles = "Adm,Gestor,Transportador")]
        public async Task<IActionResult> Index()
        {
            var meuDbContext = _context.Cacambas.Include(c => c.Transportadores);
            return View(await meuDbContext.ToListAsync());
        }

        // GET: Cacambas/Details/5
        [Authorize(Roles = "Adm,Gestor,Transportador")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Cacambas == null)
            {
                return NotFound();
            }

            var cacambas = await _context.Cacambas
                .Include(c => c.Transportadores)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cacambas == null)
            {
                return NotFound();
            }

            return View(cacambas);
        }

        // GET: Cacambas/Create
        [Authorize(Roles = "Adm,Gestor,Transportador")]
        public IActionResult Create()
        {
            //Pega o e-mail do logado
            string iduser = User.FindFirstValue(ClaimTypes.NameIdentifier);



            //var transport = _context.UsuarioTransportadores
            //    .Include(c => c.Transportadores)
            //    ;


            if (User.IsInRole("Transportador"))
            {
                var transportadora = from usertrasnp in _context.UsuarioTransportadores
                                     join transp in _context.Transportadores on usertrasnp.TransportadoresId equals transp.Id
                                     select new { transp.Id, transp.NomeFantasia, usertrasnp.UserId };

                ViewData["TransportadoresId"] = new SelectList(transportadora.Where(x => x.UserId == iduser), "Id", "NomeFantasia");
            }
            else
            {
                ViewData["TransportadoresId"] = new SelectList(_context.Transportadores, "Id", "NomeFantasia");
            }
            return View();
        }

        // POST: Cacambas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Adm,Gestor,Transportador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cacambas cacambas)
        {

            //var transportadora = from transp in _context.Transportadores
            //                     select new { transp.Id, transp.NomeFantasia };

            var transportadora = _context.Transportadores.FirstOrDefault(x => x.Id == cacambas.TransportadoresId);


            //string x = transportadora.Any(x => x.Id == cacambas.TransportadoresId);

            if (ModelState.IsValid)
            {
                cacambas.Descricao = cacambas.Descricao + " - " + transportadora.NomeFantasia;

                _context.Add(cacambas);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TransportadoresId"] = new SelectList(_context.Transportadores, "Id", "Id", cacambas.TransportadoresId);
            return View(cacambas);
        }

        // GET: Cacambas/Edit/5
        [Authorize(Roles = "Adm,Gestor,Transportador")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Cacambas == null)
            {
                return NotFound();
            }

            var cacambas = await _context.Cacambas.FindAsync(id);
            if (cacambas == null)
            {
                return NotFound();
            }
            ViewData["TransportadoresId"] = new SelectList(_context.Transportadores, "Id", "Id", cacambas.TransportadoresId);
            return View(cacambas);
        }

        // POST: Cacambas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Numero,Descricao,Obs,TransportadoresId")] Cacambas cacambas)
        {
            if (id != cacambas.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cacambas);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CacambasExists(cacambas.Id))
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
            ViewData["TransportadoresId"] = new SelectList(_context.Transportadores, "Id", "Id", cacambas.TransportadoresId);
            return View(cacambas);
        }

        // GET: Cacambas/Delete/5
        [Authorize(Roles = "Adm,Gestor")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Cacambas == null)
            {
                return NotFound();
            }

            var cacambas = await _context.Cacambas
                .Include(c => c.Transportadores)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cacambas == null)
            {
                return NotFound();
            }

            return View(cacambas);
        }

        // POST: Cacambas/Delete/5
        [Authorize(Roles = "Adm,Gestor")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Cacambas == null)
            {
                return Problem("Entity set 'MeuDbContext.Cacambas'  is null.");
            }
            var cacambas = await _context.Cacambas.FindAsync(id);
            if (cacambas != null)
            {
                _context.Cacambas.Remove(cacambas);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CacambasExists(int id)
        {
          return _context.Cacambas.Any(e => e.Id == id);
        }
    }
}
