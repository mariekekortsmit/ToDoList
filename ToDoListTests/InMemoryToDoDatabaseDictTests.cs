using FluentAssertions;
using ToDoList.DataAccess.Implementations;
using ToDoList.DataAccess.Interfaces;
using ToDoList.Models.Dtos;

namespace ToDoListTests
{
    public class InMemoryToDoDatabaseDictTests : InMemoryToDoDatabaseTests<InMemoryToDoDatabaseDict> { }
    public class InMemoryToDoDatabaseListTests : InMemoryToDoDatabaseTests<InMemoryToDoDatabaseList> { }

    public abstract class InMemoryToDoDatabaseTests<T> where T: IToDoDatabase, new()
    {
        [Fact]
        public async Task GetAllAsync_NonEmptyDatabase_ShouldReturnAllItems()
        {
            // Arrange
            var database = new T();
            var item1 = await database.AddAsync(new AddItemDto { Task = "Task 1", IsCompleted = false }, CancellationToken.None);
            var item2 = await database.AddAsync(new AddItemDto { Task = "Task 2", IsCompleted = true }, CancellationToken.None);

            // Act
            var result = await database.GetAllAsync(CancellationToken.None);

            // Assert
            result.Should().HaveCount(2);
            result.Should().Contain(x => x.Id == item1.Id && x.Task == "Task 1" && x.IsCompleted == false);
            result.Should().Contain(x => x.Id == item2.Id && x.Task == "Task 2" && x.IsCompleted == true);
        }

        [Fact]
        public async Task GetAllAsync_EmptyDatabase_ShouldReturn()
        {
            // Arrange
            var database = new T();
            
            // Act
            var result = await database.GetAllAsync(CancellationToken.None);

            // Assert
            result.Should().HaveCount(0);
        }

        [Fact]
        public async Task GetAllAsync_WithCancelledToken_ThrowsTaskCanceledException()
        {
            // Arrange
            var database = new T();
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            // Act 
            Task action() => database.GetAllAsync(cancellationTokenSource.Token);

            // Assert
            await Assert.ThrowsAsync<TaskCanceledException>(action);
        }


        [Fact]
        public async Task GetAsync_ValidId_ShouldReturnItem()
        {
            // Arrange
            var database = new T();
            var newItem = await database.AddAsync(new AddItemDto { Task = "New Task", IsCompleted = false }, CancellationToken.None);

            // Act
            var result = await database.GetAsync(newItem.Id, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result?.Id.Should().Be(newItem.Id);
            result?.Task.Should().Be("New Task");
            result?.IsCompleted.Should().BeFalse();
        }

        [Fact]
        public async Task GetAsync_InvalidId_ShouldReturnNull()
        {
            // Arrange
            var database = new T();

            // Act
            var result = await database.GetAsync(Guid.NewGuid(), CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetAsync_WithCancelledToken_ThrowsTaskCanceledException()
        {
            // Arrange
            var database = new T();
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            // Act 
            Task action() => database.GetAsync(Guid.NewGuid(), cancellationTokenSource.Token);

            // Assert
            await Assert.ThrowsAsync<TaskCanceledException>(action);
        }

        [Fact]
        public async Task AddAsync_ValidItem_ShouldAddItem()
        {
            // Arrange
            var database = new T();
            var item = new AddItemDto { Task = "New Task", IsCompleted = false };

            // Act
            var result = await database.AddAsync(item, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Task.Should().Be("New Task");
            result.IsCompleted.Should().BeFalse();
            (await database.GetAllAsync(CancellationToken.None)).Should().ContainSingle(i => i.Id == result.Id);
        }

        [Fact]
        public async Task AddAsync_WithCancelledToken_DoesNotAdd_ThrowsTaskCanceledException()
        {
            // Arrange
            var database = new T();
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();
            var item = new AddItemDto { Task = "New Task", IsCompleted = false };

            // Act 
            Task action() => database.AddAsync(item, cancellationTokenSource.Token);

            // Assert
            await Assert.ThrowsAsync<TaskCanceledException>(action);
            (await database.GetAllAsync(CancellationToken.None)).Should().HaveCount(0);
        }

        [Fact]
        public async Task UpdateAsync_ValidItem_UpdatesItem()
        {
            // Arrange
            var database = new T();
            var existingItem = await database.AddAsync(new AddItemDto { Task = "Original Task", IsCompleted = false }, CancellationToken.None);
            var updatedItem = new UpdateItemDto { Task = "Updated Task", IsCompleted = true };

            // Act
            var result = await database.UpdateAsync(existingItem.Id, updatedItem, CancellationToken.None);
            var retrievedItem = await database.GetAsync(existingItem.Id, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
            retrievedItem.Should().NotBeNull();
            retrievedItem?.Task.Should().Be(updatedItem.Task);
            retrievedItem?.IsCompleted.Should().Be(updatedItem.IsCompleted.Value);
        }
        /*
         * Update_InvalidId
         * Update_ItemWithInvalidFields
         * Update_NonExistingItem
         */

        [Fact]
        public async Task UpdateAsync_ValidItem_WithCancelledToken_DoesNotUpdate_ThrowsTaskCanceledException()
        {
            // Arrange
            var database = new T();
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();
            var existingItem = await database.AddAsync(new AddItemDto { Task = "Original Task", IsCompleted = false }, CancellationToken.None);
            var updatedItem = new UpdateItemDto { Task = "Updated Task", IsCompleted = true };

            // Act & Assert
            await Assert.ThrowsAsync<TaskCanceledException>(() => database.UpdateAsync(existingItem.Id, updatedItem, cancellationTokenSource.Token));

            var retrievedItem = await database.GetAsync(existingItem.Id, CancellationToken.None);
            retrievedItem.Should().NotBeNull();
            retrievedItem?.Task.Should().Be(existingItem.Task);
            retrievedItem?.IsCompleted.Should().Be(existingItem.IsCompleted);
        }
    }
}

