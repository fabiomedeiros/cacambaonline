using System.ComponentModel.DataAnnotations;

namespace cacambaonline.Models
{
    public class Cacambas
    {
        #region "Propriedades"
        [Key]
        public int Id { get; set; }
        public string? Numero { get; set; }
        public string? Descricao { get; set; }
        public string? Obs { get; set; }
        public bool? Ocupada { get; set; }
        public int TransportadoresId { get; set; }
        /*EF Relation*/
        public Transportadores? Transportadores { get; set; }
        #endregion
    }
}
