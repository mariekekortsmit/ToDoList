# 🌟 Welcome to my Journey of Learning C# with ToDoList! 🌟

🎉 Hey there! Welcome to my adventure in the world of C# programming! This repository isn't just about coding; it's about enjoying the journey of building a ToDoList application from the ground up. 🚀

Here's how you can join in the fun:

1. Start Simple: Kick off with a console-based ToDoList app. It's like the "Hello, World!" of our adventure.
1. Level Up: Transform your simple app into a feature-rich masterpiece, step by step.
1. Explore and Discover: Each assignment is a new quest in the land of C#, complete with my own treasure maps (notes) and secrets I've unearthed along the way. 🗺️

Expect to find:

- 🤔 Puzzles to solve.
- 💡 'Aha!' moments.
- 📚 Learning, packaged in a mix of challenges and triumphs.


Repository for learning C# by working through a ToDoList application in C#. First, write a simple todo-application as console application. Next do a rest API. Then build it up step by step according to the assignments.

## Let the adventure begin

ToDoList: Your Ticket to Coding Adventures! Embark on this quest to conquer the world of C# with our ToDoList app. Each challenge you complete is a badge of honor, a story to tell. 

Ready? Set? Code! 🌈👨‍💻👩‍💻🌈

1.  Write a simple ToDoLits application as console application.
1.  Convert the solution to use REST API.
1.  Replace all foreach loops with LINQ:

    Current implementation uses `FirstOrDefault` as follows:
    ```csharp
        public bool Update(int id, UpdateItemDto item)
        {
            var itemToUpdate = _items.FirstOrDefault(item => item.Id == id);
            if (itemToUpdate != null)
            {
                if (item.Task != null)
                {
                    itemToUpdate.Task = item.Task;
                }
                if (item.IsCompleted.HasValue)
                {
                    itemToUpdate.IsCompleted = item.IsCompleted.Value;
                }
                return true;
            }
            return false;
        }
    ```
    To use `First` instead of `FirstOrDefault`: `First` throws an exception if no item is found that matches the condition. If you expect that an item with the given id may not exist, it's generally safer to use `FirstOrDefault`. However, if you are sure that the item will exist and you want to use `First`, you should wrap it in a try-catch block to handle the potential `InvalidOperationException`. Using `FirstOrDefault` is the correct approach when the presence of the item isn't guaranteed. It avoids the unnecessary cost of exception handling which should not be used for normal control flow in your programs.

    Here's how you might use  `First`:
    ```csharp
    public bool Update(int id, UpdateItemDto item)
    {
        try
        {
            // Find the first item that matches the ID or throw an exception if none found.
            var itemToUpdate = _items.First(i => i.Id == id);
            
            if (item.Task != null)
            {
                itemToUpdate.Task = item.Task;
            }
            if (item.IsCompleted.HasValue)
            {
                itemToUpdate.IsCompleted = item.IsCompleted.Value;
            }

            // If we've reached here, the item has been successfully updated.
            return true;
        }
        catch (InvalidOperationException)
        {
            // If an exception was caught, it means no item matched the ID.
            return false;
        }
    }
    ```
1.  Replace the inmemory database with an interface;
1.  Add a seperate implementation for the database that uses a dictionary instead of a list.
1.  Split out classes and files in different folders.
1.  Update the code to handle concurrent updates. Assume 2 people are creating a new todo item at the same time. What to do with the ids? Think of the solution as 
    multiple backends running against a central database. The database will then handle the concurrency. How to solve it here? 

    I found several solutions: 
    - `int newId = Interlocked.Increment(ref _nextId);`
    - `Id = Guid.NewGuid(),`s
    - `lock (_lock) {}`
    
    which are complimentary in the sense that you can choose either `Guid` or `Interlocked.Increment` because they are both guaranteeing a unique Id. But additionally to control access to the "database" itself you need some kind of `lock` pattern.
    Whereas my first implementation included the lock in the form of"
    ```csharp
    private readonly object _lock = new();
    lock (_lock)
    {
        // code to be locked
    }
    ``` 
    You don't need a new object `_lock` for this as you can just use `lock(this)` instead.
1.  Dependency inject which database to use.
1.  Implement the Mediater Pattern. Use the `Mediatr` package.
1.  Unit tests.
    
    - Using the interface to test both db implementations in the same way.

        At first, I just unit tested one implementation of my database, the dictionary version, with something that looked like this:
        ```csharp
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
        ```
        However, changing to this implementation of unit tests makes that you can use the interface to test both implementations at once:
        ```csharp
        public class InMemoryToDoDatabaseDictTests : InMemoryToDoDatabaseTests<InMemoryToDoDatabaseDict> { }
        public class InMemoryToDoDatabaseListTests : InMemoryToDoDatabaseTests<InMemoryToDoDatabaseList> { }

        public abstract class InMemoryToDoDatabaseTests<T> where T: IToDoDatabase, new()
        {
            [Fact]
            public void GetAll_NonEmptyDatabase_ShouldReturnAllItems()
            {
                // Arrange
                var database = new T();
                // ..
            }
        }
        ```

    - Mocks in unit testing: 
        
        The first implementation of unit tests included a mocked database based on the interface like this:
        ```csharp
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
        ```
        However, as the database implementations should be fully tested, you don't need to mock the actual database in your other tests as you can then trust this is doing the correct thing (you've covered this in your other unit tests).
