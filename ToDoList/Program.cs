using System.Reflection;
using ToDoList.DataAccess.Implementations;
using ToDoList.DataAccess.Interfaces;
using ToDoList.Models.Dtos;
using ToDoList.ToDos.Queries.Requests;
using ToDoList.ToDos.Commands.Requests;
using ToDoList.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ToDoList
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Load environment variables and JSON files into configuration for all environments
            builder.Configuration.AddEnvironmentVariables();

            if(builder.Environment.IsDevelopment())
            {
                // Additional configuration for Development environment
                builder.Configuration.AddJsonFile("appsettings.Development.json");
            }

            // Rest of the database setup
            if(builder.Environment.IsEnvironment("Testing"))
            {
                // Register the InMemoryToDoDatabaseDict with DI container.
                // If you want to use a different database, register it here.
                builder.Services.AddSingleton<IToDoDatabase, InMemoryToDoDatabaseDict>();
            }
            else
            {
                // Get connection string from the configuration
                var connectionString = builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING")
                    ?? throw new InvalidOperationException("Azure SQL connection string not found");

                // Use SQL database for other environments
                builder.Services.AddDbContext<ToDoDbContext>(options =>
                    options.UseSqlServer(connectionString));
                builder.Services.AddScoped<IToDoDatabase, EFCoreToDoDatabase>();
            }

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            var group = app
               .MapGroup("/todos")
               .WithTags("todos")               ;

            // Use the injected IToDoDatabase service
            group.MapGet<GetToDoItemById, ToDoItemDto?>("/{Id}/");
            group.MapGet<GetToDoItems, List<ToDoItemDto>>("/");
            group.MapPost<AddToDoItem, ToDoItem>("/");
            group.MapPut<PutToDoItemById, bool>("/{Id}/");
            group.MapDelete<DeleteToDoItemById, bool>("/{Id}/");

            app.Run();
        }
    }
}
