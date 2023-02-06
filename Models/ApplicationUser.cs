using Microsoft.AspNetCore.Identity;

namespace cacambaonline.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Nome { get; set; }
        public string Cpf { get; set; }
    }
}
