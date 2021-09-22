using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.API.Entities
{
    public class Dashboards
    {
        [Key]
        public int DashboardsId { get; set; }

        [Required, StringLength(200)]
        public string Title { get; set; }
        public string Username { get; set; }

        [ForeignKey(nameof(Username))]
        public Accounts Account { get; set; }
        public int LayoutsId { get; set; }

        [ForeignKey(nameof(LayoutsId))]
        public Layouts Layout { get; set; }
        public List<Widgets> Widgets { get; set; }
    }
}
