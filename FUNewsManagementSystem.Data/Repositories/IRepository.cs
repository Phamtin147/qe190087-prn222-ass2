namespace FUNewsManagementSystem.Data.Repositories;

public interface IRepository<T, in TKey>
{
    IReadOnlyList<T> GetAll();
    T? GetById(TKey id);
    void Add(T entity);
    void Update(T entity);
    bool Delete(TKey id);
}
