using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.API.ResourceParameters
{
    public class TasksFilterDTO
    {
        private int _pageNumber = 1;

        /// <summary>
        /// Page number want to get
        /// </summary>
        public int PageNumber
        {
            get => _pageNumber;
            set => _pageNumber = (value <= 0) ? 1 : value;
        }

        /// <summary>
        /// Size of page
        /// </summary>
        [Range(1, 50)]
        public int PageSize { get; set; } = 1;

        /// <summary>
        /// Task title
        /// </summary>
        public string TaskTitle { get; set; } = "";

        /// <summary>
        /// Determine whether the task is complete or not
        /// </summary>
        public bool? IsCompleted { get; set; }

        /// <summary>
        /// Widget's Id
        /// </summary>
        public int? WidgetsId { get; set; }
    }
}
