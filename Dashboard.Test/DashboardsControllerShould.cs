using AutoMapper;
using Dashboard.API.Controllers;
using Dashboard.API.Entities;
using Dashboard.API.Helper;
using Dashboard.API.Models;
using Dashboard.API.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using Xunit;

namespace Dashboard.Test
{
    public class DashboardsControllerShould
    {
        private Mock<IDashboardsRepository> _mockRepo;
        private Mock<IMapper> _mockMapper;
        private Mock<IMyTools> _mockHelper;
        private Mock<HttpContext> _context;
        private Mock<IIdentity> _mockIdentity;

        public DashboardsControllerShould()
        {
            _mockRepo = new Mock<IDashboardsRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockHelper = new Mock<IMyTools>();
            _context = new Mock<HttpContext>();
            _mockIdentity = new Mock<IIdentity>();

            _context.SetupGet(x => x.User.Identity).Returns(_mockIdentity.Object);
            _mockHelper.Setup(x => x.GetUserOfRequest(It.IsAny<IEnumerable<Claim>>())).Returns("");
        }

        private IEnumerable<Dashboards> GetDashboardsFake()
        {
            return new List<Dashboards> { new Dashboards() };
        }

        [Fact]
        public void GetDashboards_WithHasData_Return200OK()
        {
            // Arrange
            _mockRepo.Setup(x => x.GetDashboards(It.IsAny<string>())).Returns(GetDashboardsFake());

            var controller = new DashboardsController(_mockRepo.Object, _mockMapper.Object, _mockHelper.Object);
            controller.ControllerContext.HttpContext = _context.Object;

            // Act
            var result = controller.GetDashboards();

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void GetDashboards_WithNoData_Return404NotFound()
        {
            // Arrange
            _mockRepo.Setup(x => x.GetDashboards(It.IsAny<string>())).Returns(new List<Dashboards>());


            var controller = new DashboardsController(_mockRepo.Object, _mockMapper.Object, _mockHelper.Object);
            controller.ControllerContext.HttpContext = _context.Object;

            // Act
            var result = controller.GetDashboards();

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public void UpdateDashboard_WithDataBindingError_Return400BadRequest()
        {
            // Arrange
            var controller = new DashboardsController(_mockRepo.Object, _mockMapper.Object, _mockHelper.Object);
            controller.ControllerContext.HttpContext = _context.Object;

            controller.ModelState.AddModelError("data", "data error");

            // Act
            var result = controller.UpdateDashboard(It.IsAny<int>(), It.IsAny<DashboardsManipulateDTO>());

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void UpdateDashboard_WithNoData_Return404NotFound()
        {
            // Arrange
            Dashboards dashboard = null;
            _mockRepo.Setup(x => x.GetDashboard(It.IsAny<int>(), It.IsAny<string>())).Returns(dashboard);

            var controller = new DashboardsController(_mockRepo.Object, _mockMapper.Object, _mockHelper.Object);
            controller.ControllerContext.HttpContext = _context.Object;

            // Act
            var result = controller.UpdateDashboard(It.IsAny<int>(), It.IsAny<DashboardsManipulateDTO>());

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void UpdateDashboard_WithIncorrectData_Return400BadRequest()
        {
            // Arrange
            _mockRepo.Setup(x => x.GetDashboard(It.IsAny<int>(), It.IsAny<string>())).Returns(new Dashboards());
            _mockRepo.Setup(x => x.UpdateDashboards(It.IsAny<Dashboards>())).Returns(false);

            var controller = new DashboardsController(_mockRepo.Object, _mockMapper.Object, _mockHelper.Object);
            controller.ControllerContext.HttpContext = _context.Object;

            // Act
            var result = controller.UpdateDashboard(It.IsAny<int>(), It.IsAny<DashboardsManipulateDTO>());

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void UpdateDashboard_Return204NoContent()
        {
            // Arrange
            _mockRepo.Setup(x => x.GetDashboard(It.IsAny<int>(), It.IsAny<string>())).Returns(new Dashboards());
            _mockRepo.Setup(x => x.UpdateDashboards(It.IsAny<Dashboards>())).Returns(true);

            var controller = new DashboardsController(_mockRepo.Object, _mockMapper.Object, _mockHelper.Object);
            controller.ControllerContext.HttpContext = _context.Object;

            // Act
            var result = controller.UpdateDashboard(It.IsAny<int>(), It.IsAny<DashboardsManipulateDTO>());

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
