using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.API.Entities
{
    public class Tasks
    {
        [Key]
        public int TasksId { get; set; }

        [Required, StringLength(200)]
        public string TaskTitle { get; set; }

        [Required]
        public bool IsCompleted { get; set; }

        [Required]
        public string Username { get; set; }

        [ForeignKey(nameof(Username))]
        public Accounts Account { get; set; }

        [Required]
        public int WidgetsId { get; set; }

        [ForeignKey(nameof(WidgetsId))]
        public Widgets Widget { get; set; }
    }
}
