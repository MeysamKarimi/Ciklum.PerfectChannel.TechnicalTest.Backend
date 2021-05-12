using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PerfectChannel.WebApi.Common;

namespace PerfectChannel.WebApi.Data.Models
{
    public class Task
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } // As I am using mongoDb, I just get this ID as a string property

        public string Title { get; set; }

        public string Description { get; set; }

        [BsonRepresentation(BsonType.String)]
        public TaskStatus Status { get; set; }
    }
}
