namespace PerfectChannel.WebApi.Settings
{
    public interface ITaskDatabaseSettings
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
        string CollectionName { get; set; }
    }
}
