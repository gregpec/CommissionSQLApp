namespace CommissionApp.Data.Repositories
{
    using CommissionApp.Data.Entities;
    using System.Collections.Generic;
 
    public class ListRepository<T> : IRepository<T>
            where T : class, IEntity, new()
    {
        private readonly List<T> _items = new();
        public event EventHandler<T>? ItemAdded;
        public event EventHandler<T>? ItemRemoved;
        public event EventHandler<T>? NewAuditEntry;

        public IEnumerable<T> GetAll()
        {
            return _items.ToList();
        }

        public void Add(T item)
        {        
            item.Id = _items.Count + 1; 
            _items.Add(item);
            OnItemAdded(item);
        }

        public T? GetById(int id)
        {
            var itemById = _items.SingleOrDefault(item => item.Id == id);
            if (itemById == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Object {typeof(T).Name} with id {id} not found.");
                Console.ResetColor();
            }
            return itemById;
        }

        public void Remove(T item)
        {
            _items.Remove(item);
            ItemRemoved?.Invoke(this, item);
        }

        public void RemoveAll()
        {
            var allEntities = _items.ToList();
            foreach (var item in allEntities)
            {
                Remove(item);
            }
        }

        public void Save()
        {
        }
        protected virtual void OnItemAdded(T item)
        {
            ItemAdded?.Invoke(this, item);
        }
    }
}


