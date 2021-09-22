using Dashboard.API.Configure;
using Dashboard.API.Entities;
using Dashboard.API.ResourceParameters;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;

namespace Dashboard.API.Repository
{
    public class TasksRepository : ITasksRepository
    {
        private readonly DashboardContext _dashboardContext;
        private readonly DataFromDatabaseConfig _dataFromDatabaseConfig;

        public TasksRepository(DashboardContext dashboardContext, IOptionsSnapshot<DataFromDatabaseConfig> options)
        {
            this._dashboardContext = dashboardContext;
            this._dataFromDatabaseConfig = options.Value;
        }

        public Tasks AddTask(Tasks task)
        {
            var widget = _dashboardContext.Widgets.FirstOrDefault(w => w.WidgetsId == task.WidgetsId);

            if (widget == null)
            {
                return null;
            }

            if (widget.WidgetTypesId != _dataFromDatabaseConfig.WIDGET_TASK_ID)
            {
                return null;
            }

            _dashboardContext.Tasks.Add(task);
            _dashboardContext.SaveChanges();

            return task;
        }

        public Tasks GetTask(int id, string username)
        {
            var task = _dashboardContext.Tasks.FirstOrDefault(t => t.TasksId == id && t.Username.Equals(username));
            return task;
        }

        public IEnumerable<Tasks> GetTasks(string username, TasksFilterDTO tasksFilter, out int count)
        {
            var listTasks = _dashboardContext.Tasks.Where(t => t.Username.Equals(username) && t.TaskTitle.Contains(tasksFilter.TaskTitle));

            if (tasksFilter.WidgetsId != null)
            {
                listTasks = listTasks.Where(t => t.WidgetsId == tasksFilter.WidgetsId);
            }

            if (tasksFilter.IsCompleted != null)
            {
                listTasks = listTasks.Where(t => t.IsCompleted == tasksFilter.IsCompleted);
            }

            count = listTasks.Count();

            return listTasks.Skip((tasksFilter.PageNumber - 1) * tasksFilter.PageSize).Take(tasksFilter.PageSize).ToList();
        }

        public void DeleteTask(Tasks task)
        {
            _dashboardContext.Tasks.Remove(task);
            _dashboardContext.SaveChanges();
        }

        public Tasks UpdateTask(Tasks task)
        {
            _dashboardContext.Tasks.Update(task);
            _dashboardContext.SaveChanges();
            return task;
        }
    }
}
