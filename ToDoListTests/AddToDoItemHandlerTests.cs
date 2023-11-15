using FluentAssertions;
using Moq;
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
            var mockDatabase = new Mock<IToDoDatabase>();
            var addItemDto = new AddItemDto { Task = "Test Task", IsCompleted = false };
            var expectedToDoItem = new ToDoItem { Id = Guid.NewGuid(), Task = "Test Task", IsCompleted = false };
            mockDatabase.Setup(db => db.Add(addItemDto)).Returns(expectedToDoItem);
            var handler = new AddToDoItemHandler(mockDatabase.Object);

            var request = new AddToDoItem(addItemDto);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(expectedToDoItem);
            mockDatabase.Verify(db => db.Add(addItemDto), Times.Once);
        }
    }
}
