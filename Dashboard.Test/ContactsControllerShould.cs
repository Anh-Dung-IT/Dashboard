using AutoMapper;
using Dashboard.API.Controllers;
using Dashboard.API.Entities;
using Dashboard.API.Models;
using Dashboard.API.Repository;
using Dashboard.API.ResourceParameters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Dashboard.Test
{
    public class ContactsControllerShould
    {
        private Mock<IContactsRepository> _mockRepo;
        private Mock<IMapper> _mockMapper;
        private Mock<HttpContext> _context;

        public ContactsControllerShould()
        {
            _mockRepo = new Mock<IContactsRepository>();
            _mockMapper = new Mock<IMapper>();
            _context = new Mock<HttpContext>();

            var mockHeader = new Mock<IHeaderDictionary>();

            _context.SetupGet(x => x.Response.Headers).Returns(mockHeader.Object);
        }

        private IEnumerable<Contacts> GetContactsFake()
        {
            return new List<Contacts> { new Contacts() };
        }

        [Fact]
        public void GetContacts_WithIncorrectData_Return400BadRequest()
        {
            // Arrange
            var controller = new ContactsController(_mockRepo.Object, _mockMapper.Object);

            controller.ModelState.AddModelError("data", "error");

            // Act
            var result = controller.GetContacts(It.IsAny<ContactsFilterDTO>());

            // Assert
            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Fact]
        public void GetContacts_WithNoData_Return404NotFound()
        {
            // Arrange
            int count = 0;

            _mockRepo.Setup(x => x.GetContacts(It.IsAny<ContactsFilterDTO>(), out count)).Returns(new List<Contacts>());

            var controller = new ContactsController(_mockRepo.Object, _mockMapper.Object);

            // Act
            var result = controller.GetContacts(It.IsAny<ContactsFilterDTO>());

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public void GetContactss_WithHasData_Return200OK()
        {
            // Arrange
            int count = 0;
            var contactsFilter = new ContactsFilterDTO();

            _mockRepo.Setup(x => x.GetContacts(It.IsAny<ContactsFilterDTO>(), out count)).Returns(GetContactsFake());

            var controller = new ContactsController(_mockRepo.Object, _mockMapper.Object);
            controller.ControllerContext.HttpContext = _context.Object;

            // Act
            var result = controller.GetContacts(contactsFilter);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void GetContact_WithNoData_Return404NotFound()
        {
            // Arrange
            Contacts contact = null;

            _mockRepo.Setup(x => x.GetContact(It.IsAny<int>())).Returns(contact);

            var controller = new ContactsController(_mockRepo.Object, _mockMapper.Object);

            // Act
            var result = controller.GetContact(It.IsAny<int>());

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public void GetContact_WithHasData_Return200Ok()
        {
            // Arrange
            Contacts contact = new Contacts();

            _mockRepo.Setup(x => x.GetContact(It.IsAny<int>())).Returns(contact);

            var controller = new ContactsController(_mockRepo.Object, _mockMapper.Object);

            // Act
            var result = controller.GetContact(It.IsAny<int>());

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void UpdateContact_WithIncorrectData_Return400BadRequest()
        {
            // Arrange
            var contactsManipulate = new ContactsManipulateDTO();

            var controller = new ContactsController(_mockRepo.Object, _mockMapper.Object);

            controller.ModelState.AddModelError("data", "error");

            // Act
            var result = controller.UpdateContact(It.IsAny<int>(), contactsManipulate);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void UpdateContact_WithNoData_Return404NotFound()
        {
            // Arrange
            var contactsManipulate = new ContactsManipulateDTO();
            Contacts contact = null;

            _mockRepo.Setup(x => x.GetContact(It.IsAny<int>())).Returns(contact);

            var controller = new ContactsController(_mockRepo.Object, _mockMapper.Object);

            // Act
            var result = controller.UpdateContact(It.IsAny<int>(), contactsManipulate);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void UpdateContact_WithHasData_Return204NoContent()
        {
            // Arrange
            var contactsManipulate = new ContactsManipulateDTO();
            Contacts contact = new Contacts();

            _mockRepo.Setup(x => x.GetContact(It.IsAny<int>())).Returns(contact);

            var controller = new ContactsController(_mockRepo.Object, _mockMapper.Object);

            // Act
            var result = controller.UpdateContact(It.IsAny<int>(), contactsManipulate);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void DeleteContact_WithNoData_Return404NotFound()
        {
            // Arrange
            Contacts contact = null;

            _mockRepo.Setup(x => x.GetContact(It.IsAny<int>())).Returns(contact);

            var controller = new ContactsController(_mockRepo.Object, _mockMapper.Object);

            // Act
            var result = controller.DeleteContact(It.IsAny<int>());

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void DeleteContact_WithHasData_Return204NoContent()
        {
            // Arrange
            Contacts contact = new Contacts();

            _mockRepo.Setup(x => x.GetContact(It.IsAny<int>())).Returns(contact);

            var controller = new ContactsController(_mockRepo.Object, _mockMapper.Object);

            // Act
            var result = controller.DeleteContact(It.IsAny<int>());

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void AddContacts_WithIncorrectData_Return400BadRequest()
        {
            // Arrange
            var controller = new ContactsController(_mockRepo.Object, _mockMapper.Object);

            controller.ModelState.AddModelError("data", "error");

            // Act
            var result = controller.AddContacts(It.IsAny<Contacts>());

            // Assert
            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Fact]
        public void AddContacts_WithExistData_Return400BadRequest()
        {
            // Arrange
            Contacts contact = null;

            _mockRepo.Setup(x => x.AddContact(It.IsAny<Contacts>())).Returns(contact);

            var controller = new ContactsController(_mockRepo.Object, _mockMapper.Object);

            // Act
            var result = controller.AddContacts(It.IsAny<Contacts>());

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public void AddContacts_WithNotExistedYetData_Return201Created()
        {
            // Arrange
            Contacts contact = new Contacts();

            _mockRepo.Setup(x => x.AddContact(It.IsAny<Contacts>())).Returns(contact);

            var controller = new ContactsController(_mockRepo.Object, _mockMapper.Object);

            // Act
            var result = controller.AddContacts(It.IsAny<Contacts>());

            // Assert
            Assert.IsType<CreatedAtRouteResult>(result.Result);
        }
    }
}
