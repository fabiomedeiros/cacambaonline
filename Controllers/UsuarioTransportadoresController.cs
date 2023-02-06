using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using cacambaonline.Data;
using cacambaonline.Models;
using Microsoft.AspNetCore.Identity;
using cacambaonline.Areas.Identity.Data;

namespace cacambaonline.Controllers
{
    public class UsuarioTransportadoresController : Controller
    {
        private readonly MeuDbContext _context;
        private readonly IdentityContext _identitycontext;
        public readonly UserManager<IdentityUser> _UserManager;
        public readonly RoleManager<IdentityRole> _RoleManager;
        

        public UsuarioTransportadoresController(MeuDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IdentityContext identitycontext)
        {
            _context = context;
            _UserManager = userManager;
            _RoleManager = roleManager;
            _identitycontext = identitycontext;
        }

        // GET: UsuarioTransportadores
        public async Task<IActionResult> Index()
        {
            var meuDbContext = _context.UsuarioTransportadores.Include(u => u.Transportadores);

            ViewBag.QueryUser = new SelectList(_identitycontext.Users, "Id", "Email");

            return View(await meuDbContext.ToListAsync());
        }

        // GET: UsuarioTransportadores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.UsuarioTransportadores == null)
            {
                return NotFound();
            }
            ViewBag.QueryUser = new SelectList(_identitycontext.Users, "Id", "Email");
            var usuarioTransportador = await _context.UsuarioTransportadores
                .Include(u => u.Transportadores)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuarioTransportador == null)
            {
                return NotFound();
            }

            return View(usuarioTransportador);
        }

        // GET: UsuarioTransportadores/Create
        public IActionResult Create()
        {
           // var usuarios = _UserManager.Users.ToList();

            //var usuarios = _UserManager.GetUsersInRoleAsync("Transportador");

            //var usuarios = _UserManager.IsInRoleAsync()
            //_userManager.AddToRoleAsync

            //var users = (from user in _UserManager.Users
            //                  join userRole in _RoleManager.user
            //                  on user.Id equals userRole.UserId
            //                  join role in _dbContext.Roles
            //                  on userRole.RoleId equals role.Id
            //                  where role.Name == "User"
            //                  select user)
            //                     .ToListAsync();

            var users = from userrole in _identitycontext.UserRoles
                        join user in _identitycontext.Users on userrole.UserId equals user.Id
                        join role in _identitycontext.Roles on userrole.RoleId equals role.Id
                        select new {userrole.UserId, userrole.RoleId,user.Email,role.Name };


            ViewData["UsuariosId"] = new SelectList(users.Where(x=>x.Name == "Transportador"), "UserId", "Email");
            ViewData["TransportadoresId"] = new SelectList(_context.Transportadores, "Id", "NomeFantasia");
            return View();
        }

        // POST: UsuarioTransportadores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UsuarioTransportador usuarioTransportador)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usuarioTransportador);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TransportadoresId"] = new SelectList(_context.Transportadores, "Id", "Id", usuarioTransportador.TransportadoresId);
            return View(usuarioTransportador);
        }

        // GET: UsuarioTransportadores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.QueryUser = new SelectList(_identitycontext.Users, "Id", "Email");

            if (id == null || _context.UsuarioTransportadores == null)
            {
                return NotFound();
            }

            var usuarioTransportador = await _context.UsuarioTransportadores.FindAsync(id);
            if (usuarioTransportador == null)
            {
                return NotFound();
            }
            ViewData["TransportadoresId"] = new SelectList(_context.Transportadores, "Id", "NomeFantasia", usuarioTransportador.TransportadoresId);
            return View(usuarioTransportador);
        }

        // POST: UsuarioTransportadores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,TransportadoresId")] UsuarioTransportador usuarioTransportador)
        {
            if (id != usuarioTransportador.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuarioTransportador);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioTransportadorExists(usuarioTransportador.Id))
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
            ViewData["TransportadoresId"] = new SelectList(_context.Transportadores, "Id", "Id", usuarioTransportador.TransportadoresId);
            return View(usuarioTransportador);
        }

        // GET: UsuarioTransportadores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.UsuarioTransportadores == null)
            {
                return NotFound();
            }

            var usuarioTransportador = await _context.UsuarioTransportadores
                .Include(u => u.Transportadores)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuarioTransportador == null)
            {
                return NotFound();
            }

            return View(usuarioTransportador);
        }

        // POST: UsuarioTransportadores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.UsuarioTransportadores == null)
            {
                return Problem("Entity set 'MeuDbContext.UsuarioTransportadores'  is null.");
            }
            var usuarioTransportador = await _context.UsuarioTransportadores.FindAsync(id);
            if (usuarioTransportador != null)
            {
                _context.UsuarioTransportadores.Remove(usuarioTransportador);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioTransportadorExists(int id)
        {
          return _context.UsuarioTransportadores.Any(e => e.Id == id);
        }
    }
}
