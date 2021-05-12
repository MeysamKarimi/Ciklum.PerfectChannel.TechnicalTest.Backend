using PerfectChannel.WebApi.Data.Interfaces;
using MongoDB.Driver;
using PerfectChannel.WebApi.Settings;
using PerfectChannel.WebApi.Data.Models;

namespace PerfectChannel.WebApi.Data
{
    public class TaskContext : ITaskContext
    {
        public IMongoCollection<Task> Tasks { get; }

        public TaskContext(ITaskDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            Tasks = database.GetCollection<Task>(settings.CollectionName);

            //TaskContextSeed.SeedData(Tasks);

        }
    }
}
