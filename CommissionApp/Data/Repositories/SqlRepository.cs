namespace CommissionApp.Data.Repositories;
using CommissionApp.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

public class SqlRepository<T> : IRepository<T> where T : class, IEntity, new()
{
    private readonly DbSet<T> _dbSet;
    private readonly CommissionAppSQLDbContext _commissionAppSQLDbContext;
    private readonly Action<T>? _itemAddedCallback;
    private readonly List<T> _items = new();
    public SqlRepository(CommissionAppSQLDbContext commissionAppSQLDbContext)
    {
        _commissionAppSQLDbContext = commissionAppSQLDbContext;
        _dbSet = _commissionAppSQLDbContext.Set<T>();
        _commissionAppSQLDbContext.Database.EnsureCreated();
    }
    public event EventHandler<T>? ItemAdded;
    public event EventHandler<T>? ItemRemoved;
    public event EventHandler<T>? NewAuditEntry;
    public IEnumerable<T> GetAll()
    {
        var items = _dbSet.ToList();
        int newId = 1;
        foreach (var item in items)
        {
            item.Id = newId++;
        }
        return items;
    }
    public T? GetById(int id)
    {
        return _dbSet.Find(id);
    }

    public void Add(T item)
    {
        var maxId = _dbSet.Any() ? _dbSet.Max(x => x.Id) : 0;
        item.Id = maxId + 1;
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
}
    