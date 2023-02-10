using Microsoft.AspNetCore.Identity;

namespace cacambaonline.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Matricula { get; set; }
        public string Cargo { get; set; }
        public string Telefone { get; set; }
    }
}
