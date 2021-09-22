using AutoMapper;
using Dashboard.API.Entities;
using Dashboard.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.API.Profiles
{
    public class AccountsProfile : Profile
    {
        public AccountsProfile()
        {
            CreateMap<Accounts, AccountsDTO>();
            CreateMap<Accounts, AccountsRegisterDTO>();
            CreateMap<AccountsRegisterDTO, Accounts>();
        }
    }
}
