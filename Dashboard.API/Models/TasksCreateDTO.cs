using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.API.Models
{
    public class TasksCreateDTO : TasksManipulateDTO
    {
        [Required]
        public int WidgetsId { get; set; }
    }
}
