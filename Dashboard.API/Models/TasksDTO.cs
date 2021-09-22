using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.API.Models
{
    public class TasksDTO
    {
        public int TasksId { get; set; }
        public string TaskTitle { get; set; }
        public bool IsCompleted { get; set; }
        public int WidgetsId { get; set; }
    }
}
