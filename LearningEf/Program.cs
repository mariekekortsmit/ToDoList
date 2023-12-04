using LearningEF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connection = String.Empty;
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddEnvironmentVariables().AddJsonFile("appsettings.Development.json");
    connection = builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING");
}
else
{
    connection = Environment.GetEnvironmentVariable("AZURE_SQL_CONNECTIONSTRING");
}

builder.Services.AddDbContext<PersonDbContext>(options =>
    options.UseSqlServer(connection));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Person Endpoints
app.MapGet("/Person", (PersonDbContext context) =>
{
    return context.Person.Include(p => p.Addresses).ToList();
})
.WithName("GetPersons")
.WithOpenApi();

app.MapPost("/Person", (Person person, PersonDbContext context) =>
{
    context.Add(person);
    context.SaveChanges();
    return Results.Created($"/Person/{person.Id}", person);
})
.WithName("CreatePerson")
.WithOpenApi();

// Address Endpoints
app.MapGet("/Address", (PersonDbContext context) =>
{
    return context.Addresses.ToList();
})
.WithName("GetAddresses")
.WithOpenApi();

app.MapPost("/Address", (Address address, PersonDbContext context) =>
{
    // Check if PersonId is provided and valid
    if (address.PersonId != null && !context.Person.Any(p => p.Id == address.PersonId))
    {
        return Results.BadRequest("Invalid PersonId");
    }
    context.Add(address);
    context.SaveChanges();
    return Results.Created($"/Address/{address.Id}", address);
})
.WithName("CreateAddress")
.WithOpenApi();

app.Run();
