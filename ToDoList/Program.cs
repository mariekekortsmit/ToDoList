
using Microsoft.AspNetCore.Builder;

namespace ToDoList
{
    public class Program
    {
        // Define the database as a static member
        private static readonly InMemoryToDoDatabase _database = new();

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();          

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            // Directly use the static _database member
            app.MapGet("/todos/{id}", (int id) => _database.Get(id));
            app.MapGet("/todos", () => _database.GetAll());
            app.MapPost("/todos", (AddItemDto item) => _database.Add(item));
            app.MapPut("/todos/{id}", (int id,  UpdateItemDto item) => _database.Update(id, item));
            app.MapDelete("/todos/{id}", (int id) => _database.Delete(id));

            app.Run();
        }
    }
}