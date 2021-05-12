﻿using System.Collections.Generic;
using System.Threading.Tasks;


namespace PerfectChannel.WebApi.Repositories.Interfaces
{
    public interface ITaskRepository
    {
        Task<IEnumerable<Data.Models.Task>> GetTasks();
        Task<Data.Models.Task> GetTask(string id);
        Task<IEnumerable<Data.Models.Task>> GetTaskByTitle(string title);
        Task<IEnumerable<Data.Models.Task>> GetTaskByStatus(Common.TaskStatus status);
    }
}