using AutoMapper;
using Dashboard.API.Entities;
using Dashboard.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.API.Profiles
{
    public class DashboardsProfile : Profile
    {
        public DashboardsProfile()
        {
            CreateMap<Dashboards, DashboardsDTO>();
            CreateMap<Dashboards, DashboardsManipulateDTO>();
            CreateMap<DashboardsManipulateDTO, Dashboards>();
        }
    }
}
