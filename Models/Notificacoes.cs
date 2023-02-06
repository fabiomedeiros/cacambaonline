using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cacambaonline.Models
{
    public class Notificacoes
    {
        #region "Propriedades"
        [Key]
        public int Id { get; set; }
        public string? Descricao { get; set; }
        public string? Obs { get; set; }
        public string? Localizacao { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        //[NotMapped]
        //public IFormFile UploadFoto1 { get; set; } //anexo (Depois do scaffold temos que descomentar.)
        public string? Foto1 { get; set; }
        //[NotMapped]
        //public IFormFile UploadFoto2 { get; set; } //anexo (Depois do scaffold temos que descomentar.)
        public string? Foto2 { get; set; }
        public bool? Multa { get; set; }
        
        //Através deste conseguimos os dados da CTR
        public int? CTRId { get; set; }
        /*EF Relation*/
        public CTR? Ctr { get; set; }

        //Através deste conseguimos os dados da CTR
        public int InfracoesId { get; set; }
        /*EF Relation*/
        public Infracoes? Infracoes { get; set; }

        //Quando não houver CTR
        public int? TransportadoresId { get; set; }
        /*EF Relation*/
        public Transportadores? Transportadores { get; set; }

        public int? CacambasId { get; set; }
        /*EF Relation*/
        public Cacambas? Cacambas { get; set; }

        //Fim quando não é CTR

        [StringLength(450,ErrorMessage ="Máximo de 450 caracteres")]
        public string? UsuarioFiscalId { get; set; }


        [NotMapped]
        public IFormFile UploadFoto1 { get; set; } //anexo (Depois do scaffold temos que descomentar.)

        [NotMapped]
        public IFormFile UploadFoto2 { get; set; } //anexo (Depois do scaffold temos que descomentar.)

        public DateTime? Data { get; set; }

        public bool? Excluido { get; set; }

        public string? Arquivo { get; set; }


        #endregion
    }
}
