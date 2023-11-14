﻿using MediatR;
using ToDoList.DataAccess.Interfaces;
using ToDoList.ToDos.Commands.Requests;
using ToDoList.Models.Dtos;
using ToDoList.Models.Entities;

namespace ToDoList.ToDos.Commands.Handlers
{
    public class AddToDoItemHandler : IRequestHandler<AddToDoItem, ToDoItem>
    {
        private readonly IToDoDatabase _database;

        public AddToDoItemHandler(IToDoDatabase database)
        {
            _database = database;
        }

        public Task<ToDoItem> Handle(AddToDoItem request, CancellationToken cancellationToken)
        {
            var newItem = _database.Add(request.Item);
            return Task.FromResult(newItem);
        }

    }
}