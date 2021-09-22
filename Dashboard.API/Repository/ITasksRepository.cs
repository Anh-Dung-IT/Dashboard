using Dashboard.API.Entities;
using Dashboard.API.ResourceParameters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dashboard.API.Repository
{
    public interface ITasksRepository
    {
        IEnumerable<Tasks> GetTasks(string username, TasksFilterDTO tasksFilter, out int count);
        Tasks GetTask(int id, string username);
        Tasks AddTask(Tasks task);
        void DeleteTask(Tasks task);
        Tasks UpdateTask(Tasks task);
    }
}
