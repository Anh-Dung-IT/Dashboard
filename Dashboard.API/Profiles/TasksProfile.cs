using AutoMapper;
using Dashboard.API.Entities;
using Dashboard.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.API.Profiles
{
    public class TasksProfile : Profile
    {
        public TasksProfile()
        {
            CreateMap<Tasks, TasksDTO>();
            CreateMap<Tasks, TasksManipulateDTO>();
            CreateMap<Tasks, TasksCreateDTO>();
            CreateMap<TasksManipulateDTO, Tasks>();
            CreateMap<TasksCreateDTO, Tasks>();
        }
    }
}
