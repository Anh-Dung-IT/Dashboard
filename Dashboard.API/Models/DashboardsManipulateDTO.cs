using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.API.Models
{
    public class DashboardsManipulateDTO
    {
        [Required, StringLength(200)]
        public string Title { get; set; }

        [Required]
        public int LayoutsId { get; set; }
    }
}
