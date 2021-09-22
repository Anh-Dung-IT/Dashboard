using Dashboard.API.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dashboard.API
{
    public class DashboardContext : DbContext
    {
        public DbSet<Accounts> Accounts { get; set; }
        public DbSet<Tasks> Tasks { get; set; }
        public DbSet<Contacts> Contacts { get; set; }
        public DbSet<Dashboards> Dashboards { get; set; }
        public DbSet<Layouts> Layouts { get; set; }
        public DbSet<Widgets> Widgets { get; set; }
        public DbSet<RefreshTokens> RefreshTokens { get; set; }

        public DashboardContext(DbContextOptions<DashboardContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Prepare data for test
            modelBuilder.Entity<Contacts>().HasData(
                new Contacts { EmployeeId = 1, Firstname = "First", Lastname = "Person", Title = "DEV", Department = "D1", Project = "Project1", AvatarUrl = "avatar1" },
                new Contacts { EmployeeId = 2, Firstname = "second", Lastname = "Person", Title = "DEV", Department = "D2", Project = "Project2", AvatarUrl = "avatar2" },
                new Contacts { EmployeeId = 3, Firstname = "third", Lastname = "Person", Title = "DEV", Department = "D3", Project = "Project3", AvatarUrl = "avatar3" },
                new Contacts { EmployeeId = 4, Firstname = "fourth", Lastname = "Person", Title = "DEV", Department = "D4", Project = "Project4", AvatarUrl = "avatar4" },
                new Contacts { EmployeeId = 5, Firstname = "fifth", Lastname = "Person", Title = "DEV", Department = "D5", Project = "Project5", AvatarUrl = "avatar5" });

            modelBuilder.Entity<Accounts>().HasData(
                new Accounts { Username = "admin", Password = "$2a$11$8NwgycyRH50lI3/gy/ncu.VxXdHFwmpvwCsnmrLjwa97lgUHqbvsG", Firstname = "admin", Lastname = "admin", Email = "abc@gmail.com" },
                new Accounts { Username = "admin1", Password = "$2a$11$8NwgycyRH50lI3/gy/ncu.VxXdHFwmpvwCsnmrLjwa97lgUHqbvsG", Firstname = "admin1", Lastname = "admin1", Email = "abc1@gmail.com" });

            modelBuilder.Entity<WidgetTypes>().HasData(
                new WidgetTypes { WidgetTypesId = 1, WidgetTypesName = "Widget Task" },
                new WidgetTypes { WidgetTypesId = 2, WidgetTypesName = "Widget Note" },
                new WidgetTypes { WidgetTypesId = 3, WidgetTypesName = "Widget Contact" });

            modelBuilder.Entity<Layouts>().HasData(
                new Layouts { LayoutsId = 1, Column = 1, Row = 1 },
                new Layouts { LayoutsId = 2, Column = 2, Row = 2 },
                new Layouts { LayoutsId = 3, Column = 3, Row = 3 });

            modelBuilder.Entity<Dashboards>().HasData(
                new Dashboards { DashboardsId = 1, Title = "Dashboard1", Username = "admin", LayoutsId = 1 });

            modelBuilder.Entity<Widgets>().HasData(
                new Widgets { WidgetsId = 1, Title = "Widget1", MinWidth = 10, MinHeight = 10, WidgetTypesId = 1, DashboardsId = 1 },
                new Widgets { WidgetsId = 2, Title = "Widget2", MinWidth = 20, MinHeight = 20, WidgetTypesId = 2, DashboardsId = 1, Description = "for test" },
                new Widgets { WidgetsId = 3, Title = "Widget3", MinWidth = 30, MinHeight = 30, WidgetTypesId = 3, DashboardsId = 1 });

            modelBuilder.Entity<Tasks>().HasData(
                new Tasks { TasksId = 1, TaskTitle = "Task1", IsCompleted = false, Username = "admin", WidgetsId = 1 },
                new Tasks { TasksId = 2, TaskTitle = "Task2", IsCompleted = false, Username = "admin", WidgetsId = 1 },
                new Tasks { TasksId = 3, TaskTitle = "Task3", IsCompleted = true, Username = "admin", WidgetsId = 1 },
                new Tasks { TasksId = 4, TaskTitle = "Task4", IsCompleted = false, Username = "admin", WidgetsId = 1 });

        }
    }
}
