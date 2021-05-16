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

            #region Some notes with respect to user stories
            // We can make the Task.Title + Task.Description as an unique index in MongoDb to prevent a task with a same <Title, Description> set.
            // As it is not an active option in user stories, I ignored this.
            //Tasks.Indexes.CreateOne(new CreateIndexModel<Task>(new IndexKeysDefinitionBuilder<Task>().Ascending(x => x.Title).Ascending(x => x.Description), 
            //    new CreateIndexOptions { Unique = true }));

            //We also can seed data for the collection but as I didn't, as it mentioned in User Story 1:  * Initially, to-do list will be empty.
            //TaskContextSeed.SeedData(Tasks);
            #endregion            
        }
    }
}
