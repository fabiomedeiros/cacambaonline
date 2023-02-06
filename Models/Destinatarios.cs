using System.ComponentModel.DataAnnotations;

namespace cacambaonline.Models
{
    public class Destinatarios
    {
        #region "Propriedades"
        [Key]
        public int Id { get; set; }
        public string? Cnpj { get; set; }
        public string? NomeEmpresarial { get; set; }
        public string? NomeFantasia { get; set; }
        public string? Telefone { get; set; }
        public string? Email { get; set; }
        public string? EnderecoEletronico { get; set; }
        public EnderecosDestinatario? EnderecosDestinatario { get; set; }
        public string? UsuarioId { get; set; }

        #endregion
    }
}
