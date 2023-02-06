using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using cacambaonline.Data;
using cacambaonline.Models;
using cacambaonline.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;

namespace cacambaonline.Controllers
{
    public class UsuarioDestinatariosController : Controller
    {
        private readonly MeuDbContext _context;
        private readonly IdentityContext _identitycontext;
        public readonly UserManager<IdentityUser> _UserManager;
        public readonly RoleManager<IdentityRole> _RoleManager;

        public UsuarioDestinatariosController(MeuDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IdentityContext identitycontext)
        {
            _context = context;
            _UserManager = userManager;
            _RoleManager = roleManager;
            _identitycontext = identitycontext;
        }

        // GET: UsuarioDestinatarios
        public async Task<IActionResult> Index()
        {
            var meuDbContext = _context.UsuarioDestinatarios.Include(u => u.Destinatarios);


            ViewBag.QueryUser = new SelectList(_identitycontext.Users, "Id", "Email");


            return View(await meuDbContext.ToListAsync());
        }

        // GET: UsuarioDestinatarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.UsuarioDestinatarios == null)
            {
                return NotFound();
            }
            ViewBag.QueryUser = new SelectList(_identitycontext.Users, "Id", "Email");

            var usuarioDestinatario = await _context.UsuarioDestinatarios
                .Include(u => u.Destinatarios)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuarioDestinatario == null)
            {
                return NotFound();
            }

            return View(usuarioDestinatario);
        }

        // GET: UsuarioDestinatarios/Create
        public IActionResult Create()
        {
            var users = from userrole in _identitycontext.UserRoles
                        join user in _identitycontext.Users on userrole.UserId equals user.Id
                        join role in _identitycontext.Roles on userrole.RoleId equals role.Id
                        select new { userrole.UserId, userrole.RoleId, user.Email, role.Name };


            ViewData["UsuariosId"] = new SelectList(users.Where(x => x.Name == "Destinatario"), "UserId", "Email");

            ViewData["DestinatariosId"] = new SelectList(_context.Destinatarios, "Id", "NomeFantasia");
            return View();
        }

        // POST: UsuarioDestinatarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( UsuarioDestinatario usuarioDestinatario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usuarioDestinatario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DestinatariosId"] = new SelectList(_context.Destinatarios, "Id", "NomeFantasia", usuarioDestinatario.DestinatariosId);
            return View(usuarioDestinatario);
        }

        // GET: UsuarioDestinatarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.UsuarioDestinatarios == null)
            {
                return NotFound();
            }

            var usuarioDestinatario = await _context.UsuarioDestinatarios.FindAsync(id);
            if (usuarioDestinatario == null)
            {
                return NotFound();
            }
            ViewData["DestinatariosId"] = new SelectList(_context.Destinatarios, "Id", "NomeFantasia", usuarioDestinatario.DestinatariosId);
            return View(usuarioDestinatario);
        }

        // POST: UsuarioDestinatarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UsuarioDestinatario usuarioDestinatario)
        {
            if (id != usuarioDestinatario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuarioDestinatario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioDestinatarioExists(usuarioDestinatario.Id))
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
            ViewData["DestinatariosId"] = new SelectList(_context.Destinatarios, "Id", "NomeFantasia", usuarioDestinatario.DestinatariosId);
            return View(usuarioDestinatario);
        }

        // GET: UsuarioDestinatarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.UsuarioDestinatarios == null)
            {
                return NotFound();
            }

            var usuarioDestinatario = await _context.UsuarioDestinatarios
                .Include(u => u.Destinatarios)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuarioDestinatario == null)
            {
                return NotFound();
            }

            return View(usuarioDestinatario);
        }

        // POST: UsuarioDestinatarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.UsuarioDestinatarios == null)
            {
                return Problem("Entity set 'MeuDbContext.UsuarioDestinatarios'  is null.");
            }
            var usuarioDestinatario = await _context.UsuarioDestinatarios.FindAsync(id);
            if (usuarioDestinatario != null)
            {
                _context.UsuarioDestinatarios.Remove(usuarioDestinatario);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioDestinatarioExists(int id)
        {
          return _context.UsuarioDestinatarios.Any(e => e.Id == id);
        }
    }
}
