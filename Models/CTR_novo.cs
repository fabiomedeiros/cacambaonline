using System.ComponentModel.DataAnnotations;

namespace cacambaonline.Models
{
    public class CTR_novo

    {
        #region "Propriedades"
        [Key]
        public int Id { get; set; }
        public string? Localizacao { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public DateTime? Data { get; set; }

        public int? TransportadoresId { get; set; }
        /*EF Relation*/
        public Transportadores? Transportadores { get; set; }

        public int? CacambasId { get; set; }
        /*EF Relation*/
        public Cacambas? Cacambas { get; set; }

        //public int? GeradoresId { get; set; }
        ///*EF Relation*/
        //public Geradores? Geradores { get; set; }

        public int? DestinatariosId { get; set; }
        /*EF Relation*/
        public Destinatarios? Destinatarios { get; set; }

        public string? UsuarioId { get; set; }

        public bool? Fechada { get; set; }

        public bool? Excluido { get; set; }
        public bool? Notificado { get; set; }
        public bool? Multado { get; set; }

        public DateTime? DataBaixa { get; set; }

        #endregion
    }
}
