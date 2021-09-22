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
using System.Linq;

namespace Dashboard.API.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    [Authorize]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class TasksController : ControllerBase
    {
        private readonly ITasksRepository _tasksRepository;
        private readonly IMapper _mapper;
        private readonly IMyTools _myTools;

        public TasksController(ITasksRepository tasksRepository, IMapper mapper, IMyTools myTools)
        {
            this._tasksRepository = tasksRepository;
            this._mapper = mapper;
            this._myTools = myTools;
        }

        /// <summary>
        /// Get list tasks
        /// </summary>
        /// <param name="tasksFilter"></param>
        [HttpGet(Name = "GetTasks")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<TasksDTO>> GetTasks([FromQuery] TasksFilterDTO tasksFilter)
        {
            var username = _myTools.GetUserOfRequest(User.Claims);

            var tasks = _tasksRepository.GetTasks(username, tasksFilter, out int count);

            if (!tasks.Any())
            {
                return NotFound("Not found any task of this user");
            }

            var pageResponse = CreateResourceUri(tasksFilter, count);

            Response.Headers.Add("X-Paging", JsonConvert.SerializeObject(pageResponse));


            return Ok(_mapper.Map<IEnumerable<TasksDTO>>(tasks));
        }

        /// <summary>
        /// Create new a task
        /// </summary>
        /// <param name="task"></param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public ActionResult<TasksDTO> AddTask([FromBody] TasksCreateDTO task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var username = _myTools.GetUserOfRequest(User.Claims);

            var taskToAdd = _mapper.Map<Tasks>(task);
            taskToAdd.Username = username;

            taskToAdd = _tasksRepository.AddTask(taskToAdd);

            if (taskToAdd == null)
            {
                return BadRequest("Invalid widgetId");
            }

            return CreatedAtRoute("GetTask", new { id = taskToAdd.TasksId }, _mapper.Map<TasksDTO>(taskToAdd));
        }

        /// <summary>
        /// Get specific task
        /// </summary>
        /// <param name="id">Task's Id</param>
        [HttpGet("{id}", Name = "GetTask")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<TasksDTO> GetTask(int id)
        {
            var username = _myTools.GetUserOfRequest(User.Claims);

            var task = _tasksRepository.GetTask(id, username);

            if (task == null)
            {
                return NotFound("This user's task could not be found");
            }

            return Ok(_mapper.Map<TasksDTO>(task));
        }

        /// <summary>
        /// Delete specific task
        /// </summary>
        /// <param name="id">Task's Id</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult DeleteTask(int id)
        {
            var username = _myTools.GetUserOfRequest(User.Claims);
            var task = _tasksRepository.GetTask(id, username);

            if (task == null)
            {
                return NotFound("This user's task could not be found");
            }

            _tasksRepository.DeleteTask(task);
            return NoContent();
        }

        /// <summary>
        /// Update specific task
        /// </summary>
        /// <param name="id">Task's Id</param>
        /// <param name="task"></param>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult UpdateTask(int id, [FromBody] TasksManipulateDTO task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var username = _myTools.GetUserOfRequest(User.Claims);
            var taskToUpdate = _tasksRepository.GetTask(id, username);

            if (taskToUpdate == null)
            {
                return NotFound("This task was not found");
            }

            _mapper.Map(task, taskToUpdate);
            _tasksRepository.UpdateTask(taskToUpdate);

            return NoContent();
        }

        private PageResponse CreateResourceUri(TasksFilterDTO tasksFilter, int count)
        {
            var currentPage = tasksFilter.PageNumber;
            var pageSize = tasksFilter.PageSize;
            var totalCount = count;
            var totalPage = (int)Math.Ceiling(1.0 * totalCount / pageSize);

            var pPre = currentPage - 1;
            var pNext = currentPage + 1;

            string pagePrevious = null;
            string pageNext = null;

            if (currentPage > 1)
            {
                pagePrevious = Url.Link("GetTasks", new TasksFilterDTO
                {
                    TaskTitle = tasksFilter.TaskTitle,
                    IsCompleted = tasksFilter.IsCompleted,
                    WidgetsId = tasksFilter.WidgetsId,
                    PageNumber = pPre,
                    PageSize = tasksFilter.PageSize
                });
            }

            if (currentPage < totalPage)
            {
                pageNext = Url.Link("GetTasks", new TasksFilterDTO
                {
                    TaskTitle = tasksFilter.TaskTitle,
                    IsCompleted = tasksFilter.IsCompleted,
                    WidgetsId = tasksFilter.WidgetsId,
                    PageNumber = pNext,
                    PageSize = tasksFilter.PageSize
                });
            }
            return new PageResponse(totalPage, currentPage, pageSize, totalCount, pagePrevious, pageNext);
        }
    }
}
