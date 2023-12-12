using System.Reflection;
using ToDoList.DataAccess.Implementations;
using ToDoList.DataAccess.Interfaces;
using ToDoList.Models.Dtos;
using ToDoList.ToDos.Queries.Requests;
using ToDoList.ToDos.Commands.Requests;
using ToDoList.People.Queries.Requests;
using ToDoList.People.Commands.Requests;
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

            var todoGroup = app
               .MapGroup("/todos")
               .WithTags("todos")               ;

            todoGroup.MapGet<GetToDoItemById, ToDoItemDto?>("/{Id}/");
            todoGroup.MapGet<GetToDoItems, List<ToDoItemDto>>("/");
            todoGroup.MapPost<AddToDoItem, ToDoItem>("/");
            todoGroup.MapPut<PutToDoItemById, bool>("/{Id}/");
            todoGroup.MapDelete<DeleteToDoItemById, bool>("/{Id}/");
            todoGroup.MapGet<GetToDoItemsByPerson, List<ToDoItemDto>>("/people/{personId}/"); // TODO in ef dict and list

            var personGroup = app
              .MapGroup("/people")
              .WithTags("people");

            personGroup.MapGet<GetPersonById, PersonDto?>("/{Id}/");
            personGroup.MapGet<GetPeople, List<PersonDto>>("/");
            personGroup.MapPost<AddPerson, Person>("/");
            personGroup.MapPut<PutPersonById, bool>("/{Id}/");
            personGroup.MapDelete<DeletePersonById, bool>("/{Id}/");
            personGroup.MapPost<AddPersonToToDoItem, bool>("/{personId}/todos/{itemId}"); // TODO
            personGroup.MapDelete<DeletePersonFromToDoItem, bool>("/{personId}/todos/{itemId}"); // TODO
            personGroup.MapGet<GetPeopleByToDoItem, List<PersonDto>>("/todos/{itemId}"); // TODO

            app.Run();
        }
    }
}
