using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using cacambaonline.Data;
using cacambaonline.Models;
using System.Timers;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using cacambaonline.Areas.Identity.Data;
using PdfSharpCore.Drawing;

namespace cacambaonline.Controllers
{
    public class CTRsController : Controller
    {
        private readonly MeuDbContext _context;
        private readonly IWebHostEnvironment _webHostEnv;

        public CTRsController(MeuDbContext context, IWebHostEnvironment webHostEnvt)
        {
            _context = context;
            _webHostEnv = webHostEnvt;
        }

        // GET: CTRs
        [Authorize(Roles = "Adm,Fiscal,Gestor,Transportador,Destinatario")]
        public async Task<IActionResult> Index()
        {
            var meuDbContext = _context.CTR.Include(c => c.Cacambas).Include(c => c.Destinatarios).Include(c => c.Transportadores);

            var meuDbContexttransportador = _context.CTR.Include(c => c.Cacambas).Include(c => c.Destinatarios).Include(c => c.Transportadores).Where(x => x.UsuarioId == User.FindFirstValue(ClaimTypes.NameIdentifier));

            string busca = Request.Query["busca"];

            var query = from s in meuDbContext
                        where (EF.Functions.Like(s.Cacambas.Descricao, "%" + busca + "%") || EF.Functions.Like(s.Destinatarios.NomeFantasia, "%" + busca + "%") || EF.Functions.Like(s.Transportadores.NomeFantasia, "%" + busca + "%"))
                        select s;

            var role = this.User.FindFirstValue(ClaimTypes.Role);

            //if (role =="Transportador")
            if(User.IsInRole("Transportador"))
            {
                return View(await meuDbContexttransportador.Where(x=>(x.Fechada==false || x.Fechada == null) && string.IsNullOrWhiteSpace(x.VeiodaNotificacao)).ToListAsync());
                //return View(await meuDbContexttransportador.ToListAsync());
            }

            if (User.IsInRole("Destinatario"))
            {
                ///var cTR = await _context.CTR.FindAsync(id);

                var codusuario = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var destinatario = _context.UsuarioDestinatarios.Where(x => x.UserId == codusuario);

                return View(await meuDbContext.Where(x => ((x.Fechada == false || x.Fechada == null) && string.IsNullOrWhiteSpace(x.VeiodaNotificacao)) && x.DestinatariosId == destinatario.FirstOrDefault().DestinatariosId).ToListAsync());
                //return View(await meuDbContexttransportador.ToListAsync());
            }


            if (string.IsNullOrEmpty(busca))
            {
                return View(await meuDbContext.Where(x => (x.Fechada == false || x.Fechada == null) && string.IsNullOrWhiteSpace(x.VeiodaNotificacao)).ToListAsync());
            }
            else
            {
                return View( query.Where(x => (x.Fechada == false || x.Fechada == null) && string.IsNullOrWhiteSpace(x.VeiodaNotificacao)).OrderByDescending(p => p.Id));
            }


        }

        [Authorize(Roles = "Adm,Gestor,Fiscal")]
        public async Task<IActionResult> Geolocalizacao()
        {
            var meuDbContext = _context.CTR.Include(c => c.Cacambas).Include(c => c.Destinatarios).Include(c => c.Transportadores);

            ViewData["Latitude"] = new SelectList(_context.Cacambas, "Id", "Latitude");
            ViewData["Longitude"] = new SelectList(_context.Cacambas, "Id", "Longitude");

            string x = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string y = User.FindFirstValue(ClaimTypes.Email);

            return View(await meuDbContext.ToListAsync());
        }


        // GET: CTRs/Details/5
        [Authorize(Roles = "Adm,Fiscal,Gestor,Transportador,Destinatario")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CTR == null)
            {
                return NotFound();
            }

            var cTR = await _context.CTR
                .Include(c => c.Cacambas)
                .Include(c => c.Destinatarios)
                .Include(c => c.Transportadores)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cTR == null)
            {
                return NotFound();
            }

            return View(cTR);
        }

        // GET: CTRs/Create
        [Authorize(Roles = "Adm,Transportador")]
        public IActionResult Create()
        {
            
            var transportadora = from usertransp in _context.UsuarioTransportadores
                                 join tranp in _context.Transportadores on usertransp.TransportadoresId equals tranp.Id
                                 select new { usertransp.UserId, usertransp.TransportadoresId, tranp.NomeFantasia };

            var cacamba = from ctr in _context.CTR
                                 join cacambas in _context.Cacambas on ctr.CacambasId equals cacambas.Id
                                 select new { ctr.Fechada,cacambas.Id,cacambas.Descricao };


            ViewData["DestinatariosId"] = new SelectList(_context.Destinatarios, "Id", "NomeFantasia");
            ViewData["TransportadoresId"] = new SelectList(transportadora.Where(x => x.UserId == this.User.FindFirstValue(ClaimTypes.NameIdentifier)), "TransportadoresId", "NomeFantasia","Selecione");
            ViewData["CacambasId"] = new SelectList(cacamba.Where(x=>x.Fechada ==false && x.Fechada ==null), "Id", "Descricao");
            return View();
        }

        // POST: CTRs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Adm,Transportador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( CTR cTR)
        {
            var Ctrbanco = await _context.CTR.FirstOrDefaultAsync(m => m.CacambasId == cTR.CacambasId && m.Fechada == false || m.Fechada == null);
       
            if (ModelState.IsValid)
            {
                if (!_context.CTR.Any(m => m.CacambasId == cTR.CacambasId && (m.Fechada == false || m.Fechada == null)))
                { 
                    cTR.Data = DateTime.Now;
                    cTR.Fechada = false;
                    cTR.UsuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    _context.Add(cTR);
                    await _context.SaveChangesAsync();

                    var cacamba = await _context.Cacambas.FindAsync(cTR.CacambasId);
                    cacamba.Ocupada = true;
                    //_context.Add(cTR);
                    _context.Update(cacamba);
                    await _context.SaveChangesAsync();


                    //CTR_NOVO
                    CTR_novo logctrnovo = new CTR_novo();
                    logctrnovo.Localizacao = cTR.Localizacao;
                    logctrnovo.Latitude = cTR.Latitude;
                    logctrnovo.Longitude = cTR.Longitude;
                    logctrnovo.Data = DateTime.Now;
                    logctrnovo.TransportadoresId = cTR.TransportadoresId;
                    logctrnovo.CacambasId = cTR.CacambasId;
                    logctrnovo.DestinatariosId = cTR.DestinatariosId;
                    logctrnovo.UsuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier); 
                    logctrnovo.Fechada = cTR.Fechada;
                    logctrnovo.Excluido = cTR.Excluido;
                    logctrnovo.Notificado = cTR.Notificado;
                    logctrnovo.Multado = cTR.Multado;
                    _context.Add(logctrnovo);
                    await _context.SaveChangesAsync();

                    

                    //Salvar o Log 
                    LogCTR logctr = new LogCTR();
                    logctr.Data = DateTime.Now;
                    logctr.UsuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    logctr.CTRId = cTR.Id;
                    logctr.Operacao = "Novo";
                    logctr.CTR_novoId = logctrnovo.Id;
                    logctr.CTR_antigoId = null;
                    _context.Add(logctr);
                    await _context.SaveChangesAsync();


                    return RedirectToAction(nameof(Index));
                    //FIM LOG

                    //ViewData["CacambasId"] = new SelectList(_context.Cacambas, "Id", "Id", cTR.CacambasId);
                    //ViewData["DestinatariosId"] = new SelectList(_context.Destinatarios, "Id", "Id", cTR.DestinatariosId);
                    //ViewData["TransportadoresId"] = new SelectList(_context.Transportadores, "Id", "Id", cTR.TransportadoresId);

                    // return RedirectToAction(nameof(Index));
                    //return View();
                    //Console.WriteLine("estou aqui");
                    //return Redirect("/CTRs/Index");
                    //return View(cTR);

                }
                else
                {
                    ViewData["alerta"] = "A caçamba já está em uso, ou seja, está em uma CTR aberta.";
                    ViewData["CacambasId"] = new SelectList(_context.Cacambas, "Id", "Descricao");
                    ViewData["DestinatariosId"] = new SelectList(_context.Destinatarios, "Id", "NomeFantasia");
                    ViewData["TransportadoresId"] = new SelectList(_context.Transportadores, "Id", "NomeFantasia");

                    return View(cTR);
                }
               
            }
            ViewData["CacambasId"] = new SelectList(_context.Cacambas, "Id", "Descricao");
            ViewData["DestinatariosId"] = new SelectList(_context.Destinatarios, "Id", "NomeFantasia");
            ViewData["TransportadoresId"] = new SelectList(_context.Transportadores, "Id", "NomeFantasia");

            return View(cTR);
        }

        // GET: CTRs/Edit/5
        [Authorize(Roles = "Adm,Gestor")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CTR == null)
            {
                return NotFound();
            }

            var cTR = await _context.CTR.FindAsync(id);
            if (cTR == null)
            {
                return NotFound();
            }
            ViewData["CacambasId"] = new SelectList(_context.Cacambas, "Id", "Descricao", cTR.CacambasId);
            ViewData["DestinatariosId"] = new SelectList(_context.Destinatarios, "Id", "NomeFantasia", cTR.DestinatariosId);
            ViewData["TransportadoresId"] = new SelectList(_context.Transportadores, "Id", "NomeFantasia", cTR.TransportadoresId);
            return View(cTR);
        }

        // POST: CTRs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CTR cTR)
        {


            if (id != cTR.Id)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                try
                {
                    if (!_context.CTR.Any(m => m.CacambasId == cTR.CacambasId && m.Id!=id && (m.Fechada == false || m.Fechada == null)))
                    {

                        //LOG

                            //CTR_ANTIGO
                            var ctrantigo =  await _context.CTR.FindAsync(id);
                        CTR_antigo logctrantigo = new CTR_antigo();
                        _context.Entry(ctrantigo).State = EntityState.Detached;
                        logctrantigo.Localizacao = ctrantigo.Localizacao;
                            logctrantigo.Latitude = ctrantigo.Latitude;
                            logctrantigo.Longitude = ctrantigo.Longitude;
                            logctrantigo.Data = ctrantigo.Data;
                            logctrantigo.TransportadoresId = ctrantigo.TransportadoresId;
                            logctrantigo.CacambasId = ctrantigo.CacambasId;
                            logctrantigo.DestinatariosId = ctrantigo.DestinatariosId;
                            logctrantigo.UsuarioId = ctrantigo.UsuarioId;
                            logctrantigo.Fechada = ctrantigo.Fechada;
                            logctrantigo.Excluido = ctrantigo.Excluido;
                            logctrantigo.Notificado = ctrantigo.Notificado;
                            logctrantigo.Multado = ctrantigo.Multado;


                        _context.Add(logctrantigo);
                        await _context.SaveChangesAsync();

                        //CTR_NOVO
                        CTR_novo logctrnovo = new CTR_novo();
                        //_context.Entry(logctrnovo).State = EntityState.Detached;
                        logctrnovo.Localizacao = cTR.Localizacao;
                            logctrnovo.Latitude = cTR.Latitude;
                            logctrnovo.Longitude = cTR.Longitude;
                            logctrnovo.Data = cTR.Data;
                            logctrnovo.TransportadoresId = cTR.TransportadoresId;
                            logctrnovo.CacambasId = cTR.CacambasId;
                            logctrnovo.DestinatariosId = cTR.DestinatariosId;
                            logctrnovo.UsuarioId = cTR.UsuarioId;
                            logctrnovo.Fechada = cTR.Fechada;
                            logctrnovo.Excluido = cTR.Excluido;
                            logctrnovo.Notificado = cTR.Notificado;
                            logctrnovo.Multado = cTR.Multado;


                        
                        _context.Add(logctrnovo);
                        await _context.SaveChangesAsync();






                        //await _context.SaveChangesAsync();



                        //Salvar o Log 
                        LogCTR logctr = new LogCTR();
                        _context.Entry(logctr).State = EntityState.Detached;
                        logctr.Data = DateTime.Now;
                            logctr.UsuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                            logctr.CTRId = cTR.Id;
                            logctr.Operacao = "Edit";

                            logctr.CTR_antigoId = logctrantigo.Id;
                            logctr.CTR_novoId = logctrnovo.Id;

                        
                        _context.Add(logctr);


                        



                        //FIM LOG



                        _context.Update(cTR);
                         await _context.SaveChangesAsync();
                    }
                    else
                    {
                        ViewData["alerta"] = "A caçamba já está em uso, ou seja, está em uma CTR aberta.";
                        return View();
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CTRExists(cTR.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                
                ViewData["CacambasId"] = new SelectList(_context.Cacambas, "Id", "Id", cTR.CacambasId);
                ViewData["DestinatariosId"] = new SelectList(_context.Destinatarios, "Id", "Id", cTR.DestinatariosId);
                ViewData["TransportadoresId"] = new SelectList(_context.Transportadores, "Id", "Id", cTR.TransportadoresId);
                return RedirectToAction(nameof(Index));
            }
            //await _context.SaveChangesAsync();
            ViewData["CacambasId"] = new SelectList(_context.Cacambas, "Id", "Id", cTR.CacambasId);
            ViewData["DestinatariosId"] = new SelectList(_context.Destinatarios, "Id", "Id", cTR.DestinatariosId);
            ViewData["TransportadoresId"] = new SelectList(_context.Transportadores, "Id", "Id", cTR.TransportadoresId);
            return View(cTR);
        }


        // GET: CTRs/Edit/5
        [Authorize(Roles = "Adm,Destinatario")]
        
        public async Task<IActionResult> Baixa(int? id)
        {
            if (id == null || _context.CTR == null)
            {
                return NotFound();
            }

            //VAlor do banco 
            var cTR = await _context.CTR.FindAsync(id);

            if (cTR == null)
            {
                return NotFound();
            }
            ViewData["CacambasId"] = new SelectList(_context.Cacambas, "Id", "Descricao", cTR.CacambasId);
            ViewData["DestinatariosId"] = new SelectList(_context.Destinatarios, "Id", "NomeFantasia", cTR.DestinatariosId);
            ViewData["TransportadoresId"] = new SelectList(_context.Transportadores, "Id", "NomeFantasia", cTR.TransportadoresId);

          
            return View(cTR);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Baixa(int id)
        {
            if (id == null || _context.CTR == null)
            {
                return NotFound();
            }

           
            //VAlor do banco 
            var cTR = await _context.CTR.FindAsync(id);
           
            cTR.Fechada = true;
            cTR.DataBaixa = DateTime.Now;

            _context.Update(cTR);
            await _context.SaveChangesAsync();

            if (cTR == null)
            {
                return NotFound();
            }
            ViewData["CacambasId"] = new SelectList(_context.Cacambas, "Id", "Descricao", cTR.CacambasId);
            ViewData["DestinatariosId"] = new SelectList(_context.Destinatarios, "Id", "NomeFantasia", cTR.DestinatariosId);
            ViewData["TransportadoresId"] = new SelectList(_context.Transportadores, "Id", "NomeFantasia", cTR.TransportadoresId);


            //LOG

            //CTR_ANTIGO
            var ctrantigo = await _context.CTR.FindAsync(id);
            CTR_antigo logctrantigo = new CTR_antigo();
            logctrantigo.Localizacao = ctrantigo.Localizacao;
            logctrantigo.Latitude = ctrantigo.Latitude;
            logctrantigo.Longitude = ctrantigo.Longitude;
            logctrantigo.Data = DateTime.Now;
            logctrantigo.TransportadoresId = ctrantigo.TransportadoresId;
            logctrantigo.CacambasId = ctrantigo.CacambasId;
            logctrantigo.DestinatariosId = ctrantigo.DestinatariosId;
            logctrantigo.UsuarioId = ctrantigo.UsuarioId;
            logctrantigo.Fechada = ctrantigo.Fechada;
            logctrantigo.Excluido = ctrantigo.Excluido;
            logctrantigo.Notificado = ctrantigo.Notificado;
            logctrantigo.Multado = ctrantigo.Multado;
//logctrantigo.DataBaixa = DateTime.Now;

            _context.Add(logctrantigo);
            await _context.SaveChangesAsync();

            //CTR_NOVO
            CTR_novo logctrnovo = new CTR_novo();
            logctrnovo.Localizacao = cTR.Localizacao;
            logctrnovo.Latitude = cTR.Latitude;
            logctrnovo.Longitude = cTR.Longitude;
            logctrnovo.Data = cTR.Data;
            logctrnovo.TransportadoresId = cTR.TransportadoresId;
            logctrnovo.CacambasId = cTR.CacambasId;
            logctrnovo.DestinatariosId = cTR.DestinatariosId;
            logctrnovo.UsuarioId = cTR.UsuarioId;
            logctrnovo.Fechada = cTR.Fechada;
            logctrnovo.Excluido = cTR.Excluido;
            logctrnovo.Notificado = cTR.Notificado;
            logctrnovo.Multado = cTR.Multado;
            logctrnovo.DataBaixa = DateTime.Now;

            _context.Add(logctrnovo);
            await _context.SaveChangesAsync();

            //Salvar o Log 
            LogCTR logctr = new LogCTR();
            logctr.Data = DateTime.Now;
            logctr.UsuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            logctr.CTRId = cTR.Id;
            logctr.Operacao = "Edit";

            logctr.CTR_antigoId = logctrantigo.Id;
            logctr.CTR_novoId = logctrnovo.Id;
            _context.Add(logctr);
            await _context.SaveChangesAsync();

            //FIM LOG


            @ViewData["alerta"] = "Baixa efetuada com sucesso.";

           
            await _context.SaveChangesAsync();

            return View(cTR);
        }



        //public async Task<IActionResult> AtualizarCoordenadas()
        //{
         
        //    string latitude = Request.Query["latitude"];
        //    string longitude = Request.Query["longitude"];

        //    if (User.Identity.IsAuthenticated)
        //    {
        //        var querylinq = (from pp in _context.CTR
        //                        where pp.UsuarioId == User.FindFirstValue(ClaimTypes.NameIdentifier) && pp.Fechada == false || pp.Fechada == null
        //                        select pp).OrderByDescending(x=>x.Id).Take(1).AsNoTracking();
        //        if (querylinq != null)
        //        {
        //            CTR cTR = new CTR();

        //            cTR.Id = querylinq.FirstOrDefault().Id;
        //            cTR.Localizacao = querylinq.FirstOrDefault().Localizacao;
        //            cTR.Latitude = latitude;
        //            cTR.Longitude = longitude;
        //            cTR.Data = querylinq.FirstOrDefault().Data;
        //            cTR.TransportadoresId = querylinq.FirstOrDefault().TransportadoresId;
        //            cTR.CacambasId = querylinq.FirstOrDefault().CacambasId;
        //            cTR.DestinatariosId = querylinq.FirstOrDefault().DestinatariosId;
        //            cTR.Fechada = querylinq.FirstOrDefault().Fechada;
        //            cTR.UsuarioId = querylinq.FirstOrDefault().UsuarioId;

        //            //cTR.Id = querylinq.OrderByDescending(x => x.Id).FirstOrDefault().Id;

        //            _context.Update(cTR);
        //            await _context.SaveChangesAsync();
        //            return Redirect(Request.Headers["Referer"].ToString());

        //        }
        //        //if (User.Identity.IsAuthenticated)
        //        //{
        //        //    var userName = User.Identity.Name;
        //        //}
        //    }
        //    return Redirect(Request.Headers["Referer"].ToString());

        //}


        // GET: CTRs/Delete/5
        [Authorize(Roles = "Adm,Gestor")]
        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null || _context.CTR == null)
            {
                return NotFound();
            }

            var cTR = await _context.CTR
                .Include(c => c.Cacambas)
                .Include(c => c.Destinatarios)
                .Include(c => c.Transportadores)
                .FirstOrDefaultAsync(m => m.Id == id);




            if (cTR == null)
            {
                return NotFound();
            }

            return View(cTR);
        }

        // POST: CTRs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CTR == null)
            {
                return Problem("Entity set 'MeuDbContext.CTR'  is null.");
            }
            var cTR = await _context.CTR.FindAsync(id);

            cTR.Excluido = true;

            if (cTR != null)
            {

                //LOG

                //CTR_ANTIGO
                var ctrantigo = await _context.CTR.FindAsync(id);
                CTR_antigo logctrantigo = new CTR_antigo();
                logctrantigo.Localizacao = ctrantigo.Localizacao;
                logctrantigo.Latitude = ctrantigo.Latitude;
                logctrantigo.Longitude = ctrantigo.Longitude;
                logctrantigo.Data = ctrantigo.Data;
                logctrantigo.TransportadoresId = ctrantigo.TransportadoresId;
                logctrantigo.CacambasId = ctrantigo.CacambasId;
                logctrantigo.DestinatariosId = ctrantigo.DestinatariosId;
                logctrantigo.UsuarioId = ctrantigo.UsuarioId;
                logctrantigo.Fechada = ctrantigo.Fechada;
                logctrantigo.Excluido = ctrantigo.Excluido;
                logctrantigo.Notificado = ctrantigo.Notificado;
                logctrantigo.Multado = ctrantigo.Multado;
                _context.Add(logctrantigo);

                //CTR_NOVO
                CTR_antigo logctrnovo = new CTR_antigo();
                logctrnovo.Localizacao = cTR.Localizacao;
                logctrnovo.Latitude = cTR.Latitude;
                logctrnovo.Longitude = cTR.Longitude;
                logctrnovo.Data = cTR.Data;
                logctrnovo.TransportadoresId = cTR.TransportadoresId;
                logctrnovo.CacambasId = cTR.CacambasId;
                logctrnovo.DestinatariosId = cTR.DestinatariosId;
                logctrnovo.UsuarioId = cTR.UsuarioId;
                logctrnovo.Fechada = cTR.Fechada;
                logctrnovo.Excluido = cTR.Excluido;
                logctrnovo.Notificado = cTR.Notificado;
                logctrnovo.Multado = cTR.Multado;
                _context.Add(logctrnovo);

                //Salvar o Log 
                LogCTR logctr = new LogCTR();
                logctr.Data = DateTime.Now;
                logctr.UsuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                logctr.CTRId = cTR.Id;
                logctr.Operacao = "Delete";

                logctr.CTR_antigoId = logctrantigo.Id;
                logctr.CTR_novoId = logctrnovo.Id;
                logctr.Motivo_Exclusao = cTR.Motivo_Exclusao;
                _context.Add(logctr);

                //FIM LOG

                _context.Update(cTR);
              
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CTRExists(int id)
        {
          return _context.CTR.Any(e => e.Id == id);
        }

        //Dropdwon Dinâmico
        [HttpPost]
        public ActionResult GetCacamba(int id)
        {
            var lista = new List<Cacambas>();
            lista = _context.Cacambas.Where(x => x.TransportadoresId == id).ToList();
            return Json(new SelectList(lista, "Id", "Descricao"));
            //return Json(_context.Cidades.Where(c => c.Uf == id), new Newtonsoft.Json.JsonSerializerSettings());
        }


        //PdfSharpCore geração de relatório
        public FileResult GerarRelatorioIncluidosAlterados()
        {
            string busca = Request.Query["busca"];

            if (busca == "undefined")
            {
                busca = "";
            }
            var meuDbContext = _context.CTR.Include(c => c.Cacambas).Include(c => c.Destinatarios).Include(c => c.Transportadores);
            var query = from s in meuDbContext
                        where (EF.Functions.Like(s.Cacambas.Descricao, "%" + busca + "%") || EF.Functions.Like(s.Destinatarios.NomeFantasia, "%" + busca + "%") || EF.Functions.Like(s.Transportadores.NomeFantasia, "%" + busca + "%"))
                        select s;

            using (var doc = new PdfSharpCore.Pdf.PdfDocument())
            {
                var page = doc.AddPage();
                page.Size = PdfSharpCore.PageSize.A4;
                page.Orientation = PdfSharpCore.PageOrientation.Landscape;
                var graphics = PdfSharpCore.Drawing.XGraphics.FromPdfPage(page);
                var corFonte = PdfSharpCore.Drawing.XBrushes.Black;

                var textFormatter = new PdfSharpCore.Drawing.Layout.XTextFormatter(graphics);
                var fonteOrganzacao = new PdfSharpCore.Drawing.XFont("Arial", 10);
                var fonteDescricao = new PdfSharpCore.Drawing.XFont("Arial", 8, PdfSharpCore.Drawing.XFontStyle.BoldItalic);
                var titulodetalhes = new PdfSharpCore.Drawing.XFont("Arial", 14, PdfSharpCore.Drawing.XFontStyle.Bold);
                var fonteDetalhesDescricao = new PdfSharpCore.Drawing.XFont("Arial", 8);

                var caminhoLogo = Path.Combine(_webHostEnv.WebRootPath, @"Imagem\LOGO_PMVV_2021_2.png");
                var logo = caminhoLogo;

                var qtdPaginas = doc.PageCount;

                textFormatter.DrawString(qtdPaginas.ToString(), new PdfSharpCore.Drawing.XFont("Arial", 10), corFonte, new PdfSharpCore.Drawing.XRect(578, 825, page.Width, page.Height));

                // Impressão do LogoTipo
                XImage imagem = XImage.FromFile(logo);

                graphics.DrawImage(imagem, 230, 5, 150, 50);

                // Titulo maior 
                var tituloDetalhes = new PdfSharpCore.Drawing.Layout.XTextFormatter(graphics);
                tituloDetalhes.Alignment = PdfSharpCore.Drawing.Layout.XParagraphAlignment.Center;
                tituloDetalhes.DrawString("Relatório Pessoas (Data Inclusão e Alteração)", titulodetalhes, corFonte, new PdfSharpCore.Drawing.XRect(0, 70, page.Width, page.Height));

                //Número de registros.
                tituloDetalhes.DrawString("Qtd: " + query.Count().ToString(), fonteDetalhesDescricao, corFonte, new PdfSharpCore.Drawing.XRect(200, 70, page.Width, page.Height));


                // titulo das colunas
                var alturaTituloDetalhesY = 140;
                var detalhes = new PdfSharpCore.Drawing.Layout.XTextFormatter(graphics);

                detalhes.DrawString("Localização", fonteDescricao, corFonte, new PdfSharpCore.Drawing.XRect(30, alturaTituloDetalhesY, page.Width, page.Height));
                //detalhes.DrawString("Latitude", fonteDescricao, corFonte, new PdfSharpCore.Drawing.XRect(300, alturaTituloDetalhesY, page.Width, page.Height));
                //detalhes.DrawString("Longitude", fonteDescricao, corFonte, new PdfSharpCore.Drawing.XRect(380, alturaTituloDetalhesY, page.Width, page.Height));
                detalhes.DrawString("Data", fonteDescricao, corFonte, new PdfSharpCore.Drawing.XRect(450, alturaTituloDetalhesY, page.Width, page.Height));
                detalhes.DrawString("Transportadora", fonteDescricao, corFonte, new PdfSharpCore.Drawing.XRect(56, alturaTituloDetalhesY, page.Width, page.Height));
                detalhes.DrawString("Caçamba", fonteDescricao, corFonte, new PdfSharpCore.Drawing.XRect(630, alturaTituloDetalhesY, page.Width, page.Height));
                detalhes.DrawString("Destinatário", fonteDescricao, corFonte, new PdfSharpCore.Drawing.XRect(630, alturaTituloDetalhesY, page.Width, page.Height));

                XFont font = new XFont("Times New Roman", 8, XFontStyle.BoldItalic);

                //dados do relatório (Buscar no banco)
                var alturaDetalhesItens = 160;
                int countLines = 0;

                if (query != null)
                {
                    foreach (var cs in query.OrderByDescending(x => x.TransportadoresId))
                    {
                        textFormatter.DrawString(Convert.ToString(cs.Localizacao), fonteDetalhesDescricao, corFonte, new PdfSharpCore.Drawing.XRect(31, alturaDetalhesItens, page.Width, page.Height));
                        textFormatter.DrawString(Convert.ToString(cs.Data), fonteDetalhesDescricao, corFonte, new PdfSharpCore.Drawing.XRect(301, alturaDetalhesItens, page.Width, page.Height));
                        textFormatter.DrawString(Convert.ToString(cs.Transportadores.NomeFantasia), fonteDetalhesDescricao, corFonte, new PdfSharpCore.Drawing.XRect(381, alturaDetalhesItens, page.Width, page.Height));
                        textFormatter.DrawString(Convert.ToString(cs.Cacambas.Descricao), fonteDetalhesDescricao, corFonte, new PdfSharpCore.Drawing.XRect(451, alturaDetalhesItens, page.Width, page.Height));
                        textFormatter.DrawString(Convert.ToString(cs.Destinatarios.NomeFantasia), fonteDetalhesDescricao, corFonte, new PdfSharpCore.Drawing.XRect(561, alturaDetalhesItens, page.Width, page.Height));
                        
                        //textFormatter.DrawString(cs.DDDTelefone1+cs.Telefone1, fonteDetalhesDescricao, corFonte, new PdfSharpCore.Drawing.XRect(290, alturaDetalhesItens, page.Width, page.Height));
                        //textFormatter.DrawString(cs.DDDTelefone2+cs.Telefone2, fonteDetalhesDescricao, corFonte, new PdfSharpCore.Drawing.XRect(332, alturaDetalhesItens, page.Width, page.Height));
                        //textFormatter.DrawString(cs.Email, fonteDetalhesDescricao, corFonte, new PdfSharpCore.Drawing.XRect(332, alturaDetalhesItens, page.Width, page.Height));
                        //textFormatter.DrawString(DateTime.Now.ToString(), fonteDetalhesDescricao, corFonte, new PdfSharpCore.Drawing.XRect(400, alturaDetalhesItens, page.Width, page.Height));
                        alturaDetalhesItens += 20; //pula linha pq soma 160+20
                        countLines = countLines + 1;

                        if (countLines == 31)
                        {
                            alturaDetalhesItens = 160;
                            graphics.Dispose();

                            page.Size = PdfSharpCore.PageSize.A4;
                            page.Orientation = PdfSharpCore.PageOrientation.Landscape;

                            graphics = PdfSharpCore.Drawing.XGraphics.FromPdfPage(doc.AddPage());
                            corFonte = PdfSharpCore.Drawing.XBrushes.Black;

                            fonteOrganzacao = new PdfSharpCore.Drawing.XFont("Arial", 10);
                            fonteDescricao = new PdfSharpCore.Drawing.XFont("Arial", 8, PdfSharpCore.Drawing.XFontStyle.BoldItalic);
                            titulodetalhes = new PdfSharpCore.Drawing.XFont("Arial", 14, PdfSharpCore.Drawing.XFontStyle.Bold);
                            fonteDetalhesDescricao = new PdfSharpCore.Drawing.XFont("Arial", 8);

                            caminhoLogo = Path.Combine(_webHostEnv.WebRootPath, @"Imagem\LOGO_PMVV_2021_2.png");
                            logo = caminhoLogo;



                            // Impressão do LogoTipo
                            imagem = XImage.FromFile(logo);

                            graphics.DrawImage(imagem, 230, 5, 150, 50);

                            // Titulo maior 
                            tituloDetalhes = new PdfSharpCore.Drawing.Layout.XTextFormatter(graphics);
                            tituloDetalhes.Alignment = PdfSharpCore.Drawing.Layout.XParagraphAlignment.Center;
                            tituloDetalhes.DrawString("Relatório Pessoas (Data Inclusão e Alteração)", titulodetalhes, corFonte, new PdfSharpCore.Drawing.XRect(0, 70, page.Width, page.Height));

                            textFormatter.DrawString(Convert.ToString(cs.Localizacao), fonteDetalhesDescricao, corFonte, new PdfSharpCore.Drawing.XRect(31, alturaDetalhesItens, page.Width, page.Height));
                            textFormatter.DrawString(Convert.ToString(cs.Data), fonteDetalhesDescricao, corFonte, new PdfSharpCore.Drawing.XRect(301, alturaDetalhesItens, page.Width, page.Height));
                            textFormatter.DrawString(Convert.ToString(cs.Transportadores.NomeFantasia), fonteDetalhesDescricao, corFonte, new PdfSharpCore.Drawing.XRect(381, alturaDetalhesItens, page.Width, page.Height));
                            textFormatter.DrawString(Convert.ToString(cs.Cacambas.Descricao), fonteDetalhesDescricao, corFonte, new PdfSharpCore.Drawing.XRect(451, alturaDetalhesItens, page.Width, page.Height));
                            textFormatter.DrawString(Convert.ToString(cs.Destinatarios.NomeFantasia), fonteDetalhesDescricao, corFonte, new PdfSharpCore.Drawing.XRect(561, alturaDetalhesItens, page.Width, page.Height));

                            textFormatter = new PdfSharpCore.Drawing.Layout.XTextFormatter(graphics);

                            qtdPaginas = doc.PageCount;

                            textFormatter.DrawString(qtdPaginas.ToString(), new PdfSharpCore.Drawing.XFont("Arial", 10), corFonte, new PdfSharpCore.Drawing.XRect(578, 825, page.Width, page.Height));


                            alturaDetalhesItens += 20;
                            countLines = 0;
                        }
                    }
                }
                else
                {
                    ViewData["alertarel"] = "Não foram encontrados registros com filtro especificado.";
                }

                using (MemoryStream stream = new MemoryStream())
                {
                    var contantType = "application/pdf";
                    doc.Save(stream, false);

                    var nomeArquivo = "RelatorioPessoa.pdf";

                    return File(stream.ToArray(), contantType, nomeArquivo);
                }


            }

        }

    }
}
