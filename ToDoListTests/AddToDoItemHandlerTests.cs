using FluentAssertions;
using Moq;
using ToDoList.DataAccess.Implementations;
using ToDoList.DataAccess.Interfaces;
using ToDoList.Models.Dtos;
using ToDoList.Models.Entities;
using ToDoList.ToDos.Commands.Handlers;
using ToDoList.ToDos.Commands.Requests;

namespace ToDoListTests
{
    public class AddToDoItemHandlerTests
    {
        [Fact]
        public async Task Handle_ValidRequest_AddsItem()
        {
            // Arrange
            var db = new InMemoryToDoDatabaseList();
            var addItemDto = new AddItemDto { Task = "Test Task", IsCompleted = false };
            var handler = new AddToDoItemHandler(db);
            var request = new AddToDoItem(addItemDto);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.Task.Should().BeEquivalentTo(addItemDto.Task);
            result.IsCompleted.Should().Be(addItemDto.IsCompleted);
        }

        [Fact]
        public async Task Handle_TwoIdenticalItems_AddsBoth()
        {
            // Arrange
            var db = new InMemoryToDoDatabaseList();
            var addItemDto = new AddItemDto { Task = "Test Task", IsCompleted = false };
            var handler = new AddToDoItemHandler(db);
            var request = new AddToDoItem(addItemDto);

            // Act
            await handler.Handle(request, CancellationToken.None);
            await handler.Handle(request, CancellationToken.None);

            // Assert
            var itemsInDb = db.GetAll();
            itemsInDb.Should().HaveCount(2);
            foreach (var item in itemsInDb)
            {
                item.Task.Should().Be(addItemDto.Task);
                item.IsCompleted.Should().Be(addItemDto.IsCompleted);
            }

        }
    }
}
