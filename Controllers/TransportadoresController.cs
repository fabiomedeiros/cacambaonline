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
using Microsoft.AspNetCore.Identity;

namespace cacambaonline.Controllers
{
    [Authorize(Roles = "Adm,Gestor")]
    public class TransportadoresController : Controller
    {
        private readonly MeuDbContext _context;
        public readonly UserManager<IdentityUser> _UserManager;
        public readonly RoleManager<IdentityRole> _RoleManager;


        public TransportadoresController(MeuDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _UserManager = userManager;
            _RoleManager = roleManager;
        }

        // GET: Transportadores
        public async Task<IActionResult> Index()
        {
              return View(await _context.Transportadores.ToListAsync());
        }

        [AllowAnonymous]
        public async Task<IActionResult> Inicial()
        {
            return View();
        }

        // GET: Transportadores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Transportadores == null)
            {
                return NotFound();
            }

            var transportadores = await _context.Transportadores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transportadores == null)
            {
                return NotFound();
            }

            return View(transportadores);
        }

        // GET: Transportadores/Create
        [AllowAnonymous]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Transportadores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Transportadores transportadores)
        {
            string userid = Request.Query["userid"];

            var applicationRole = await _RoleManager.FindByNameAsync("Transportador");
            if (applicationRole != null)
            {
                //IdentityResult roleResult = await _UserManager.AddToRoleAsync(userid, applicationRole.Name);
            }

            if (ModelState.IsValid)
            {
                _context.Add(transportadores);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(transportadores);
        }

        // GET: Transportadores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Transportadores == null)
            {
                return NotFound();
            }

            var transportadores = await _context.Transportadores.FindAsync(id);
            if (transportadores == null)
            {
                return NotFound();
            }
            return View(transportadores);
        }

        // POST: Transportadores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Cnpj,NomeEmpresarial,NomeFantasia,Telefone,Email,EnderecoEletronico")] Transportadores transportadores)
        {
            if (id != transportadores.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transportadores);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransportadoresExists(transportadores.Id))
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
            return View(transportadores);
        }

        // GET: Transportadores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Transportadores == null)
            {
                return NotFound();
            }

            var transportadores = await _context.Transportadores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transportadores == null)
            {
                return NotFound();
            }

            return View(transportadores);
        }

        // POST: Transportadores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Transportadores == null)
            {
                return Problem("Entity set 'MeuDbContext.Transportadores'  is null.");
            }
            var transportadores = await _context.Transportadores.FindAsync(id);
            if (transportadores != null)
            {
                _context.Transportadores.Remove(transportadores);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransportadoresExists(int id)
        {
          return _context.Transportadores.Any(e => e.Id == id);
        }
    }
}
