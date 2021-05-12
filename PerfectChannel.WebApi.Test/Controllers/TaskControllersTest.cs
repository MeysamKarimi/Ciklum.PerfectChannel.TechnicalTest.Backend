using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using PerfectChannel.WebApi.Common;
using PerfectChannel.WebApi.Controllers;
using PerfectChannel.WebApi.Data;
using PerfectChannel.WebApi.Repositories;
using PerfectChannel.WebApi.Repositories.Interfaces;
using PerfectChannel.WebApi.Services;
using PerfectChannel.WebApi.Services.Interfaces;
using PerfectChannel.WebApi.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PerfectChannel.WebApi.Test.Controllers
{
    public class TaskControllersTest
    {
        ITaskRepository _mockRepository;
        IMapper _mockMapper;
        ITaskService _mockService;
        TaskController _mockTaskController;       

        [SetUp]
        public void Setup()
        {
            _mockRepository = GetMockTaskRepository();

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            _mockMapper = mappingConfig.CreateMapper();

            _mockService = new TaskService(_mockRepository, _mockMapper);
            _mockTaskController = new TaskController(_mockService);
        }

        [Test]
        public async Task Get_Tasks()
        {
            var AllTasks = await _mockTaskController.GetTasks();
            var okResult = AllTasks.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Test]
        public async Task Get_Task_By_Id_With_Correct_Existing_ID_Test()
        {
            var task = await _mockTaskController.GetTaskById("609aef52ebbf00e163722c03");

            var okResult = task.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual((int)HttpStatusCode.OK, okResult.StatusCode);

            Services.DTOs.Task currentTask = (Services.DTOs.Task)okResult.Value;
            Assert.AreEqual(currentTask.Id, "609aef52ebbf00e163722c03");
        }

        [Test]
        public async Task Get_Task_By_Id_With_Correct_Format_But_Not_Existing_ID_Test()
        {
            var task = await _mockTaskController.GetTaskById("609aef52e12f00e163722c10");

            var notFoundResult = task.Result as NotFoundResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }

        [Test]
        public void Get_Task_By_Id_With_InCorrect_ID_Format_Test()
        {
            Assert.That(async () => await _mockTaskController.GetTaskById("609aef52%\nf00e163722c10"), Throws.Exception.TypeOf<FormatException>());
        }


        [TestCase(Common.TaskStatus.Pending, Common.TaskStatus.Pending)]
        [TestCase(Common.TaskStatus.Completed, Common.TaskStatus.Completed)]
        public async Task Get_Task_By_Status_All_The_Output_Tasks_Must_Have_Expected_StatusType_Value_Test(Common.TaskStatus input, Common.TaskStatus expected)
        {
            var task = await _mockTaskController.GetTaskByStatus(input);

            var okResult = task.Result as OkObjectResult;
            Assert.AreEqual((int)HttpStatusCode.OK, okResult.StatusCode);

            IEnumerable<Services.DTOs.Task> tasks = (IEnumerable<Services.DTOs.Task>)okResult.Value;
            Assert.That(() => tasks.All(x => x.Status == expected));
        }


        private ITaskRepository GetMockTaskRepository()
        {
            var _taskDatabaseSettings = GetMockDatabaseSettings();
            var _context = new TaskContext(_taskDatabaseSettings);
            return new TaskRepository(_context);
        }

        private ITaskDatabaseSettings GetMockDatabaseSettings()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.test.json").Build();
            return config.GetSection(nameof(TaskDatabaseSettings)).Get<TaskDatabaseSettings>();
        }

    }

}