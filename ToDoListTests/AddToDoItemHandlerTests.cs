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
            var itemsInDb = await db.GetAllAsync(CancellationToken.None);
            itemsInDb.Should().HaveCount(2);
            foreach (var item in itemsInDb)
            {
                item.Task.Should().Be(addItemDto.Task);
                item.IsCompleted.Should().Be(addItemDto.IsCompleted);
            }

        }

        [Fact]
        public async Task Handle_TwoDifferentItems_AddsBoth()
        {
            // Arrange
            var db = new InMemoryToDoDatabaseList();
            var addItemDto1 = new AddItemDto { Task = "Test Task 1", IsCompleted = false };
            var addItemDto2 = new AddItemDto { Task = "Test Task 2", IsCompleted = true };
            var handler = new AddToDoItemHandler(db);
            var request1 = new AddToDoItem(addItemDto1);
            var request2 = new AddToDoItem(addItemDto2);

            // Act
            var result1 = await handler.Handle(request1, CancellationToken.None);
            var result2 = await handler.Handle(request2, CancellationToken.None);

            // Assert
            var itemsInDb =  await db.GetAllAsync(CancellationToken.None);
            itemsInDb.Should().HaveCount(2);
            result1.Task.Should().Be(addItemDto1.Task);
            result1.IsCompleted.Should().Be(addItemDto1.IsCompleted);
            result2.Task.Should().Be(addItemDto2.Task);
            result2.IsCompleted.Should().Be(addItemDto2.IsCompleted);
        }
    }
}
