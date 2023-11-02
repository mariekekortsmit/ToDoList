namespace ToDoList
{
    public class InMemoryToDoDatabase
    {
        private readonly List<ToDoItem> _items = new();
        private int _nextId = 1;

        // Retrieve all items.
        public List<ToDoItemDto> GetAll() => _items.Select(x => x.ToDto()).ToList();

        // Find an item by Id.
        public ToDoItemDto? Get(int id)
        {
            foreach (var item in _items)
            {
                if (item.Id == id)
                {
                    return item.ToDto();
                }
            }
            return null;
        }

        // Add a new item.
        public ToDoItem Add(AddItemDto item)
        {
            _items.Add(new ToDoItem() { Id = _nextId++, Task = item.Task, IsCompleted = item.IsCompleted });
            return _items.Last();
        }

        // Update an existing item.
        public bool Update(int id, UpdateItemDto item)
        {
            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i].Id == id)
                {
                    if (item.Task != null)
                    {
                        _items[i].Task = item.Task;
                    }
                    if (item.IsCompleted.HasValue)
                    {
                        _items[i].IsCompleted = item.IsCompleted.Value;
                    }
                    return true;
                }
            }
            return false;
        }

        // Delete an item by Id.
        public bool Delete(int id)
        {
            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i].Id == id)
                {
                    _items.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }
    }
}
