using AutoMapper;
using Dashboard.API.Entities;
using Dashboard.API.Helper;
using Dashboard.API.Models;
using Dashboard.API.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Dashboard.API.Controllers
{
    [Route("api/dashboards")]
    [ApiController]
    [Authorize]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class DashboardsController : ControllerBase
    {
        private readonly IDashboardsRepository _dashboardsRepository;
        private readonly IMapper _mapper;
        private readonly IMyTools _myTools;

        public DashboardsController(IDashboardsRepository dashboardsRepository, IMapper mapper, IMyTools myTools)
        {
            this._dashboardsRepository = dashboardsRepository;
            this._mapper = mapper;
            this._myTools = myTools;
        }

        /// <summary>
        /// Get list dashboard
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<DashboardsDTO>> GetDashboards()
        {
            string username = _myTools.GetUserOfRequest(User.Claims);

            var dashboards = _dashboardsRepository.GetDashboards(username);

            if (!dashboards.Any())
            {
                return NotFound("This user's dashboards could not be found");
            }

            return Ok(_mapper.Map<IEnumerable<DashboardsDTO>>(dashboards));
        }

        /// <summary>
        /// Update dasboard
        /// </summary>
        /// <param name="id">Dashboard's Id</param>
        /// <param name="dashboardsManipulate"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateDashboard(int id, [FromBody] DashboardsManipulateDTO dashboardsManipulate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            string username = _myTools.GetUserOfRequest(User.Claims);

            var dashboard = _dashboardsRepository.GetDashboard(id, username);

            if (dashboard == null)
            {
                return NotFound("This user's dashboard could not be found");
            }

            _mapper.Map(dashboardsManipulate, dashboard);

            var result = _dashboardsRepository.UpdateDashboards(dashboard);

            if (!result)
            {
                return BadRequest("LayoutId invalid");
            }

            return NoContent();
        }
    }
}
