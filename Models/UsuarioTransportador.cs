using System.ComponentModel.DataAnnotations;

namespace cacambaonline.Models
{
    public class UsuarioTransportador
    {
        #region "Propriedades"
        [Key]
        public int Id { get; set; }
        public string? UserId { get; set; }
        
        public int? TransportadoresId { get; set; }
        /*EF Relation*/
        public Transportadores? Transportadores { get; set; }

        #endregion
    }
}
