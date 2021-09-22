using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Dashboard.API.Entities
{
    public class Widgets
    {
        [Key]
        public int WidgetsId { get; set; }

        [Required, StringLength(200)]
        public string Title { get; set; }
        public int MinWidth { get; set; }
        public int MinHeight { get; set; }
        public int WidgetTypesId { get; set; }

        [ForeignKey(nameof(WidgetTypesId))]
        public WidgetTypes WidgetType { get; set; }
        public string Description { get; set; }

        [Required]
        public int DashboardsId { get; set; }

        [ForeignKey(nameof(DashboardsId))]
        public Dashboards Dashboard { get; set; }
        public List<Tasks> Tasks { get; set; }
    }
}
