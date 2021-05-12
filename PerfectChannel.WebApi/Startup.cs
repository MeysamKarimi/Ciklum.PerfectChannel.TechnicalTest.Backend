using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using PerfectChannel.WebApi.Common;
using PerfectChannel.WebApi.Data;
using PerfectChannel.WebApi.Data.Interfaces;
using PerfectChannel.WebApi.Repositories;
using PerfectChannel.WebApi.Repositories.Interfaces;
using PerfectChannel.WebApi.Services;
using PerfectChannel.WebApi.Services.Interfaces;
using PerfectChannel.WebApi.Settings;

namespace PerfectChannel.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.Configure<TaskDatabaseSettings>(Configuration.GetSection(nameof(TaskDatabaseSettings)));

            services.AddSingleton<ITaskDatabaseSettings>(ts => ts.GetRequiredService<IOptions<TaskDatabaseSettings>>().Value);

            services.AddTransient<ITaskContext, TaskContext>();

            services.AddTransient<ITaskRepository, TaskRepository>();

            services.AddTransient<ITaskService, TaskService>();


            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddSwaggerGen(doc => {
                doc.SwaggerDoc("v1", new OpenApiInfo { Title = "To-do list API", Version = "v1" });
                doc.CustomSchemaIds(type => type.ToString());
            });

            ConfigureCors(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "To-do list API v1");
            });
        }

        private void ConfigureCors(IServiceCollection services)
        {
            var frontendServerUrl = Configuration.GetValue<string>("FrontendServerUrl");
            services.AddCors(options =>
                options.AddPolicy("AllowOrigin", builder =>
                    builder.WithOrigins(frontendServerUrl)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                )
            );
        }
    }
}
