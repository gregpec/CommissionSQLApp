using CommissionApp.Data.Entities;
namespace CommissionApp.Data.Repositories;
public interface IRepository<T> : IReadRepository<T>, IWriteRepository<T>
    where T : class, IEntity
{
    public const string auditFileName = "Resources\\Files\\Audit.txt";
    event EventHandler<T>? ItemAdded;
    event EventHandler<T>? ItemRemoved;
    event EventHandler<T> NewAuditEntry;

    void Add(T item);
    void Remove(T item);
    void Save();




}


