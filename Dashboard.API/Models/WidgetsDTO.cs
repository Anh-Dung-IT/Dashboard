using Dashboard.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.API.Models
{
    public class WidgetsDTO
    {
        public int WidgetsId { get; set; }
        public string Title { get; set; }
        public int MinWidth { get; set; }
        public int MinHeight { get; set; }
        public int WidgetTypesId { get; set; }
        public WidgetTypes WidgetType { get; set; }
        public string Description { get; set; }
    }
}
