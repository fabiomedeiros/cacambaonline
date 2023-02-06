using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cacambaonline.Models
{
    public class Geradores
    {
        #region "Propriedades"
        [Key]
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Cpf { get; set; }
        public string? Telefone { get; set; }
        public string? Email { get; set; }
        public DateTime? Data { get; set; }
        public EnderecosGerador? EnderecosGerador { get; set; }
        #endregion
    }
}
