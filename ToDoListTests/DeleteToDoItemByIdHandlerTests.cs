using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.DataAccess.Implementations;
using ToDoList.DataAccess.Interfaces;
using ToDoList.Models.Dtos;
using ToDoList.ToDos.Commands.Handlers;
using ToDoList.ToDos.Commands.Requests;

namespace ToDoListTests
{
    public class DeleteToDoItemByIdHandlerTests
    {
        [Fact]
        public async Task Handle_ValidId_DeletesItem()
        {
            // Arrange
            var db = new InMemoryToDoDatabaseList();
            var addItemDto = new AddItemDto { Task = "Test Task", IsCompleted = false };
            var addedId = (await db.AddToDoItemAsync(addItemDto, CancellationToken.None)).Id;
            var handler = new DeleteToDoItemByIdHandler(db);
            var request = new DeleteToDoItemById(addedId);

            // Act
            await handler.Handle(request, CancellationToken.None);

            // Assert
            var itemsInDb = await db.GetAllToDoItemsAsync(CancellationToken.None);
            itemsInDb.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_InvalidId_ReturnsFalse()
        {
            // Arrange
            var db = new InMemoryToDoDatabaseList();
            var handler = new DeleteToDoItemByIdHandler(db);
            Guid invalidId = new ();
            var request = new DeleteToDoItemById(invalidId);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_ValidId_WithCancelledToken_DoesNotDelete_ThrowsTaskCanceledException() 
        {
            // Arrange
            var db = new InMemoryToDoDatabaseList();
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();
            var addItemDto = new AddItemDto { Task = "Test Task", IsCompleted = false };
            var addedId = (await db.AddToDoItemAsync(addItemDto, CancellationToken.None)).Id;
            var handler = new DeleteToDoItemByIdHandler(db);
            var request = new DeleteToDoItemById(addedId);

            // Act
            Task action() => handler.Handle(request, cancellationTokenSource.Token);

            // Assert
            await Assert.ThrowsAsync<TaskCanceledException>(action);
            var itemsInDb = await db.GetAllToDoItemsAsync(CancellationToken.None);
            itemsInDb.Should().HaveCount(1);
        }
    }
}
