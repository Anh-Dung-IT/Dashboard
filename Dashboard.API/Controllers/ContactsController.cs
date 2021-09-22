using AutoMapper;
using Dashboard.API.Entities;
using Dashboard.API.Helper;
using Dashboard.API.Models;
using Dashboard.API.Repository;
using Dashboard.API.ResourceParameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Dashboard.API.Controllers
{
    [Route("api/contacts")]
    [ApiController]
    //[Authorize]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class ContactsController : ControllerBase
    {
        private readonly IContactsRepository _contactsRepository;
        private readonly IMapper _mapper;

        public ContactsController(IContactsRepository contactsRepository, IMapper mapper)
        {
            this._contactsRepository = contactsRepository;
            this._mapper = mapper;
        }

        /// <summary>
        /// Get list contacts
        /// </summary>
        [HttpGet(Name = "GetContacts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<IEnumerable<Contacts>> GetContacts([FromQuery] ContactsFilterDTO contactsFilter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var contacts = _contactsRepository.GetContacts(contactsFilter, out var count);

            if (!contacts.Any())
            {
                return NotFound("Not found this contact");
            }

            var pageResponse = CreateResourceUri(contactsFilter, count);

            Response.Headers.Add("X-Paging", JsonConvert.SerializeObject(pageResponse));

            return Ok(contacts);
        }

        /// <summary>
        /// Get specific contact
        /// </summary>
        /// <param name="id">Contact's Id</param>
        [HttpGet("{id}", Name = "GetContact")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Contacts> GetContact(int id)
        {
            var contact = _contactsRepository.GetContact(id);

            if (contact == null)
            {
                return NotFound("Not found this contact");
            }

            return Ok(contact);
        }

        /// <summary>
        /// Update specific contact
        /// </summary>
        /// <param name="id">Contact's Id</param>
        /// <param name="contact"></param>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult UpdateContact(int id, [FromBody] ContactsManipulateDTO contact)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var contactToUpdate = _contactsRepository.GetContact(id);

            if (contactToUpdate == null)
            {
                return NotFound("Not found this contact");
            }

            _mapper.Map(contact, contactToUpdate);
            _contactsRepository.UpdateContact(contactToUpdate);

            return NoContent();
        }

        /// <summary>
        /// Delete specific contact
        /// </summary>
        /// <param name="id">Contact's Id</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult DeleteContact(int id)
        {
            var contact = _contactsRepository.GetContact(id);

            if (contact == null)
            {
                return NotFound("Not found this contact");
            }

            _contactsRepository.DeleteContact(contact);

            return NoContent();
        }

        /// <summary>
        /// Create new a contact
        /// </summary>
        /// <param name="contact"></param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public ActionResult<Contacts> AddContacts([FromBody] Contacts contact)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            contact = _contactsRepository.AddContact(contact);

            if (contact == null)
            {
                return BadRequest("Duplicate id");
            }

            return CreatedAtRoute("GetContact", new { id = contact.EmployeeId }, contact);
        }

        /// <summary>
        /// Import Contact csv file
        /// </summary>
        /// <param name="file"></param>
        [HttpPost("csvReport")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AddCsv(IFormFile file)
        {
            if (!file.FileName.EndsWith(".csv"))
            {
                return BadRequest();
            }

            var listContact = new List<Contacts>();

            using (var sreader = new StreamReader(file.OpenReadStream()))
            {
                string[] headers = sreader.ReadLine().Split(',');     //Title
                while (!sreader.EndOfStream)                          //get all the content in rows 
                {
                    string[] rows = sreader.ReadLine().Split(',');

                    bool parseSuccess = int.TryParse(rows[0].Trim(), out int employeeId);
                    string firstname = rows[1].Trim();
                    string lastname = rows[2].Trim();
                    string title = rows[3].Trim();
                    string department = rows[4].Trim();
                    string project = rows[5].Trim();
                    string avatarUrl = rows[6].Trim();

                    if (!parseSuccess)
                    {
                        return BadRequest("EmployeeId not a number");
                    }

                    var contact = new Contacts
                    {
                        EmployeeId = employeeId,
                        Firstname = firstname,
                        Lastname = lastname,
                        Title = title,
                        Department = department,
                        Project = project,
                        AvatarUrl = avatarUrl
                    };

                    listContact.Add(contact);
                }
            }

            var result = _contactsRepository.AddContacts(listContact);

            if (!result) return BadRequest("Duplicate id");

            return Ok();
        }

        /// <summary>
        /// Export Contacts csv file
        /// </summary>
        /// <param name="contactsFilter"></param>
        /// <returns></returns>
        [HttpGet("csvReport")]
        [Produces("text/csv")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetCsv([FromQuery] ContactsFilterDTO contactsFilter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var contacts = _contactsRepository.GetContacts(contactsFilter, out var count);

            if (!contacts.Any())
            {
                return NotFound("Not found this contact");
            }

            string header = "EmployeeId,Firstname,Lastname,Title,Department,Project,AvatarUrl";
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine(header);
            foreach (var c in contacts)
            {
                stringBuilder.AppendLine($"{c.EmployeeId},{c.Firstname},{c.Lastname},{c.Title},{c.Department},{c.Project},{c.AvatarUrl}");
            }

            string fileName = $"Contacts_page{contactsFilter.PageNumber}.size{contactsFilter.PageSize}.csv";

            return new CsvResult(stringBuilder.ToString(), fileName);
        }

        private PageResponse CreateResourceUri(ContactsFilterDTO contactsFilter, int count)
        {
            var currentPage = contactsFilter.PageNumber;
            var pageSize = contactsFilter.PageSize;
            var totalCount = count;
            var totalPage = (int)Math.Ceiling(1.0 * totalCount / pageSize);

            var pPre = currentPage - 1;
            var pNext = currentPage + 1;

            string pagePrevious = null;
            string pageNext = null;

            if (currentPage > 1)
            {
                pagePrevious = Url.Link("GetContacts", new ContactsFilterDTO
                {
                    Firstname = contactsFilter.Firstname,
                    Lastname = contactsFilter.Lastname,
                    Title = contactsFilter.Title,
                    Department = contactsFilter.Department,
                    Project = contactsFilter.Project,
                    PageSize = contactsFilter.PageSize,
                    PageNumber = pPre
                });
            }

            if (currentPage < totalPage)
            {
                pageNext = Url.Link("GetContacts", new ContactsFilterDTO
                {
                    Firstname = contactsFilter.Firstname,
                    Lastname = contactsFilter.Lastname,
                    Title = contactsFilter.Title,
                    Department = contactsFilter.Department,
                    Project = contactsFilter.Project,
                    PageSize = contactsFilter.PageSize,
                    PageNumber = pNext
                });
            }
            return new PageResponse(totalPage, currentPage, pageSize, totalCount, pagePrevious, pageNext);
        }
    }
}
