using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PerfectChannel.WebApi.Services.Interfaces
{
    public interface ITaskService
    {
        Task<IEnumerable<DTOs.Task>> GetTasks();
        Task<DTOs.Task> GetTask(string id);
        Task<IEnumerable<DTOs.Task>> GetTaskByTitle(string title);
        Task<IEnumerable<DTOs.Task>> GetTaskByStatus(Common.TaskStatus status);
    }
}
