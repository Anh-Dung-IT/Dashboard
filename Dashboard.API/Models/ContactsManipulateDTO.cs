using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.API.Models
{
    public class ContactsManipulateDTO
    {
        [Required, StringLength(100)]
        public string Firstname { get; set; }

        [Required, StringLength(100)]
        public string Lastname { get; set; }

        [Required, StringLength(200)]
        public string Title { get; set; }

        [Required, StringLength(200)]
        public string Department { get; set; }

        [Required, StringLength(200)]
        public string Project { get; set; }
        public string AvatarUrl { get; set; } = "";
    }
}
