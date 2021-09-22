using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.API.Models
{
    public class AccountsRegisterDTO
    {
        [Required, StringLength(50)]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required, StringLength(200)]
        public string Firstname { get; set; }

        [Required, StringLength(200)]
        public string Lastname { get; set; }

        [Required, StringLength(400)]
        public string Email { get; set; }
    }
}
