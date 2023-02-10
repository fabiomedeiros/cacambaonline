// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using cacambaonline.Data;
using cacambaonline.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace cacambaonline.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public readonly RoleManager<IdentityRole> _roleManager;

        private readonly MeuDbContext _context;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager,
            MeuDbContext context)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;

            _context = context;


        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {

            [Required(ErrorMessage = "O campo {0} é obrigatório")]
            public string Nome { get; set; }

            [Required(ErrorMessage = "O campo {0} é obrigatório")]
            [StringLength(11, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 11)]
            public string Cpf { get; set; }

            [Required(ErrorMessage = "O campo {0} é obrigatório")]
            public string Matricula { get; set; }
            [Required(ErrorMessage = "O campo {0} é obrigatório")]
            public string Cargo { get; set; }
            [Required(ErrorMessage = "O campo {0} é obrigatório")]
            public string Telefone { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required(ErrorMessage = "O campo {0} é obrigatório")]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required(ErrorMessage = "O campo {0} é obrigatório")]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [Display(Name = "Papel")]
            public string Papel { get; set; }


        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }


        public void Enviar(string destinatario, string link)
        {
            //Define os dados do e-mail
            string nomeRemetente = "Secretaria Municipal de Meio Ambiente";
            //smtp.Credentials = new System.Net.NetworkCredential("nao-responda@vilavelha.es.gov.br", "V7tklf4xy7maragLBYRU");

            string emailRemetente = "nao-responda@vilavelha.es.gov.br";
            string senha = "V7tklf4xy7maragLBYRU";

            //string emailRemetente = "avaliacaodesempenho@vilavelha.es.gov.br";
            //string senha = "avaliacao@2017";

            //Host da porta SMTP
            string SMTP = "correio.vilavelha.es.gov.br";

            string emailDestinatario = destinatario;
            //string emailComCopia        = "email@comcopia.com.br";
            //string emailComCopiaOculta  = "email@comcopiaoculta.com.br";

            string assuntoMensagem = "SECRETARIA MUNICIPAL DE MEIO AMBIENTE  - CAÇAMBA ONLINE - CONFIRMAR EMAIL.";

            //Gerar número randômico
            Random rand = new Random();
            int numeroaleatorio = rand.Next(10800, 50800);
            string stringaleatoria = destinatario.Substring(0, 5) + Convert.ToString(numeroaleatorio);

            //string conteudoMensagem = "Para resetar a senha é necessário abrir ou copiar o link a seguir em seu navegador - " + "https://novo.vilavelha.es.gov.br/habitavv/Identity/Account/ResetPassword?ale="+ code; //< a href = "http://www.vilavelha.es.gov.br/" target = "_blank"> </a>;

            string conteudoMensagem = "Para validar a senha é necessário abrir ou copiar o link a seguir em seu navegador - " + link; //< a href = "http://www.vilavelha.es.gov.br/" target = "_blank"> </a>;

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

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            //if (ModelState.IsValid)
            //{
                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

            
            var result = await _userManager.CreateAsync(user, Input.Password);


            //var user = new ApplicationUser()
            //{
            //    Nome = Input.Email,
            //    Cpf = Input.Cpf,
            //    Email = Input.Email,
            //    UserName = Input.Email
            //};


            if (result.Succeeded)
                {

                
               

                    _logger.LogInformation("O usuário criou uma nova conta com senha.");

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                //Complentar os dados tabela pessoa
                Pessoas p = new Pessoas();
                p.Cpf = Input.Cpf;
                p.Nome = Input.Nome;
                p.Matricula = Input.Matricula;
                p.Cargo = Input.Cargo;
                p.Telefone = Input.Telefone;
                p.AspNetUsersId = userId;
                p.Data = DateTime.Now;

                _context.Add(p);
                await _context.SaveChangesAsync();


                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    Enviar(Input.Email, $"<a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>Clique aqui</a>.");

                await _emailSender.SendEmailAsync(Input.Email, "Confirme seu e-mail",
                    $"Confirme sua conta por <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>Clique aqui</a>.");

                //-------------------atribuir role ao user------------------------------
                //var applicationRole = await _roleManager.FindByNameAsync(Input.Papel);
                //if (applicationRole != null)
                //{
                //    IdentityResult roleResult = await _userManager.AddToRoleAsync(user, applicationRole.Name);
                //}
                //-------------------atribuir role ao user------------------------------


                if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        //return Redirect("/cacambaonline/Home/Papel/?userid=" + userId);
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            //}

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private IdentityUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Não é possível criar uma instância de '{nameof(ApplicationUser)}'. " +
                    $"Garanta que '{nameof(ApplicationUser)}' não é uma classe abstrata e tem um construtor sem parâmetros, ou alternativamente " +
                    $"substituir a página de registro em /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("A interface do usuário padrão requer um repositório de usuários com suporte por e-mail.");
            }
            return (IUserEmailStore<IdentityUser>)_userStore;
        }
    }
}
