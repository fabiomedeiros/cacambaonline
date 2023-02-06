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
using System.Runtime.ConstrainedExecution;
using cacambaonline.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Options;
using Newtonsoft.Json.Linq;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.Advanced;
using System.Net.Mail;
using System.IO;

namespace cacambaonline.Controllers
{
    public class NotificacoesController : Controller
    {
        private readonly MeuDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public readonly UserManager<IdentityUser> _UserManager;
        public readonly RoleManager<IdentityRole> _RoleManager;
        public readonly IdentityContext _IdentityContext;

        public NotificacoesController(MeuDbContext context, IWebHostEnvironment webHostEnvironment, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IdentityContext identity)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _UserManager = userManager;
            _RoleManager = roleManager;
            _IdentityContext = identity;

        }

        // GET: Notificacoes
        [Authorize(Roles = "Adm,Fiscal,Gestor,Transportador")]
        public async Task<IActionResult> Index()
        {
            var meuDbContext = _context.Notificacoes.Include(n => n.Ctr);

            var meuDbContexttransportador = _context.Notificacoes.Include(n => n.Ctr).Where(x => x.Ctr.UsuarioId == User.FindFirstValue(ClaimTypes.NameIdentifier));

            ViewData["Cacamba"] = new SelectList(_context.Set<Cacambas>(), "Id", "Descricao");

            //var usuarios = _UserManager.Users.ToList();
            var usuarios = _IdentityContext.Users.ToList();

            ViewData["Usuario"] = new SelectList(usuarios, "Id", "Email");

            if (User.IsInRole("Transportador"))
            {
                return View(await meuDbContexttransportador.ToListAsync());
            }

            return View(await meuDbContext.ToListAsync());
        }

        // GET: Notificacoes/Details/5
        [Authorize(Roles = "Adm,Fiscal,Gestor,Transportador")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Notificacoes == null)
            {
                return NotFound();
            }

            var notificacoes = await _context.Notificacoes
                .Include(n => n.Ctr)
                .Include(n=>n.Transportadores)
                .Include(n=>n.Cacambas)
                .Include(n => n.Infracoes)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (notificacoes == null)
            {
                return NotFound();
            }

            return View(notificacoes);
        }


        // GET: Notificacoes/Details/5
        [Authorize(Roles = "Adm,Fiscal,Gestor,Transportador")]
        public async Task<IActionResult> NaoAssinada(int? id)
        {
            if (id == null || _context.Notificacoes == null)
            {
                return NotFound();
            }

            var notificacoes = await _context.Notificacoes
                .Include(n => n.Ctr)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (notificacoes == null)
            {
                return NotFound();
            }

            //arquivoPrefixo
            string queryString = "?ctrid=" + Convert.ToString(notificacoes.CTRId) + "&transportadorid=" + Convert.ToString(notificacoes.TransportadoresId) + "&cacambaid=" + Convert.ToString(notificacoes.CacambasId) + "&busca=" + Convert.ToString(notificacoes.Id) + "&arquivoPrefixo=" + notificacoes.Arquivo;
            return Redirect(Url.Action("GerarRelatorio") + queryString);

           // return View(notificacoes);
        }

        // GET: Notificacoes/Details/5
        [Authorize(Roles = "Adm,Fiscal,Gestor,Transportador")]
        public async Task<IActionResult> Assinada(int id,Notificacoes notificacoes)
       
        {
            //if (id == null || _context.Notificacoes == null)
            //{
            //    return NotFound();
            //}

            var notificacoesbanco = await _context.Notificacoes
                .Include(n => n.Ctr)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (notificacoes == null)
            {
                return NotFound();
            }

            return View(notificacoes);
        }


        //Dropdwon Dinâmico
        [HttpPost]
        public ActionResult GetCacamba(int id)
        {


            var lista = new List<Cacambas>();

            //Adicionando valores padr~eos, isso para solucionar a questão da caçamba sem CTR e caçamba não cadastrada.
            //lista.Add(new Cacambas() { Id = -1, Descricao = "--Selecione--" });
            //lista.Add(new Cacambas() { Id = -2, Descricao = "Caçamba sem CTR" });
            //lista.Add(new Cacambas() { Id = -3, Descricao = "Caçamba não cadastrada" });

            lista = _context.Cacambas.Where(x => x.TransportadoresId == id).ToList();
            //lista.Add(new Cacambas() { Id = -2, Descricao = "Caçamba sem CTR" });
            //lista.Add(new Cacambas() { Id = -3, Descricao = "Caçamba não cadastrada" });


            return Json(new SelectList(lista, "Id", "Descricao"));
            //return Json(_context.Cidades.Where(c => c.Uf == id), new Newtonsoft.Json.JsonSerializerSettings());
        }


        // GET: Notificacoes/Create
        [Authorize(Roles = "Adm,Fiscal,Gestor")]
        public IActionResult Create(int? id)
        {
            var notificar = from ctr in _context.CTR
                            join cacamba in _context.Cacambas on ctr.CacambasId equals cacamba.Id
                            select new { ctr.Id, ctr.Fechada, ctr.Notificado, cacamba.Descricao };

            ViewData["CTRId"] = new SelectList(notificar.Where(x => (x.Fechada == false || x.Fechada == null) && x.Notificado == false), "Id", "Descricao");

            //Tratar Notificação sem CTR
            ViewData["NCtr"] = "Não";

            if (id != null)
            {
                ViewData["NCtr"] = "Sim";
                ViewData["CTRId"] = new SelectList(notificar.Where(x => x.Id == id), "Id", "Descricao");
            }

            ViewData["InfracoesId"] = new SelectList(_context.Infracoes, "Id", "Descricao");

            ViewData["TrasnportadoraId"] = new SelectList(_context.Transportadores, "Id", "NomeFantasia");
            ViewData["CacambaId"] = new SelectList(_context.Transportadores, "Id", "Descricao");


            //if (!notificar.Any(x => (x.Fechada == false || x.Fechada == null) && x.Notificado == false))
            //{
            //    ViewData["alerta"] = "Não há CTR para notificar.";
            //}
            //else
            //{
            //    ViewData["alerta"] = "";
            //}

            return View();
        }

        // POST: Notificacoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Adm,Fiscal,Gestor")]
        public async Task<IActionResult> Create(Notificacoes notificacoes)
        {

            var notificar = from ctr in _context.CTR
                            join cacamba in _context.Cacambas on ctr.CacambasId equals cacamba.Id
                            select new { ctr.Id, ctr.Fechada, ctr.Notificado, cacamba.Descricao };

            if (ModelState.IsValid)
            {
                

                string x = User.FindFirstValue(ClaimTypes.NameIdentifier);

                notificacoes.UsuarioFiscalId = x;
                notificacoes.Data = DateTime.Now;
                if (string.IsNullOrWhiteSpace(x))
                {
                    ViewData["Longitude"] = "A operação não foi concluída, pois sua sessão inspirou. É necessário entrar novamente no sistema.";
                }
                else
                {
                    //Nova notificação
                    notificacoes.Id = 0;

                    if (notificacoes.CacambasId == -2 || notificacoes.CacambasId == -1 || notificacoes.CacambasId == -3)
                    {
                        notificacoes.CacambasId = null;
                    }
                    if (notificacoes.TransportadoresId == -1)
                    {
                        notificacoes.TransportadoresId = null;
                    }
                    if (notificacoes.CTRId == -1)
                    {
                        notificacoes.CTRId = null;
                    }

                    var arquivoPrefixo = Guid.NewGuid() + "_"; //para não repitir o nome
                    notificacoes.Foto1 = arquivoPrefixo + notificacoes.UploadFoto1.FileName;
                    notificacoes.Foto2 = arquivoPrefixo + notificacoes.UploadFoto2.FileName;

                    //var nomeArquivo = "arquivos/notificacoes/" + busca + "/" + arquivoPrefixo + "Notificacao.pdf";

                    notificacoes.Arquivo = arquivoPrefixo + "Notificacao.pdf";

                    _context.Add(notificacoes);
                    await _context.SaveChangesAsync();

                    //Primeiro adiconar os documentos dos uploads
                    string webRootPath = _webHostEnvironment.WebRootPath;
                    string contentRootPath = _webHostEnvironment.ContentRootPath;
                    string path = "";
                    path = Path.Combine(webRootPath, "arquivos/notificacoes/" + notificacoes.Id);
                  
                    if (!await UploadArquivo(notificacoes.UploadFoto1, arquivoPrefixo, path))
                    {
                        return View(notificacoes);
                    }
                   

                    if (!await UploadArquivo(notificacoes.UploadFoto2, arquivoPrefixo, path))
                    {
                        return View(notificacoes);
                    }
                    
                    //Caso de transformar Notificação em CTR
                    CTR ctrn = new CTR();
                    ctrn.Localizacao = notificacoes.Localizacao;
                    ctrn.Latitude = notificacoes.Latitude;
                    ctrn.Longitude = notificacoes.Longitude;
                    ctrn.Data = DateTime.Now;
                    ctrn.TransportadoresId = notificacoes.TransportadoresId;
                    //ctrn.CacambasId = notificacoes;
                    ctrn.DestinatariosId = null;
                    ctrn.Fechada = null;
                    ctrn.UsuarioId = notificacoes.UsuarioFiscalId;
                    ctrn.Excluido = null;
                    ctrn.Multado = null;
                    ctrn.Notificado = null;
                    ctrn.DataBaixa = null;

                    if (notificacoes.InfracoesId == 15)
                    {
                        ctrn.VeiodaNotificacao = "Caçamba sem CTR";
                        _context.Add(ctrn);
                        await _context.SaveChangesAsync();
                    }
                    else if (notificacoes.InfracoesId == 4)
                    {
                        //Caçamba não cadastrada
                        ctrn.VeiodaNotificacao = "Caçamba não cadastrada";
                        _context.Add(ctrn);
                        await _context.SaveChangesAsync();

                        notificacoes.CacambasId = null;

                    }
                    //Fim 

                    //Notificacoes_novo
                    Notificacoes_Novo notificacao_novo = new Notificacoes_Novo();
                    notificacao_novo.Descricao = notificacoes.Descricao;
                    notificacao_novo.Obs = notificacoes.Obs;
                    notificacao_novo.Localizacao = notificacoes.Localizacao;
                    notificacao_novo.Latitude = notificacoes.Latitude;
                    notificacao_novo.Longitude = notificacoes.Latitude;
                    notificacao_novo.Foto1 = notificacoes.Foto1;
                    notificacao_novo.Foto2 = notificacoes.Foto2;
                    notificacao_novo.Multa = notificacoes.Multa;
                    notificacao_novo.CTRId = notificacoes.CTRId;
                    notificacao_novo.InfracoesId = notificacoes.InfracoesId;
                    notificacao_novo.UsuarioFiscalId = notificacoes.UsuarioFiscalId;
                    _context.Add(notificacao_novo);
                    await _context.SaveChangesAsync();

                    //Salvar o Log 
                    LogNotificacao lognotificacao = new LogNotificacao();
                    lognotificacao.Data = DateTime.Now;
                    lognotificacao.Operacao = "Create";
                    lognotificacao.UsuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    lognotificacao.NotificacoesId = notificacoes.Id;
                    _context.Add(lognotificacao);
                    await _context.SaveChangesAsync();

                    //Alterar o campo notificado da tabela CTR.
                    if (notificacoes.CacambasId != null)
                    {
                        //Só entra quando a infração não for (caçamba sem ctr ou caçamba não cadastrada).
                        if (notificacoes.InfracoesId != 15 && notificacoes.InfracoesId != 4)
                        {
                            var ctr = await _context.CTR.FindAsync(notificacoes.CTRId);
                            ctr.Notificado = true;
                            _context.Update(ctr);
                            await _context.SaveChangesAsync();
                        }
                    }

                    ViewData["CTRId"] = new SelectList(notificar.Where(x => (x.Fechada == false || x.Fechada == null) && x.Notificado == false), "Id", "Descricao");
                    ViewData["InfracoesId"] = new SelectList(_context.Infracoes, "Id", "Descricao");


                    //Mandando e-mail para transportadora a saber 

                    //arquivoPrefixo
                    string queryString = "?ctrid="+Convert.ToString(notificacoes.CTRId)+"&transportadorid="+Convert.ToString(notificacoes.TransportadoresId)+"&cacambaid="+Convert.ToString(notificacoes.CacambasId) +"&busca="+Convert.ToString(notificacoes.Id) + "&arquivoPrefixo=" + notificacoes.Arquivo;
                    return Redirect(Url.Action("GerarRelatorio") + queryString);
                }

            }

            ViewData["CTRId"] = new SelectList(notificar.Where(x => (x.Fechada == false || x.Fechada == null) && x.Notificado == false), "Id", "Descricao");
            ViewData["InfracoesId"] = new SelectList(_context.Infracoes, "Id", "Descricao");

            return View(notificacoes);
        }

        public void Enviar(string destinatario, string anexo)
        {
            //Define os dados do e-mail
            string nomeRemetente = "Secretaria Municipal de Meio Ambiente";

            string emailRemetente = "nao-responda@vilavelha.es.gov.br";
            string senha = "V7tklf4xy7maragLBYRU";
           
            //Host da porta SMTP
            string SMTP = "correio.vilavelha.es.gov.br";

            string emailDestinatario = destinatario;
            //string emailComCopia        = "email@comcopia.com.br";
            //string emailComCopiaOculta  = "email@comcopiaoculta.com.br";

            string assuntoMensagem = "SECRETARIA MUNICIPAL DE MEIO AMBIENTE  - CAÇAMBA ONLINE - NOTIFICAÇÃO.";

            //Gerar número randômico
            Random rand = new Random();
            int numeroaleatorio = rand.Next(10800, 50800);
            string stringaleatoria = destinatario.Substring(0, 5) + Convert.ToString(numeroaleatorio);

            string conteudoMensagem = "Prezado(a) você recebeu uma notificação. O arquivo da nofitiaçãoe está anexo." ; //< a href = "http://www.vilavelha.es.gov.br/" target = "_blank"> </a>;

            //Cria objeto com dados do e-mail.
            MailMessage objEmail = new MailMessage();

            //Define o Campo From e ReplyTo do e-mail.
            objEmail.From = new System.Net.Mail.MailAddress(nomeRemetente + "<" + emailRemetente + ">");

            //Define os destinatários do e-mail.
            objEmail.To.Add(emailDestinatario);

            //Define a prioridade do e-mail.
            objEmail.Priority = System.Net.Mail.MailPriority.Normal;

            //Define o formato do e-mail HTML (caso não queira HTML alocar valor false)
            objEmail.IsBodyHtml = true;

            //Define título do e-mail.
            objEmail.Subject = assuntoMensagem;

            //Define o corpo do e-mail.
            objEmail.Body = conteudoMensagem;

            //Anexo
            if (!String.IsNullOrEmpty(anexo)) //anexo é o nome do arquivo
            {
                Attachment anexar = new Attachment(anexo);
                objEmail.Attachments.Add(anexar);
            }

            //Para evitar problemas de caracteres "estranhos", configuramos o charset para "ISO-8859-1"
            objEmail.SubjectEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");
            objEmail.BodyEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");

            //Cria objeto com os dados do SMTP
            System.Net.Mail.SmtpClient objSmtp = new System.Net.Mail.SmtpClient();

            //Alocamos o endereço do host para enviar os e-mails  
            objSmtp.Credentials = new System.Net.NetworkCredential(emailRemetente, senha);
            objSmtp.Host = SMTP;
            objSmtp.Port = 25;
            //Caso utilize conta de email do exchange da locaweb deve habilitar o SSL
            //objEmail.EnableSsl = true;

            //Enviamos o e-mail através do método .send()
            try
            {
                objSmtp.Send(objEmail);

                Response.Redirect("SucessoEnvio.aspx");
            }
            catch (Exception ex)
            {
                // Response.Write("Ocorreram problemas no envio do e-mail. Erro = " + ex.Message);
            }
            finally
            {
                //excluímos o objeto de e-mail da memória
                objEmail.Dispose();
                //anexo.Dispose();
            }

        }


        private async Task<bool> UploadArquivo(IFormFile arquivo, string arquivoPrefixo, string path)
        {
            if (arquivo.Length <= 0) return false;

            CriarPasta(path);

            var pathcompleto = Path.Combine(Directory.GetCurrentDirectory(), path, arquivoPrefixo + arquivo.FileName);

            if (System.IO.File.Exists(pathcompleto))
            {
                ModelState.AddModelError(string.Empty, "Jà existe um arquivo com este nome!");
                return false;
            }
            using (var stream = new FileStream(pathcompleto, FileMode.Create))
            {
                await arquivo.CopyToAsync(stream); //gravação 
            }
            return true;
        }

        //Verificar se existe o path passado, senão tiver cria.
        private void CriarPasta(string path)
        {

            bool folderExists = Directory.Exists(path);
            if (!folderExists)
                Directory.CreateDirectory(path);
        }


        // GET: Notificacoes/Edit/5
        [Authorize(Roles = "Adm,Gestor")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Notificacoes == null)
            {
                return NotFound();
            }

            var notificacoes = await _context.Notificacoes.FindAsync(id);
            if (notificacoes == null)
            {
                return NotFound();
            }
            ViewData["CTRId"] = new SelectList(_context.CTR, "Id", "Id", notificacoes.CTRId);
            return View(notificacoes);
        }

        // POST: Notificacoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Adm,Gestor")]
        public async Task<IActionResult> Edit(int id, Notificacoes notificacoes)
        {
            if (id != notificacoes.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(notificacoes);
                    await _context.SaveChangesAsync();

                    //Notificacoes_antigo
                    var notificacaobanco = await _context.Notificacoes.FindAsync(id);
                    Notificacoes_Novo notificacao_antigo = new Notificacoes_Novo();
                    notificacao_antigo.Descricao = notificacaobanco.Descricao;
                    notificacao_antigo.Obs = notificacaobanco.Obs;
                    notificacao_antigo.Localizacao = notificacaobanco.Localizacao;
                    notificacao_antigo.Latitude = notificacaobanco.Latitude;
                    notificacao_antigo.Longitude = notificacaobanco.Latitude;
                    notificacao_antigo.Foto1 = notificacaobanco.Foto1;
                    notificacao_antigo.Foto2 = notificacaobanco.Foto2;
                    notificacao_antigo.Multa = notificacaobanco.Multa;
                    notificacao_antigo.CTRId = notificacaobanco.CTRId;
                    notificacao_antigo.InfracoesId = notificacaobanco.InfracoesId;
                    notificacao_antigo.UsuarioFiscalId = notificacaobanco.UsuarioFiscalId;
                    _context.Add(notificacao_antigo);
                    await _context.SaveChangesAsync();

                    //Notificacoes_novo
                    Notificacoes_Novo notificacao_novo = new Notificacoes_Novo();
                    notificacao_novo.Descricao = notificacoes.Descricao;
                    notificacao_novo.Obs = notificacoes.Obs;
                    notificacao_novo.Localizacao = notificacoes.Localizacao;
                    notificacao_novo.Latitude = notificacoes.Latitude;
                    notificacao_novo.Longitude = notificacoes.Latitude;
                    notificacao_novo.Foto1 = notificacoes.Foto1;
                    notificacao_novo.Foto2 = notificacoes.Foto2;
                    notificacao_novo.Multa = notificacoes.Multa;
                    notificacao_novo.CTRId = notificacoes.CTRId;
                    notificacao_novo.InfracoesId = notificacoes.InfracoesId;
                    notificacao_novo.UsuarioFiscalId = notificacoes.UsuarioFiscalId;
                    _context.Add(notificacao_novo);
                    await _context.SaveChangesAsync();

                    //Salvar o Log 
                    LogNotificacao lognotificacao = new LogNotificacao();
                    lognotificacao.Data = DateTime.Now;
                    lognotificacao.Operacao = "Create";
                    lognotificacao.UsuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    lognotificacao.NotificacoesId = notificacoes.Id;
                    lognotificacao.Notificacoes_AntigoId = notificacao_antigo.Id;
                    lognotificacao.Notificacoes_NovoId = notificacao_novo.Id;
                    _context.Add(lognotificacao);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NotificacoesExists(notificacoes.Id))
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
            ViewData["CTRId"] = new SelectList(_context.CTR, "Id", "Id", notificacoes.CTRId);
            return View(notificacoes);
        }

        // GET: Notificacoes/Delete/5
        [Authorize(Roles = "Adm,Gestor")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Notificacoes == null)
            {
                return NotFound();
            }

            var notificacoes = await _context.Notificacoes
                .Include(n => n.Ctr)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (notificacoes == null)
            {
                return NotFound();
            }

            return View(notificacoes);
        }

        [Authorize(Roles = "Adm,Gestor")]
        // POST: Notificacoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Notificacoes == null)
            {
                return Problem("Entity set 'MeuDbContext.Notificacoes'  is null.");
            }
            var notificacoes = await _context.Notificacoes.FindAsync(id);
            if (notificacoes != null)
            {
                notificacoes.Excluido = true;
                _context.Notificacoes.Update(notificacoes);

                //Notificacoes_antigo
                var notificacaobanco = await _context.Notificacoes.FindAsync(id);
                Notificacoes_Novo notificacao_antigo = new Notificacoes_Novo();
                notificacao_antigo.Descricao = notificacaobanco.Descricao;
                notificacao_antigo.Obs = notificacaobanco.Obs;
                notificacao_antigo.Localizacao = notificacaobanco.Localizacao;
                notificacao_antigo.Latitude = notificacaobanco.Latitude;
                notificacao_antigo.Longitude = notificacaobanco.Latitude;
                notificacao_antigo.Foto1 = notificacaobanco.Foto1;
                notificacao_antigo.Foto2 = notificacaobanco.Foto2;
                notificacao_antigo.Multa = notificacaobanco.Multa;
                notificacao_antigo.CTRId = notificacaobanco.CTRId;
                notificacao_antigo.InfracoesId = notificacaobanco.InfracoesId;
                notificacao_antigo.UsuarioFiscalId = notificacaobanco.UsuarioFiscalId;
                _context.Add(notificacao_antigo);
                await _context.SaveChangesAsync();

                //Notificacoes_novo
                Notificacoes_Novo notificacao_novo = new Notificacoes_Novo();
                notificacao_novo.Descricao = notificacoes.Descricao;
                notificacao_novo.Obs = notificacoes.Obs;
                notificacao_novo.Localizacao = notificacoes.Localizacao;
                notificacao_novo.Latitude = notificacoes.Latitude;
                notificacao_novo.Longitude = notificacoes.Latitude;
                notificacao_novo.Foto1 = notificacoes.Foto1;
                notificacao_novo.Foto2 = notificacoes.Foto2;
                notificacao_novo.Multa = notificacoes.Multa;
                notificacao_novo.CTRId = notificacoes.CTRId;
                notificacao_novo.InfracoesId = notificacoes.InfracoesId;
                notificacao_novo.UsuarioFiscalId = notificacoes.UsuarioFiscalId;
                _context.Add(notificacao_novo);
                await _context.SaveChangesAsync();

                //Salvar o Log 
                LogNotificacao lognotificacao = new LogNotificacao();
                lognotificacao.Data = DateTime.Now;
                lognotificacao.Operacao = "Create";
                lognotificacao.UsuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                lognotificacao.NotificacoesId = notificacoes.Id;
                lognotificacao.Notificacoes_AntigoId = notificacao_antigo.Id;
                lognotificacao.Notificacoes_NovoId = notificacao_novo.Id;
                _context.Add(lognotificacao);
                await _context.SaveChangesAsync();

            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NotificacoesExists(int id)
        {
            return _context.Notificacoes.Any(e => e.Id == id);
        }

        //PdfSharpCore geração de relatório
        public FileResult GerarRelatorio()
        {
            string? busca = Request.Query["busca"];
            string? ctrid = Request.Query["ctrid"];
            string? transportadorid = Request.Query["transportadorid"];
            string? cacambaid = Request.Query["cacambaid"];
            string? arquivoPrefixo = Request.Query["arquivoPrefixo"];
            string? nmarquivo ="";
            string? email = "";

            if (busca == "undefined")
            {
                busca = null;
            }
            if (ctrid == "undefined")
            {
                ctrid = null;
            }
            if (transportadorid == "undefined")
            {
                transportadorid = null;
            }
            if (cacambaid == "undefined")
            {
                cacambaid = null;
            }

            var queryresumido = (from n in _context.Notificacoes
                               join ctr in _context.CTR on n.CTRId equals ctr.Id
                               join t in _context.Transportadores on n.TransportadoresId equals t.Id
                               join i in _context.Infracoes on n.InfracoesId equals i.Id
                               select new
                               {
                                   Notificacao = n.Id,
                                   n.CTRId,
                                   n.UsuarioFiscalId,
                                   t.Cnpj,
                                   t.NomeEmpresarial,
                                   t.NomeFantasia,
                                   t.EnderecosTransportador.Logradouro,
                                   t.EnderecosTransportador.Numero,
                                   t.EnderecosTransportador.Bairro,
                                   t.EnderecosTransportador.Cidade,
                                   t.Email,

                                   n.Localizacao,
                                   n.Data,
                                   IdentificacaoCacamba = ctr.Cacambas.Numero,
                                   TipoInfracao = i.Descricao,
                                   n.Obs,

                                   n.Foto1,
                                   n.Foto2
                               }).ToList();

            if (!string.IsNullOrWhiteSpace(ctrid))
            {

                queryresumido = (from n in _context.Notificacoes
                               join ctr in _context.CTR on n.CTRId equals ctr.Id
                               join t in _context.Transportadores on n.TransportadoresId equals t.Id
                               join i in _context.Infracoes on n.InfracoesId equals i.Id
                               select new
                               {
                                   Notificacao = n.Id,
                                   n.CTRId,
                                   n.UsuarioFiscalId,
                                   t.Cnpj,
                                   t.NomeEmpresarial,
                                   t.NomeFantasia,
                                   t.EnderecosTransportador.Logradouro,
                                   t.EnderecosTransportador.Numero,
                                   t.EnderecosTransportador.Bairro,
                                   t.EnderecosTransportador.Cidade,
                                   t.Email,

                                   n.Localizacao,
                                   n.Data,
                                   IdentificacaoCacamba = ctr.Cacambas.Numero,
                                   TipoInfracao = i.Descricao,
                                   n.Obs,

                                   n.Foto1,
                                   n.Foto2
                               }).ToList();

            }
            if (string.IsNullOrWhiteSpace(ctrid) && !string.IsNullOrWhiteSpace(transportadorid) && !string.IsNullOrWhiteSpace(cacambaid) )
            {
                queryresumido = (from n in _context.Notificacoes
                               join t in _context.Transportadores on n.TransportadoresId equals t.Id
                               join c in _context.Cacambas on n.CacambasId equals c.Id
                               join i in _context.Infracoes on n.InfracoesId equals i.Id
                               select new
                               {
                                   Notificacao = n.Id,
                                   n.CTRId,
                                   n.UsuarioFiscalId,
                                   t.Cnpj,
                                   t.NomeEmpresarial,
                                   t.NomeFantasia,
                                   t.EnderecosTransportador.Logradouro,
                                   t.EnderecosTransportador.Numero,
                                   t.EnderecosTransportador.Bairro,
                                   t.EnderecosTransportador.Cidade,
                                   t.Email,

                                   n.Localizacao,
                                   n.Data,
                                   IdentificacaoCacamba = c.Numero,
                                   TipoInfracao = i.Descricao,
                                   n.Obs,

                                   n.Foto1,
                                   n.Foto2
                               }).ToList();
            }
            if (string.IsNullOrWhiteSpace(ctrid)  && !string.IsNullOrWhiteSpace(transportadorid) && string.IsNullOrWhiteSpace(cacambaid))
            {
                queryresumido = (from n in _context.Notificacoes
                               join t in _context.Transportadores on n.TransportadoresId equals t.Id
                               join i in _context.Infracoes on n.InfracoesId equals i.Id
                               select new
                               {
                                   Notificacao = n.Id,
                                   n.CTRId,
                                   n.UsuarioFiscalId,
                                   t.Cnpj,
                                   t.NomeEmpresarial,
                                   t.NomeFantasia,
                                   t.EnderecosTransportador.Logradouro,
                                   t.EnderecosTransportador.Numero,
                                   t.EnderecosTransportador.Bairro,
                                   t.EnderecosTransportador.Cidade,
                                   t.Email,

                                   n.Localizacao,
                                   n.Data,
                                   IdentificacaoCacamba = n.Descricao,
                                   TipoInfracao = i.Descricao,
                                   n.Obs,

                                   n.Foto1,
                                   n.Foto2
                               }).ToList();
            }

            //Jogada para capturar o nome da tabela pessoa que a identificação seja igual a chave da AspNetUsers
            string UsuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var pessoas = _context.Pessoas.ToList().Where(x=>x.AspNetUsersId == UsuarioId);


            if (!string.IsNullOrWhiteSpace(busca))
            {
                queryresumido = queryresumido.Where(n => n.Notificacao == Convert.ToInt32(busca)).ToList();
            }

            using (var doc = new PdfSharpCore.Pdf.PdfDocument())
            {
                var page = doc.AddPage();
                page.Size = PdfSharpCore.PageSize.A4;
                page.Orientation = PdfSharpCore.PageOrientation.Portrait;
                var graphics = PdfSharpCore.Drawing.XGraphics.FromPdfPage(page);
                var corFonte = PdfSharpCore.Drawing.XBrushes.Black;

                var textFormatter = new PdfSharpCore.Drawing.Layout.XTextFormatter(graphics);
                var fonteOrganzacao = new PdfSharpCore.Drawing.XFont("Arial", 10);
                var fonteDescricao = new PdfSharpCore.Drawing.XFont("Arial", 8, PdfSharpCore.Drawing.XFontStyle.BoldItalic);
                var titulodetalhes = new PdfSharpCore.Drawing.XFont("Arial", 14, PdfSharpCore.Drawing.XFontStyle.Bold);

                var subtitulodetalhes = new PdfSharpCore.Drawing.XFont("Arial", 12, PdfSharpCore.Drawing.XFontStyle.Bold);

                var fonteDetalhesDescricaoTitulo = new PdfSharpCore.Drawing.XFont("Arial", 8, PdfSharpCore.Drawing.XFontStyle.Bold);

                var fonteDetalhesDescricao = new PdfSharpCore.Drawing.XFont("Arial", 8);


                XPen lineRed = new XPen(XColors.Black, 5);

                var qtdPaginas = doc.PageCount;

                textFormatter.DrawString(qtdPaginas.ToString(), new PdfSharpCore.Drawing.XFont("Arial", 10), corFonte, new PdfSharpCore.Drawing.XRect(578, 825, page.Width, page.Height));

                //Número de registros.
                //tituloDetalhes.DrawString("Qtd: " + querydetalhado.Count().ToString(), fonteDetalhesDescricao, corFonte, new PdfSharpCore.Drawing.XRect(200, 70, page.Width, page.Height));


                // titulo das colunas
                var alturaDetalhesItens = 140;
                var detalhes = new PdfSharpCore.Drawing.Layout.XTextFormatter(graphics);

                //Foi feito só para imprimir 1 vez.
                int auxx = 0;
                foreach (var cs in queryresumido.OrderByDescending(x => x.Notificacao).ToList())
                {
                    auxx++;
                    if (auxx == 1)
                    {
                        var caminhoLogo = Path.Combine(_webHostEnvironment.WebRootPath, @"imagens\LOGO_PMVV_2021_2.png");
                        var logo = caminhoLogo;
                        // Impressão do LogoTipo
                        XImage imagem = XImage.FromFile(logo);
                        graphics.DrawImage(imagem, 31, 20, 150, 50);

                        // Titulo maior 
                        var tituloDetalhes = new PdfSharpCore.Drawing.Layout.XTextFormatter(graphics);
                        tituloDetalhes.Alignment = PdfSharpCore.Drawing.Layout.XParagraphAlignment.Center;
                        tituloDetalhes.DrawString("Notificação ", titulodetalhes, corFonte, new PdfSharpCore.Drawing.XRect(0, 40, page.Width, page.Height));
                        //textFormatter.DrawString(Convert.ToString(cs.Notificacao), fonteDetalhesDescricao, corFonte, new PdfSharpCore.Drawing.XRect(31, alturaDetalhesItens + 95, page.Width, page.Height));
                        var tituloDetalhesconteudo = new PdfSharpCore.Drawing.Layout.XTextFormatter(graphics);
                        tituloDetalhesconteudo.Alignment = PdfSharpCore.Drawing.Layout.XParagraphAlignment.Center;
                        tituloDetalhesconteudo.DrawString(Convert.ToString(cs.Notificacao), titulodetalhes, corFonte, new PdfSharpCore.Drawing.XRect(0, 55, page.Width, page.Height));


                        textFormatter.DrawString("Transportadora", subtitulodetalhes, corFonte, new PdfSharpCore.Drawing.XRect(31, alturaDetalhesItens, page.Width, page.Height));
                        textFormatter.DrawString("_______________________________________________________________________________________________________________", fonteDetalhesDescricaoTitulo, corFonte, new PdfSharpCore.Drawing.XRect(31, alturaDetalhesItens+7, page.Width, page.Height));
                        //graphics.DrawLines(lineRed, 0, page.Height, page.Width, page.Height);

                        textFormatter.DrawString("CNPJ", fonteDetalhesDescricaoTitulo, corFonte, new PdfSharpCore.Drawing.XRect(31, alturaDetalhesItens+30, page.Width, page.Height));
                        if (!String.IsNullOrWhiteSpace(Convert.ToString(cs.Cnpj)))
                        {
                            textFormatter.DrawString(Convert.ToString(cs.Cnpj), fonteDetalhesDescricao, corFonte, new PdfSharpCore.Drawing.XRect(31, alturaDetalhesItens+40, page.Width, page.Height));
                        }

                        textFormatter.DrawString("Nome/Razão Social", fonteDetalhesDescricaoTitulo, corFonte, new PdfSharpCore.Drawing.XRect(31, alturaDetalhesItens+55, page.Width, page.Height));
                        if (!String.IsNullOrWhiteSpace(Convert.ToString(cs.NomeEmpresarial)))
                        {
                            textFormatter.DrawString(Convert.ToString(cs.NomeEmpresarial), fonteDetalhesDescricao, corFonte, new PdfSharpCore.Drawing.XRect(31, alturaDetalhesItens+65, page.Width, page.Height));
                        }

                        textFormatter.DrawString("Nome/Fantasia", fonteDetalhesDescricaoTitulo, corFonte, new PdfSharpCore.Drawing.XRect(31, alturaDetalhesItens + 80, page.Width, page.Height));
                        if (!String.IsNullOrWhiteSpace(Convert.ToString(cs.NomeFantasia)))
                        {
                            textFormatter.DrawString(Convert.ToString(cs.NomeFantasia), fonteDetalhesDescricao, corFonte, new PdfSharpCore.Drawing.XRect(31, alturaDetalhesItens + 90, page.Width, page.Height));
                            nmarquivo = cs.NomeFantasia;
                        }

                        textFormatter.DrawString("Endereço", fonteDetalhesDescricaoTitulo, corFonte, new PdfSharpCore.Drawing.XRect(31, alturaDetalhesItens + 105, page.Width, page.Height));
                        if (!String.IsNullOrWhiteSpace(Convert.ToString(cs.Logradouro)))
                        {
                            textFormatter.DrawString(Convert.ToString(cs.Logradouro), fonteDetalhesDescricao, corFonte, new PdfSharpCore.Drawing.XRect(31, alturaDetalhesItens + 115, page.Width, page.Height));
                        }

                        //Segunda Coluna

                        textFormatter.DrawString("Nº:", fonteDetalhesDescricaoTitulo, corFonte, new PdfSharpCore.Drawing.XRect(300, alturaDetalhesItens + 30, page.Width, page.Height));
                        if (!String.IsNullOrWhiteSpace(Convert.ToString(cs.Numero)))
                        {
                            textFormatter.DrawString(Convert.ToString(cs.Numero), fonteDetalhesDescricao, corFonte, new PdfSharpCore.Drawing.XRect(300, alturaDetalhesItens + 40, page.Width, page.Height));
                        }

                        textFormatter.DrawString("Bairro:", fonteDetalhesDescricaoTitulo, corFonte, new PdfSharpCore.Drawing.XRect(300, alturaDetalhesItens + 55, page.Width, page.Height));
                        if (!String.IsNullOrWhiteSpace(Convert.ToString(cs.Bairro)))
                        {
                            textFormatter.DrawString(Convert.ToString(cs.Bairro), fonteDetalhesDescricao, corFonte, new PdfSharpCore.Drawing.XRect(300, alturaDetalhesItens + 65, page.Width, page.Height));
                        }

                        textFormatter.DrawString("Cidade:", fonteDetalhesDescricaoTitulo, corFonte, new PdfSharpCore.Drawing.XRect(300, alturaDetalhesItens + 80, page.Width, page.Height));
                        if (!String.IsNullOrWhiteSpace(Convert.ToString(cs.Cidade)))
                        {
                            textFormatter.DrawString(Convert.ToString(cs.Cidade), fonteDetalhesDescricao, corFonte, new PdfSharpCore.Drawing.XRect(300, alturaDetalhesItens + 90, page.Width, page.Height));
                        }

                        if (!String.IsNullOrWhiteSpace(Convert.ToString(cs.Email)))
                        {
                            email = cs.Email;
                        }

                        textFormatter.DrawString("Fiscalização", subtitulodetalhes, corFonte, new PdfSharpCore.Drawing.XRect(31, alturaDetalhesItens+ 150, page.Width, page.Height));
                        textFormatter.DrawString("_______________________________________________________________________________________________________________", fonteDetalhesDescricaoTitulo, corFonte, new PdfSharpCore.Drawing.XRect(31, alturaDetalhesItens + 157, page.Width, page.Height));

                        textFormatter.DrawString("Tipo de Infração:", fonteDetalhesDescricaoTitulo, corFonte, new PdfSharpCore.Drawing.XRect(31, alturaDetalhesItens + 180, page.Width, page.Height));
                        if (!String.IsNullOrWhiteSpace(Convert.ToString(cs.TipoInfracao)))
                        {
                            textFormatter.DrawString(Convert.ToString(cs.TipoInfracao), fonteDetalhesDescricao, corFonte, new PdfSharpCore.Drawing.XRect(31, alturaDetalhesItens + 190, page.Width, page.Height));
                        }
                       
                        textFormatter.DrawString("Fiscal:", fonteDetalhesDescricaoTitulo, corFonte, new PdfSharpCore.Drawing.XRect(31, alturaDetalhesItens + 205, page.Width, page.Height));
                        if (pessoas.Count() > 0)
                        {
                            textFormatter.DrawString(pessoas.FirstOrDefault().Nome, fonteDetalhesDescricao, corFonte, new PdfSharpCore.Drawing.XRect(31, alturaDetalhesItens + 215, page.Width, page.Height));
                            
                        }



                        textFormatter.DrawString("Endereço:", fonteDetalhesDescricaoTitulo, corFonte, new PdfSharpCore.Drawing.XRect(31, alturaDetalhesItens + 230, page.Width, page.Height));
                        if (!String.IsNullOrWhiteSpace(Convert.ToString(cs.Localizacao)))
                        {
                            textFormatter.DrawString(Convert.ToString(cs.Localizacao), fonteDetalhesDescricao, corFonte, new PdfSharpCore.Drawing.XRect(31, alturaDetalhesItens + 240, page.Width, page.Height));
                        }

                        textFormatter.DrawString("Data da Fiscalização:", fonteDetalhesDescricaoTitulo, corFonte, new PdfSharpCore.Drawing.XRect(31, alturaDetalhesItens + 255, page.Width, page.Height));
                        if (!String.IsNullOrWhiteSpace(Convert.ToString(cs.Data)))
                        {
                            textFormatter.DrawString(Convert.ToString(cs.Data), fonteDetalhesDescricao, corFonte, new PdfSharpCore.Drawing.XRect(31, alturaDetalhesItens + 265, page.Width, page.Height));
                        }

                        //Segunda Coluna Fiscal

                        textFormatter.DrawString("Observação da Fiscalização:", fonteDetalhesDescricaoTitulo, corFonte, new PdfSharpCore.Drawing.XRect(31, alturaDetalhesItens + 280, page.Width, page.Height));
                        if (!String.IsNullOrWhiteSpace(Convert.ToString(cs.Obs)))
                        {
                            textFormatter.DrawString(Convert.ToString(cs.Obs), fonteDetalhesDescricao, corFonte, new PdfSharpCore.Drawing.XRect(31, alturaDetalhesItens + 290, page.Width, page.Height));
                        }

                        textFormatter.DrawString("Identificação da Caçamba:", fonteDetalhesDescricaoTitulo, corFonte, new PdfSharpCore.Drawing.XRect(31, alturaDetalhesItens + 305, page.Width, page.Height));
                        if (!String.IsNullOrWhiteSpace(Convert.ToString(cs.IdentificacaoCacamba)))
                        {
                            textFormatter.DrawString(Convert.ToString(cs.IdentificacaoCacamba), fonteDetalhesDescricao, corFonte, new PdfSharpCore.Drawing.XRect(31, alturaDetalhesItens + 315, page.Width, page.Height));
                        }

                        //textFormatter.DrawString("Tipo de Infração:", fonteDetalhesDescricaoTitulo, corFonte, new PdfSharpCore.Drawing.XRect(300, alturaDetalhesItens + 230, page.Width, page.Height));
                        //if (!String.IsNullOrWhiteSpace(Convert.ToString(cs.TipoInfracao)))
                        //{
                        //    textFormatter.DrawString(Convert.ToString(cs.TipoInfracao), fonteDetalhesDescricao, corFonte, new PdfSharpCore.Drawing.XRect(300, alturaDetalhesItens + 240, page.Width, page.Height));
                        //}

                        textFormatter.DrawString("Imagens", fonteDetalhesDescricaoTitulo, corFonte, new PdfSharpCore.Drawing.XRect(31, alturaDetalhesItens + 330, page.Width, page.Height));

                        if (!string.IsNullOrEmpty(cs.Foto1))
                        {
                           
                            var caminhoLogo1 = Path.Combine(_webHostEnvironment.WebRootPath, @"arquivos\notificacoes\"+busca +"\\"+ cs.Foto1);
                            var logo1 = caminhoLogo1;
                            // Impressão do LogoTipo
                            XImage imagem1 = XImage.FromFile(logo1);
                            graphics.DrawImage(imagem1, 31, alturaDetalhesItens + 345, 200, 200);
                            

                        }

                        if (!string.IsNullOrEmpty(cs.Foto2))
                        {
                            var caminhoLogo2 = Path.Combine(_webHostEnvironment.WebRootPath, @"arquivos\notificacoes\" + busca + "\\" + cs.Foto2);
                            var logo2 = caminhoLogo2;
                            // Impressão do LogoTipo
                            XImage imagem2 = XImage.FromFile(logo2);
                            graphics.DrawImage(imagem2, 250, alturaDetalhesItens + 345, 200, 200);
                        }

                    }

                }
                //Fim

                using (MemoryStream stream = new MemoryStream())
                {
                    var contantType = "application/pdf";
          
                    doc.Save(Path.Combine(_webHostEnvironment.WebRootPath, @"arquivos\notificacoes\" + busca + "\\" + nmarquivo +arquivoPrefixo ));

                    //Não pode envia direto 
                    Enviar(email, Path.Combine(_webHostEnvironment.WebRootPath, @"arquivos\notificacoes\" + busca + "\\" + arquivoPrefixo));

                    //path = Path.Combine(webRootPath, "arquivos/notificacoes/" + notificacoes.Id);

                    doc.Save(stream, false);
                    var nomeArquivo = nmarquivo+arquivoPrefixo;
                    //var nomeArquivo = "notificacao_"+ nmarquivo;

                   return File(stream.ToArray(), contantType, nomeArquivo);
                   
                }


            }


        }






    }
}
