using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PerfectChannel.WebApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace PerfectChannel.WebApi.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowOrigin")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        readonly ITaskService _service;

        public TaskController(ITaskService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Services.DTOs.Task>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Services.DTOs.Task>>> GetTasks()
        {
            var tasks = await _service.GetTasks();
            return Ok(tasks);
        }

        [HttpGet("{id:length(24)}", Name = "GetTask")]
        [ProducesResponseType(typeof(IEnumerable<Services.DTOs.Task>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<Services.DTOs.Task>> GetTaskById(string id)
        {
            var task = await _service.GetTask(id);
            if (task == null)
            {
                return NotFound();
            }
            return Ok(task);
        }

        [Route("[action]/{status}")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Services.DTOs.Task>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Services.DTOs.Task>>> GetTaskByStatus(Common.TaskStatus status)
        {
            var tasks = await _service.GetTaskByStatus(status);
            return Ok(tasks);
        }
    }
}