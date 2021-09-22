using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.API.ResourceParameters
{
    public class ContactsFilterDTO
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
        public string Firstname { get; set; } = "";
        public string Lastname { get; set; } = "";
        public string Title { get; set; } = "";
        public string Department { get; set; } = "";
        public string Project { get; set; } = "";
    }
}
