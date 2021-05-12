using MongoDB.Driver;
using PerfectChannel.WebApi.Data.Models;

namespace PerfectChannel.WebApi.Data.Interfaces
{
    public interface ITaskContext
    {
        public IMongoCollection<Task> Tasks { get; }
    }
}
