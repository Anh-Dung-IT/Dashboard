using AutoMapper;
using Dashboard.API.Entities;
using Dashboard.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.API.Profiles
{
    public class ContactsProfile : Profile
    {
        public ContactsProfile()
        {
            CreateMap<Contacts, ContactsManipulateDTO>();
            CreateMap<ContactsManipulateDTO, Contacts>();
        }
    }
}
