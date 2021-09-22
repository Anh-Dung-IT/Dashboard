using Dashboard.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.API.Models
{
    public class DashboardsDTO
    {
        public int DashboardsId { get; set; }
        public string Title { get; set; }
        public string Username { get; set; }
        public int LayoutsId { get; set; }
        public Layouts Layout { get; set; }
        public List<WidgetsDTO> Widgets { get; set; }
    }
}
