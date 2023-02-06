using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace cacambaonline.Models
{
    public class RolesViewModel
    {
        public IEnumerable<IdentityRole>? Roles  { get; set; }
        public IdentityUser? Usuario { get; set; }
       
        public string? Role { get; set; }

    }
}
