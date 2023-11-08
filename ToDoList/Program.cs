
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using ToDoList.DataAccess.Implementations;
using ToDoList.DataAccess.Interfaces;
using ToDoList.Features.Todos.Queries;
using ToDoList.Models.Dtos;
using ToDoList;


namespace ToDoList
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Register the InMemoryToDoDatabaseDict with DI container.
            // If you want to use a different database, register it here.
            builder.Services.AddSingleton<IToDoDatabase, InMemoryToDoDatabaseDict>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            var group = app
               .MapGroup("/todos")
               .WithTags("todos")               ;

            // Use the injected IToDoDatabase service
            app.MapGet("/todos/{id}", (Guid id, IToDoDatabase database) => database.Get(id));
            //app.MapGet("/todos", (IToDoDatabase database) => database.GetAll());\
            group.MapGet<GetAllToDoItemsQuery, List<ToDoItemDto>>("/");
            app.MapPost("/todos", (AddItemDto item, IToDoDatabase database) => database.Add(item));
            app.MapPut("/todos/{id}", (Guid id, UpdateItemDto item, IToDoDatabase database) => database.Update(id, item));
            app.MapDelete("/todos/{id}", (Guid id, IToDoDatabase database) => database.Delete(id));


            app.Run();
        }
    }
}
