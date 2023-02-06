using Microsoft.AspNetCore.SignalR;
using System.ComponentModel.DataAnnotations;

namespace cacambaonline.Models
{
    public class Usuarios
    {
        public string? Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
    }
}
