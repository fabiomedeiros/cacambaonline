using System.ComponentModel.DataAnnotations;

namespace cacambaonline.Models
{
    public class LogNotificacao
    {
        #region "Propriedades"
        [Key]
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public string? Operacao { get; set; }
        public string? UsuarioId { get; set; }
        public int? NotificacoesId { get; set; }
        /*EF Relation*/
        public Notificacoes? Notificacoes { get; set; }

        public int? Notificacoes_AntigoId { get; set; }
        /*EF Relation*/
        public Notificacoes_Antigo? Notificacoes_Antigo { get; set; }

        //CTR alterado
        public int? Notificacoes_NovoId { get; set; }
        /*EF Relation*/
        public Notificacoes_Novo? Notificacoes_Novo { get; set; }


        #endregion
    }
}
