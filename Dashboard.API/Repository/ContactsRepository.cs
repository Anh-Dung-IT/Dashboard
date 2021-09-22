using Dashboard.API.Entities;
using Dashboard.API.ResourceParameters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dashboard.API.Repository
{
    public class ContactsRepository : IContactsRepository
    {
        private readonly DashboardContext _dashboardContext;
        private readonly ILogger _logger;

        public ContactsRepository(DashboardContext dashboardContext, ILogger<ContactsRepository> logger)
        {
            this._dashboardContext = dashboardContext;
            this._logger = logger;
        }

        public Contacts AddContact(Contacts contact)
        {
            var contactFromDatabase = _dashboardContext.Contacts.FirstOrDefault(c => c.EmployeeId == contact.EmployeeId);

            if (contactFromDatabase != null)
            {
                return null;
            }

            _dashboardContext.Contacts.Add(contact);
            _dashboardContext.SaveChanges();

            return contact;
        }

        public bool AddContacts(IEnumerable<Contacts> contacts)
        {
            try
            {
                _dashboardContext.Contacts.AddRange(contacts);
                _dashboardContext.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                return false;
            }

            return true;
        }

        public void DeleteContact(Contacts contact)
        {
            _dashboardContext.Contacts.Remove(contact);
            _dashboardContext.SaveChanges();
        }

        public Contacts GetContact(int id)
        {
            var contact = _dashboardContext.Contacts.FirstOrDefault(c => c.EmployeeId == id);
            return contact;
        }

        public IEnumerable<Contacts> GetContacts(ContactsFilterDTO contactsFilter, out int count)
        {
            var contacts = _dashboardContext.Contacts.Where(c => c.Firstname.Contains(contactsFilter.Firstname)
                                                      && c.Lastname.Contains(contactsFilter.Lastname)
                                                      && c.Title.Contains(contactsFilter.Title)
                                                      && c.Department.Contains(contactsFilter.Department)
                                                      && c.Project.Contains(contactsFilter.Project));

            count = contacts.Count();

            return contacts.Skip((contactsFilter.PageNumber - 1) * contactsFilter.PageSize).Take(contactsFilter.PageSize).OrderBy(c => c.EmployeeId).ToList();
        }

        public void UpdateContact(Contacts contact)
        {
            _dashboardContext.Contacts.Update(contact);
            _dashboardContext.SaveChanges();
        }
    }
}
