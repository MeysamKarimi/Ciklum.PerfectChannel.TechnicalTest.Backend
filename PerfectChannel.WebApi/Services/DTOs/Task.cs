namespace PerfectChannel.WebApi.Services.DTOs
{
    public class Task
    {     
        public string Id { get; set; } // As I am using mongoDb, I just get this ID as a string property

        public string Title { get; set; }

        public string Description { get; set; }

        public Common.TaskStatus Status { get; set; }
    }
}
