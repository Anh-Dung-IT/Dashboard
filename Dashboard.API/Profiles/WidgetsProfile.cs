using AutoMapper;
using Dashboard.API.Entities;
using Dashboard.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.API.Profiles
{
    public class WidgetsProfile : Profile
    {
        public WidgetsProfile()
        {
            CreateMap<Widgets, WidgetsDTO>();
        }
    }
}
