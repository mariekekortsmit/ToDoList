using FluentAssertions;
using ToDoList.DataAccess.Implementations;
using ToDoList.Models.Dtos;

namespace ToDoListTests
{
    public class InMemoryToDoDatabaseDictTests
    {
        [Fact]
        public void GetAll_NonEmptyDatabase_ShouldReturnAllItems()
        {
            // Arrange
            var database = new InMemoryToDoDatabaseDict();
            var item1 = database.Add(new AddItemDto { Task = "Task 1", IsCompleted = false });
            var item2 = database.Add(new AddItemDto { Task = "Task 2", IsCompleted = true });

            // Act
            var result = database.GetAll();

            // Assert
            result.Should().HaveCount(2);
            result.Should().Contain(x => x.Id == item1.Id && x.Task == "Task 1" && x.IsCompleted == false);
            result.Should().Contain(x => x.Id == item2.Id && x.Task == "Task 2" && x.IsCompleted == true);
        }

        [Fact]
        public void GetAll_EmptyDatabase_ShouldReturn()
        {
            // Arrange
            var database = new InMemoryToDoDatabaseDict();
            
            // Act
            var result = database.GetAll();

            // Assert
            result.Should().HaveCount(0);
            //Maybe some other asserts?
        }

        [Fact]
        public void Get_ValidId_ShouldReturnItem()
        {
            // Arrange
            var database = new InMemoryToDoDatabaseDict();
            var newItem = database.Add(new AddItemDto { Task = "New Task", IsCompleted = false });

            // Act
            var result = database.Get(newItem.Id);

            // Assert
            result.Should().NotBeNull();
            result?.Id.Should().Be(newItem.Id);
            result?.Task.Should().Be("New Task");
            result?.IsCompleted.Should().BeFalse();
        }

        [Fact]
        public void Get_InvalidId_ShouldReturnNull()
        {
            // Arrange
            var database = new InMemoryToDoDatabaseDict();

            // Act
            var result = database.Get(Guid.NewGuid());

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void Add_ValidItem_ShouldAddItem()
        {
            // Arrange
            var database = new InMemoryToDoDatabaseDict();
            var item = new AddItemDto { Task = "New Task", IsCompleted = false };

            // Act
            var result = database.Add(item);

            // Assert
            result.Should().NotBeNull();
            result.Task.Should().Be("New Task");
            result.IsCompleted.Should().BeFalse();
            database.GetAll().Should().ContainSingle(i => i.Id == result.Id);
        }

        [Fact]
        public void Add_NullItem()
        {
            // Arrange
            var database = new InMemoryToDoDatabaseDict();

            // Act & Assert
            var exception = Assert.Throws<NullReferenceException>(() => database.Add(null));
        }

        /*
         * Add_NullItem: if I do database.Add(null) then it already complains. Is it then still valid to test?
         * Add_DuplicateItem: no need to test this because of the guids?
         * Add_ItemWithInvalidFields: same
         */

        [Fact]
        public void Update_ValidItem_UpdatesItem()
        {
            // Arrange
            var database = new InMemoryToDoDatabaseDict();
            var existingItem = database.Add(new AddItemDto { Task = "Original Task", IsCompleted = false });
            var updatedItem = new UpdateItemDto { Task = "Updated Task", IsCompleted = true };

            // Act
            var result = database.Update(existingItem.Id, updatedItem);
            var retrievedItem = database.Get(existingItem.Id);

            // Assert
            result.Should().BeTrue();
            retrievedItem.Should().NotBeNull();
            retrievedItem.Task.Should().Be(updatedItem.Task);
            retrievedItem.IsCompleted.Should().Be(updatedItem.IsCompleted.Value);
        }
        /*
         * Update_InvalidId
         * Update_ItemWithInvalidFields
         * Update_NonExistingItem
         */
    }
}

