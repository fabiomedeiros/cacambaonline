using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using cacambaonline.ViewModels;
using cacambaonline.Models;
//using dev.app.Data;
//using dev.data.Context;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace dev.app.Controllers
{
    [Authorize(Roles = "Adm,Gestor")]
    public class RolesController : Controller
    {
             
        RoleManager<IdentityRole> roleManager;
        private readonly UserManager<IdentityUser> _userManager ;
        


        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            this.roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var roles = roleManager.Roles.ToList();
            return View(roles);
        }

        public IActionResult Gerenciar()
        {

            var usuarios = _userManager.Users.ToList();
            var usuarioRoles = new List<Tuple<string, IList<string>>>();

            

            foreach (var usuario in usuarios)
            {
                 usuarioRoles.Add( new Tuple<string, IList<string>>(usuario.Email, _userManager.GetRolesAsync(usuario).Result));
            }


            // return View(new { Usuarios = usuarios, Roles = usuarioRoles });

            return View(usuarioRoles);
            
        }
        [HttpPost]
        public async Task<IActionResult> GerenciarEditar(string email)
        {
                       
            try
            {
                var usuario = await _userManager.FindByEmailAsync(email);
                var roles = roleManager.Roles.ToList();
                var role = _userManager.GetRolesAsync(usuario).Result.FirstOrDefault();

                var teste = new RolesViewModel { Usuario = usuario, Roles = roles, Role = role, };
                return View(teste);
            }
            catch
            {
                return NotFound();
            }
                          
        }

        [HttpPost]
        public async Task<IActionResult> GerenciarEditarConfirm(string id,string perfil, string role)
        {
            try
            {
               // Console.WriteLine(id + "\n" + perfil + "\n"+role);
                var usuario = await _userManager.FindByIdAsync(id);

                //perfil = usuario

                if (role != null)
                { 
                    await _userManager.RemoveFromRoleAsync(usuario, role);
                }

                await _userManager.AddToRoleAsync(usuario, perfil);

                await _userManager.UpdateAsync(usuario);
            }
            catch
            {

            }

            return RedirectToAction("Gerenciar", "Roles");

        }
        public IActionResult Criar()
        {
            return View(new IdentityRole());
        }

        [HttpPost]
        public async Task<IActionResult> Criar(IdentityRole role)
        {
            await roleManager.CreateAsync(role);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Excluir(string id)
        {

            var role = await roleManager.FindByIdAsync(id);

            try
            {
                await roleManager.DeleteAsync(role);
            }
            catch
            {
              
            }
            return RedirectToAction("Index");
                       
        }

        
        public async Task<IActionResult> Editar(string id)
        {

            var role = await roleManager.FindByIdAsync(id);

            if (role == null)
                return NotFound();
            return View(role);
        }

        public async Task<IActionResult> EditarConfirm(string id, string Name)
        {

            var role = await roleManager.FindByIdAsync(id);
            
            if (role == null)
                return NotFound();

            try
            {
                role.Name = Name;
                await roleManager.UpdateAsync(role);
            }
            catch
            {

            }

            return RedirectToAction("index" ,"Roles");
        }


    }
}
