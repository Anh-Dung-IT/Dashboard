using Dashboard.API.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Dashboard.API.Repository
{
    public class DashboardsRepository : IDashboardsRepository
    {
        private readonly DashboardContext _dashboardContext;

        public DashboardsRepository(DashboardContext dashboardContext)
        {
            this._dashboardContext = dashboardContext;
        }

        public Dashboards GetDashboard(int id, string username)
        {
            return _dashboardContext.Dashboards.FirstOrDefault(d => d.Username.Equals(username) && d.DashboardsId == id);
        }

        public IEnumerable<Dashboards> GetDashboards(string username)
        {
            return _dashboardContext.Dashboards.Where(d => d.Username.Equals(username))
                                               .Include(d => d.Layout)
                                               .Include(d => d.Widgets).ThenInclude(w => w.WidgetType)
                                               .ToList();
        }

        public bool UpdateDashboards(Dashboards dashboard)
        {
            var layout = _dashboardContext.Layouts.FirstOrDefault(l => l.LayoutsId == dashboard.LayoutsId);

            if (layout == null) return false;

            _dashboardContext.Dashboards.Update(dashboard);
            _dashboardContext.SaveChanges();

            return true;
        }
    }
}
