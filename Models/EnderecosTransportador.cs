using System.ComponentModel.DataAnnotations;

namespace cacambaonline.Models
{
    public class EnderecosTransportador
    {
        #region "Propriedades"
        [Key]
        public int Id { get; set; }
        public string? CEP { get; set; }
        public string? Cidade { get; set; }
        public string? Estado { get; set; }
        public string? Bairro { get; set; }
        public string? Logradouro { get; set; }
        public string? Numero { get; set; }
        public string? Complemento { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }

        public int TransportadoresId { get; set; }
        /*EF Relation*/
        public Transportadores? Transportadores { get; set; }
        #endregion

    }
}
