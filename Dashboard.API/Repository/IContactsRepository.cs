using Dashboard.API.Entities;
using Dashboard.API.ResourceParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.API.Repository
{
    public interface IContactsRepository
    {
        IEnumerable<Contacts> GetContacts(ContactsFilterDTO contactsFilter, out int count);
        Contacts GetContact(int id);
        Contacts AddContact(Contacts contact);
        void UpdateContact(Contacts contact);
        void DeleteContact(Contacts contact);
        bool AddContacts(IEnumerable<Contacts> contacts);
    }
}
