using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cacambaonline.Models
{
    public class Pessoas 
    {
        #region "Propriedades"
        [Key]
        public int Id { get; set; }
        public string AspNetUsersId { get; set; }
        public string? Nome { get; set; }
        public string? Cpf { get; set; }

        public string? Matricula { get; set; }

        public string? Cargo { get; set; }
        public string? Telefone { get; set; }


        public DateTime? Data { get; set; }


        #endregion
    }
}
