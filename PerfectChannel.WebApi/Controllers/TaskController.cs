using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        readonly ILogger<TaskController> _logger;

        public TaskController(ITaskService service, ILogger<TaskController> logger)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Services.DTOs.Task>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Services.DTOs.Task>>> GetTasks()
        {
            _logger.LogInformation("GetTasks is invoked!");
            var tasks = await _service.GetTasks();           
            return Ok(tasks);
        }

        [HttpGet("{id:length(24)}", Name = "GetTask")]
        [ProducesResponseType(typeof(IEnumerable<Services.DTOs.Task>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<Services.DTOs.Task>> GetTaskById(string id)
        {
            _logger.LogInformation($"GetTaskById with id: {id}, is invoked!");
            var task = await _service.GetTask(id);
            if (task == null)
            {
                _logger.LogError($"GetTaskById with Id: {id}, BadRequest!");
                return NotFound();
            }
            _logger.LogInformation($"GetTaskById with Id: {id}, returns value!");
            return Ok(task);
        }

        [Route("[action]/{status}")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Services.DTOs.Task>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Services.DTOs.Task>>> GetTaskByStatus(Common.TaskStatus status)
        {
            _logger.LogInformation($"GetTaskByStatus with status: {status}, is invoked!");
            var tasks = await _service.GetTaskByStatus(status);
            return Ok(tasks);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Services.DTOs.Task), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<Services.DTOs.Task>> CreateTask([FromBody] Services.DTOs.Task task)
        {     
            _logger.LogInformation($"CreateTask is invoked!");

            var creatResult = await _service.Create(task);

            if (creatResult.ResponseCode == (int)Common.ResponseCode.Success)
            {
                task.Id = creatResult.Data;

                _logger.LogInformation($"Task with Id: {task.Id}, is created!");
                
                return CreatedAtRoute("GetTask", new { id = task.Id }, task);
            }

            _logger.LogError($"Task could not be created!");
            return BadRequest();
        }

        [Route("[action]/{id}")]
        [HttpPut]
        [ProducesResponseType(typeof(Services.DTOs.Task), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ToggleTaskStatus(string id)
        {
            _logger.LogInformation($"ToggleTaskStatus with Id: {id}, is invoked!");
            var updateResult = await _service.ToggleTaskStatus(id);

            if (updateResult.ResponseCode == (int)Common.ResponseCode.Success)
            {
                _logger.LogInformation($"TaskStatus with Id: {id}, is toggled!");
                return Ok(true);
            }

            _logger.LogError($"TaskStatus with Id: {id}, could not be toggled!");
            return BadRequest();
        }

        [HttpPut]
        [ProducesResponseType(typeof(Services.DTOs.Task), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateTask([FromBody] Services.DTOs.Task task)
        {
            var updateResult = await _service.Update(task);

            if (updateResult.ResponseCode == (int)Common.ResponseCode.Success)
                return Ok(true);
         
            return BadRequest();
        }

        [HttpDelete("{id:length(24)}")]
        [ProducesResponseType(typeof(Services.DTOs.Task), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteTask(string id)
        {
            var deleteResult = await _service.Delete(id);

            if (deleteResult.ResponseCode == (int)Common.ResponseCode.Success)
                return Ok(true);

            return BadRequest();
        }
    }
}