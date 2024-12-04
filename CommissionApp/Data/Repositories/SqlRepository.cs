namespace CommissionApp.Data.Repositories;
using CommissionApp.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text.Json;

public class SqlRepository<T> : IRepository<T> where T : class, IEntity, new()
{
    private readonly DbSet<T> _dbSet;
    private readonly CommissionAppSQLDbContext _commissionAppSQLDbContext;
    private readonly Action<T>? _itemAddedCallback;
    private int? lastUsedId = 1;
    private readonly string path = $"{typeof(T).Name}_save.json";
    private readonly List<T> _items = new();
         public SqlRepository(CommissionAppSQLDbContext commissionAppSQLDbContext)
    {
        _commissionAppSQLDbContext = commissionAppSQLDbContext;
        _dbSet = _commissionAppSQLDbContext.Set<T>();
    }
    public event EventHandler<T>? ItemAdded;
    public event EventHandler<T>? ItemRemoved;
    public event EventHandler<T>? NewAuditEntry;
    public IEnumerable<T> GetAll()
    {
        return _dbSet.ToList();
    }
    public T? GetById(int id)
    {
        return _dbSet.Find(id);
    }

    public void Add(T item)
    {
          if (_items.Count == 0)
            {
            item.Id = lastUsedId;
            lastUsedId++;
        }
            else if (_items.Count > 0)
            {
                lastUsedId = _items[_items.Count - 1].Id;
                item.Id = ++lastUsedId;
            }
        _dbSet.Add(item);
        _itemAddedCallback?.Invoke(item);
        ItemAdded?.Invoke(this, item);
        Save();

    }
    public void Remove(T item)
    {
        _dbSet.Remove(item);
        ItemRemoved?.Invoke(this, item);
        NewAuditEntry?.Invoke(this, item);
    }
    public void RemoveAll()
    {
        var allEntities = _dbSet.ToList();
        foreach (var item in allEntities)
        {
            Remove(item);
        }
    }
    public void Save()
    {
    _commissionAppSQLDbContext.SaveChanges();
    }
    public void SaveToFile()
        {
        File.Delete(path);
            var objectsSerialized = JsonSerializer.Serialize<IEnumerable<T>>(_dbSet);
            File.WriteAllText(path, objectsSerialized);
        }

        public IEnumerable<T> Read()
        {
            if (File.Exists(path))
            {
                var objectsSerialized = File.ReadAllText(path);
                var deserializedObjects = JsonSerializer.Deserialize<IEnumerable<T>>(objectsSerialized);
                if (deserializedObjects != null)
                {
                    foreach (var item in deserializedObjects)
                    {
                    _dbSet.Add(item);
                    }
                }
            }
            return _dbSet;
        }
}