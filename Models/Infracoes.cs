using System.ComponentModel.DataAnnotations;

namespace cacambaonline.Models
{
    public class Infracoes
    {
        #region "Propriedades"
        [Key]
        public int Id { get; set; }
        public string? Descricao { get; set; }
        public string? Obs { get; set; }
        public int? Prazo { get; set; }
      

        //[StringLength(450, ErrorMessage = "Máximo de 450 caracteres")]
        //public string UsuarioFiscalId { get; set; }

        #endregion
    }
}
