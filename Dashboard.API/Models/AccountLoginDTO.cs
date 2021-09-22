using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.API.Models
{
    public class AccountLoginDTO
    {
        [Required, StringLength(50)]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
