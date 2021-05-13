using MongoDB.Driver;
using PerfectChannel.WebApi.Data.Interfaces;
using PerfectChannel.WebApi.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PerfectChannel.WebApi.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        readonly ITaskContext _context;

        public TaskRepository(ITaskContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Data.Models.Task>> GetTasks()
        {
            return await _context
                            .Tasks
                            .Find(p => true)
                            .ToListAsync();
        }

        public async Task<Data.Models.Task> GetTask(string id)
        {
            return await _context
                                .Tasks
                                .Find(p => p.Id == id)
                                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Data.Models.Task>> GetTaskByTitle(string TaskTitle)
        {
            FilterDefinition<Data.Models.Task> filter = Builders<Data.Models.Task>.Filter.ElemMatch(p => p.Title, TaskTitle);

            return await _context
                            .Tasks
                            .Find(filter)
                            .ToListAsync();
        }

        public async Task<IEnumerable<Data.Models.Task>> GetTaskByStatus(Common.TaskStatus status)
        {
            FilterDefinition<Data.Models.Task> filter = Builders<Data.Models.Task>.Filter.Eq(p => p.Status, status);

            return await _context
                            .Tasks
                            .Find(filter)
                            .ToListAsync();
        }

        public async Task<Common.Response> Create(Data.Models.Task task)
        {
            await _context
                    .Tasks
                    .InsertOneAsync(task);
            return new Common.Response()
            {
                ResponseCode = (int)Common.ResponseCode.Success,
                Data = task.Id
            };
        }
        public async Task<bool> Update(Data.Models.Task task)
        {

            var updateResult = await _context.Tasks.ReplaceOneAsync(filter: old => old.Id == task.Id, replacement: task);

            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }
        public async Task<bool> Delete(string Id)
        {
            var deleteResult = await _context.Tasks.DeleteOneAsync(p => p.Id == Id);

            return deleteResult.IsAcknowledged
                    && deleteResult.DeletedCount > 0;
        }
    }
}

