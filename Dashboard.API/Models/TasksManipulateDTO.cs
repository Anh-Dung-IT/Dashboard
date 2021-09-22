using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.API.Models
{
    public class TasksManipulateDTO
    {
        /// <summary>
        /// Task title
        /// </summary>
        [Required, StringLength(200)]
        public string TaskTitle { get; set; }

        /// <summary>
        /// Determine whether the task is complete or not
        /// </summary>
        [Required]
        public bool IsCompleted { get; set; }
    }
}
