using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.API.Entities
{
    public class Layouts
    {
        [Key]
        public int LayoutsId { get; set; }
        public int Column { get; set; }
        public int Row { get; set; }
    }
}
