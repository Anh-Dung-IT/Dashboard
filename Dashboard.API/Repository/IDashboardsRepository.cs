using Dashboard.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.API.Repository
{
    public interface IDashboardsRepository
    {
        IEnumerable<Dashboards> GetDashboards(string username);
        Dashboards GetDashboard(int id, string username);
        bool UpdateDashboards(Dashboards dashboard);
    }
}
