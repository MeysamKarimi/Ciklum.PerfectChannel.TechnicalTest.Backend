using AutoMapper;
using MongoDB.Driver;
using PerfectChannel.WebApi.Data.Interfaces;
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
    }
}
