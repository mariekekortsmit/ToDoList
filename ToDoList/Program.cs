
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using ToDoList.DataAccess.Implementations;
using ToDoList.DataAccess.Interfaces;
//using ToDos.Queries.Requests;
using ToDoList.Models.Dtos;
using ToDoList;
using ToDoList.ToDos.Queries.Requests;
using ToDoList.ToDos.Commands.Requests;
using ToDoList.Models.Entities;

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
            group.MapGet<GetToDoItemById, ToDoItemDto?>("/{Id}/");
            group.MapGet<GetToDoItems, List<ToDoItemDto>>("/");
            group.MapPost<AddToDoItem, ToDoItem>("/");
            group.MapPut<PutToDoItemById, bool>("/{Id}/");
            group.MapDelete<DeleteToDoItemById, bool>("/{Id}/");

            app.Run();
        }
    }
}
