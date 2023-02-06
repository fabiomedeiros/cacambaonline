using System.ComponentModel.DataAnnotations;

namespace cacambaonline.Models
{
    public class CTR
    {
        #region "Propriedades"
        [Key]
        public int Id { get; set; }
        public string? Localizacao { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public DateTime Data { get; set; }

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

        //Quando não há CTR. Essa notificação vai virar CTR, porém com este campo preenchido na controller Notificacao/Create. Isso para solucionar o problema do mapa que era 2 cores, agora são 4.
        public string? VeiodaNotificacao { get; set; }


        //Avulso
        //Dados do Gerador
        //public string? Nome_Gerador { get; set; }
        //public string? Cpf_Gerador { get; set; }
        //public string? Telefone_Gerador { get; set; }
        //public string? Endereco_Gerador { get; set; }


        //Dados Transportadora
        //public string? NomeEmpresarial_Transportadora { get; set; }
        //public string? NomeFantasia_Transportadora { get; set; }
        //public string? Telefone_Transportadora { get; set; }
        //public string? Cnpj_Transportadora { get; set; }
        //Dados Caçamba
        //public string? Numero { get; set; }
        //Fim Avulso



        #endregion
    }
}
