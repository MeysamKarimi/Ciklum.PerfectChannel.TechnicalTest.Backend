using System.Collections.Generic;
using System.Threading.Tasks;


namespace PerfectChannel.WebApi.Repositories.Interfaces
{
    public interface ITaskRepository
    {
        Task<IEnumerable<Data.Models.Task>> GetTasks();
        Task<Data.Models.Task> GetTask(string id);
        Task<IEnumerable<Data.Models.Task>> GetTaskByTitle(string title);
        Task<IEnumerable<Data.Models.Task>> GetTaskByStatus(Common.TaskStatus status);
        Task<Common.Response> Create(Data.Models.Task task);
        Task<bool> ToggleTaskStatus(string Id);
        Task<bool> Update(Data.Models.Task task);
        Task<bool> Delete(string Id);
    }
}
