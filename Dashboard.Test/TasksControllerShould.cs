using AutoMapper;
using Dashboard.API.Controllers;
using Dashboard.API.Entities;
using Dashboard.API.Helper;
using Dashboard.API.Models;
using Dashboard.API.Repository;
using Dashboard.API.ResourceParameters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Dashboard.Test
{
    public class TasksControllerShould
    {
        private Mock<ITasksRepository> _mockRepo;
        private Mock<IMapper> _mockMapper;
        private Mock<IMyTools> _mockHelper;
        private Mock<HttpContext> _context;
        private Mock<IIdentity> _mockIdentity;

        public TasksControllerShould()
        {
            _mockRepo = new Mock<ITasksRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockHelper = new Mock<IMyTools>();
            _context = new Mock<HttpContext>();
            _mockIdentity = new Mock<IIdentity>();

            var mockHeader = new Mock<IHeaderDictionary>();

            _context.SetupGet(x => x.User.Identity).Returns(_mockIdentity.Object);
            _context.SetupGet(x => x.Response.Headers).Returns(mockHeader.Object);
            _mockHelper.Setup(x => x.GetUserOfRequest(It.IsAny<IEnumerable<Claim>>())).Returns("");
        }

        private IEnumerable<Tasks> GetTasksFake()
        {
            return new List<Tasks> { new Tasks() };
        }

        [Fact]
        public void GetTasks_WithNoData_Return404NotFound()
        {
            // Arrange
            int count = 0;
            _mockRepo.Setup(x => x.GetTasks(It.IsAny<string>(), It.IsAny<TasksFilterDTO>(), out count)).Returns(new List<Tasks>());

            var controller = new TasksController(_mockRepo.Object, _mockMapper.Object, _mockHelper.Object);
            controller.ControllerContext.HttpContext = _context.Object;

            // Act
            var result = controller.GetTasks(It.IsAny<TasksFilterDTO>());

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public void GetTasks_WithData_Return200OK()
        {
            // Arrange
            int count = 0;
            _mockRepo.Setup(x => x.GetTasks(It.IsAny<string>(), It.IsAny<TasksFilterDTO>(), out count)).Returns(GetTasksFake());

            var tasksFilter = new TasksFilterDTO();

            var controller = new TasksController(_mockRepo.Object, _mockMapper.Object, _mockHelper.Object);
            controller.ControllerContext.HttpContext = _context.Object;

            // Act
            var result = controller.GetTasks(tasksFilter);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void AddTask_WithDataBindingError_Return400BadRequest()
        {
            // Arrange
            var tasksCreate = new TasksCreateDTO();

            var controller = new TasksController(_mockRepo.Object, _mockMapper.Object, _mockHelper.Object);
            controller.ControllerContext.HttpContext = _context.Object;

            controller.ModelState.AddModelError("data", "error");

            // Act
            var result = controller.AddTask(tasksCreate);

            // Assert
            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Fact]
        public void AddTask_WithIncorrectData_Return400BadRequest()
        {
            // Arrange
            var tasksCreate = new TasksCreateDTO();
            Tasks tast = null;

            _mockRepo.Setup(x => x.AddTask(It.IsAny<Tasks>())).Returns(tast);
            _mockMapper.Setup(x => x.Map<Tasks>(It.IsAny<TasksCreateDTO>())).Returns(new Tasks());

            var controller = new TasksController(_mockRepo.Object, _mockMapper.Object, _mockHelper.Object);
            controller.ControllerContext.HttpContext = _context.Object;

            // Act
            var result = controller.AddTask(tasksCreate);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public void AddTask_WithCorrectData_Return201Created()
        {
            // Arrange
            var tasksCreate = new TasksCreateDTO();
            Tasks tast = new Tasks();

            _mockRepo.Setup(x => x.AddTask(It.IsAny<Tasks>())).Returns(tast);
            _mockMapper.Setup(x => x.Map<Tasks>(It.IsAny<TasksCreateDTO>())).Returns(new Tasks());

            var controller = new TasksController(_mockRepo.Object, _mockMapper.Object, _mockHelper.Object);
            controller.ControllerContext.HttpContext = _context.Object;

            // Act
            var result = controller.AddTask(tasksCreate);

            // Assert
            Assert.IsType<CreatedAtRouteResult>(result.Result);
        }

        [Fact]
        public void GetTask_WithNoData_Return404NotFound()
        {
            // Arrange
            Tasks tast = null;

            _mockRepo.Setup(x => x.GetTask(It.IsAny<int>(), It.IsAny<string>())).Returns(tast);

            var controller = new TasksController(_mockRepo.Object, _mockMapper.Object, _mockHelper.Object);
            controller.ControllerContext.HttpContext = _context.Object;

            // Act
            var result = controller.GetTask(It.IsAny<int>());

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public void GetTask_WithHasData_Return200OK()
        {
            // Arrange
            Tasks tast = new Tasks();

            _mockRepo.Setup(x => x.GetTask(It.IsAny<int>(), It.IsAny<string>())).Returns(tast);

            var controller = new TasksController(_mockRepo.Object, _mockMapper.Object, _mockHelper.Object);
            controller.ControllerContext.HttpContext = _context.Object;

            // Act
            var result = controller.GetTask(It.IsAny<int>());

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }


        [Fact]
        public void DeleteTask_WithNoData_Return404NotFound()
        {
            // Arrange
            Tasks tast = null;

            _mockRepo.Setup(x => x.GetTask(It.IsAny<int>(), It.IsAny<string>())).Returns(tast);

            var controller = new TasksController(_mockRepo.Object, _mockMapper.Object, _mockHelper.Object);
            controller.ControllerContext.HttpContext = _context.Object;

            // Act
            var result = controller.DeleteTask(It.IsAny<int>());

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void DeleteTask_WithHasData_Return204NoContent()
        {
            // Arrange
            Tasks tast = new Tasks();

            _mockRepo.Setup(x => x.GetTask(It.IsAny<int>(), It.IsAny<string>())).Returns(tast);

            var controller = new TasksController(_mockRepo.Object, _mockMapper.Object, _mockHelper.Object);
            controller.ControllerContext.HttpContext = _context.Object;

            // Act
            var result = controller.DeleteTask(It.IsAny<int>());

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void UpdateTask_WithIncorrectData_Return400BadRequest()
        {
            // Arrange
            var tasksManipulate = new TasksManipulateDTO();

            var controller = new TasksController(_mockRepo.Object, _mockMapper.Object, _mockHelper.Object);
            controller.ControllerContext.HttpContext = _context.Object;

            controller.ModelState.AddModelError("data", "error");

            // Act
            var result = controller.UpdateTask(It.IsAny<int>(), tasksManipulate);

            // Assert
            Assert.IsType<BadRequestResult>(result);

        }

        [Fact]
        public void UpdateTask_WithNoData_Return404NotFound()
        {
            // Arrange
            var tasksManipulate = new TasksManipulateDTO();
            Tasks task = null;

            _mockRepo.Setup(x => x.GetTask(It.IsAny<int>(), It.IsAny<string>())).Returns(task);

            var controller = new TasksController(_mockRepo.Object, _mockMapper.Object, _mockHelper.Object);
            controller.ControllerContext.HttpContext = _context.Object;

            // Act
            var result = controller.UpdateTask(It.IsAny<int>(), tasksManipulate);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);

        }

        [Fact]
        public void UpdateTask_WithHasData_Return204NoContent()
        {
            // Arrange
            var tasksManipulate = new TasksManipulateDTO();
            Tasks task = new Tasks();

            _mockRepo.Setup(x => x.GetTask(It.IsAny<int>(), It.IsAny<string>())).Returns(task);
            _mockMapper.Setup(x => x.Map(It.IsAny<TasksManipulateDTO>(), It.IsAny<Tasks>())).Returns(new Tasks());

            var controller = new TasksController(_mockRepo.Object, _mockMapper.Object, _mockHelper.Object);
            controller.ControllerContext.HttpContext = _context.Object;

            // Act
            var result = controller.UpdateTask(It.IsAny<int>(), tasksManipulate);

            // Assert
            Assert.IsType<NoContentResult>(result);

        }
    }
}