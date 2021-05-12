namespace PerfectChannel.WebApi.Settings
{
    public class TaskDatabaseSettings : ITaskDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string CollectionName { get; set; }
    }
}
