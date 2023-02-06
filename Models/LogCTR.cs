using System.ComponentModel.DataAnnotations;

namespace cacambaonline.Models
{
    public class LogCTR
    {
        #region "Propriedades"
        [Key]
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public string? UsuarioId { get; set; }
        public int? CTRId { get; set; }
        /*EF Relation*/
        public CTR? CTR { get; set; }
        public string? Operacao { get; set; }

        public int? CTR_antigoId { get; set; }
        /*EF Relation*/
        public CTR_antigo? CTR_antigo { get; set; }

        //CTR alterado
        public int? CTR_novoId { get; set; }
        /*EF Relation*/
        public CTR_novo? CTR_novo { get; set; }


        #endregion
    }
}
