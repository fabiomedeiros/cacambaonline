using System.ComponentModel.DataAnnotations;

namespace cacambaonline.Models
{
    public class UsuarioDestinatario
    {
        #region "Propriedades"
        [Key]
        public int Id { get; set; }
        public string? UserId { get; set; }
        
        public int? DestinatariosId { get; set; }
        /*EF Relation*/
        public Destinatarios? Destinatarios { get; set; }

        #endregion
    }
}
