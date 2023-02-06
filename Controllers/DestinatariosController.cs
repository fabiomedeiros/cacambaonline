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
    public class DestinatariosController : Controller
    {
        private readonly MeuDbContext _context;
        public readonly UserManager<IdentityUser> _UserManager;
        public readonly RoleManager<IdentityRole> _RoleManager;


        public DestinatariosController(MeuDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _UserManager = userManager;
            _RoleManager = roleManager;
        }


        // GET: Destinatarios
        public async Task<IActionResult> Index()
        {
              return View(await _context.Destinatarios.ToListAsync());
        }

        // GET: Destinatarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Destinatarios == null)
            {
                return NotFound();
            }

            var destinatarios = await _context.Destinatarios
                .FirstOrDefaultAsync(m => m.Id == id);

            if (destinatarios == null)
            {
                return NotFound();
            }

            return View(destinatarios);
        }

        // GET: Destinatarios/Create
        public IActionResult Create()
        {

            //Capturar a lista de usuários para ser exibido e escolhido na hora de Cadastrar  o Destinatário
            //var model = new List<Usuarios>();
            //var listaUsuarios = _UserManager.Users.ToList();

            //foreach (var user in listaUsuarios)
            //{
            //    var usuarios = new Usuarios
            //    {
            //        Id = user.Id,
            //        UserName = user.UserName,
            //        Email = user.Email
            //    };
            //    model.Add(usuarios);
            //}

            //ViewBag.UsuarioId = new SelectList(model, "Id", "UserName");

            return View();
        }

        // POST: Destinatarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Destinatarios destinatarios)
        {
            if (ModelState.IsValid)
            {
                _context.Add(destinatarios);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(destinatarios);
        }

        // GET: Destinatarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Destinatarios == null)
            {
                return NotFound();
            }

            var destinatarios = await _context.Destinatarios.FindAsync(id);
            if (destinatarios == null)
            {
                return NotFound();
            }
            return View(destinatarios);
        }

        // POST: Destinatarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Cnpj,NomeEmpresarial,NomeFantasia,Telefone,Email,EnderecoEletronico")] Destinatarios destinatarios)
        {
            if (id != destinatarios.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(destinatarios);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DestinatariosExists(destinatarios.Id))
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
            return View(destinatarios);
        }

        // GET: Destinatarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Destinatarios == null)
            {
                return NotFound();
            }

            var destinatarios = await _context.Destinatarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (destinatarios == null)
            {
                return NotFound();
            }

            return View(destinatarios);
        }

        // POST: Destinatarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Destinatarios == null)
            {
                return Problem("Entity set 'MeuDbContext.Destinatarios'  is null.");
            }
            var destinatarios = await _context.Destinatarios.FindAsync(id);
            if (destinatarios != null)
            {
                _context.Destinatarios.Remove(destinatarios);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DestinatariosExists(int id)
        {
          return _context.Destinatarios.Any(e => e.Id == id);
        }
    }
}
