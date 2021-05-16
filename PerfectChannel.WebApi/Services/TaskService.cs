using AutoMapper;
using PerfectChannel.WebApi.Repositories.Interfaces;
using PerfectChannel.WebApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PerfectChannel.WebApi.Services
{
    public class TaskService : ITaskService
    {
        readonly ITaskRepository _repository;
        private readonly IMapper _mapper;

        public TaskService(ITaskRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<DTOs.Task>> GetTasks()
        {
            var tasks = await _repository.GetTasks();
            return _mapper.Map<IEnumerable<DTOs.Task>>(tasks);
        }

        public async Task<DTOs.Task> GetTask(string id)
        {
            var task = await _repository.GetTask(id);
            return _mapper.Map<DTOs.Task>(task);
        }

        public async Task<IEnumerable<DTOs.Task>> GetTaskByTitle(string TaskTitle)
        {
            var tasks = await _repository.GetTaskByTitle(TaskTitle);
            return _mapper.Map<IEnumerable<DTOs.Task>>(tasks);
        }

        public async Task<IEnumerable<DTOs.Task>> GetTaskByStatus(Common.TaskStatus status)
        {
            var tasks = await _repository.GetTaskByStatus(status);
            return _mapper.Map<IEnumerable<DTOs.Task>>(tasks);
        }

        public async Task<Common.Response> Create(DTOs.Task task)
        {            
            if (task == null || string.IsNullOrWhiteSpace(task.Title))
            {
                return new Common.Response()
                {
                    ResponseCode = (int)Common.ResponseCode.ErrorValidation,
                    Data = "Invalid Input Task"
                };
            }

            task.Status = Common.TaskStatus.Pending;
            Data.Models.Task taskModel = _mapper.Map<Data.Models.Task>(task);
            return await _repository.Create(taskModel);
        }


        public async Task<Common.Response> ToggleTaskStatus(string Id)
        {
            if (string.IsNullOrWhiteSpace(Id))
            {
                return new Common.Response()
                {
                    ResponseCode = (int)Common.ResponseCode.ErrorValidation,
                    Data = "Invalid Input Task"
                };
            }
           
            try
            {
                await _repository.ToggleTaskStatus(Id);
                return new Common.Response()
                {
                    ResponseCode = (int)Common.ResponseCode.Success,
                    Data = "Task Updated"
                };

            }
            catch (Exception exception)
            {
                return new Common.Response()
                {
                    ResponseCode = (int)Common.ResponseCode.ErrorValidation,
                    Data = exception.Message
                };
            }
        }

        public async Task<Common.Response> Update(DTOs.Task task)
        {
            if (task == null || string.IsNullOrWhiteSpace(task.Id) || string.IsNullOrWhiteSpace(task.Title))
            {
                return new Common.Response()
                {
                    ResponseCode = (int)Common.ResponseCode.ErrorValidation,
                    Data = "Invalid Input Task"
                };
            }

            Data.Models.Task taskModel = _mapper.Map<Data.Models.Task>(task);
            try
            {
                await _repository.Update(taskModel);
                return new Common.Response()
                {
                    ResponseCode = (int)Common.ResponseCode.Success,
                    Data = "Task Updated"
                };

            }
            catch (Exception exception)
            {
                return new Common.Response()
                {
                    ResponseCode = (int)Common.ResponseCode.ErrorValidation,
                    Data = exception.Message
                };
            }
        }
        public async Task<Common.Response> Delete(string Id)
        {
            if (string.IsNullOrWhiteSpace(Id))
            {
                return new Common.Response()
                {
                    ResponseCode = (int)Common.ResponseCode.ErrorValidation,
                    Data = "Invalid Input Task"
                };
            }
            try
            {
                await _repository.Delete(Id);
                return new Common.Response()
                {
                    ResponseCode = (int)Common.ResponseCode.Success,
                    Data = "Task Deleted"
                };
            }
            catch (Exception exception)
            {
                return new Common.Response()
                {
                    ResponseCode = (int)Common.ResponseCode.ErrorValidation,
                    Data = exception.Message
                };
            }
        }
    }
}
