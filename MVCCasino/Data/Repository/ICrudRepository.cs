namespace MVCCasino.Data.Repository;

public interface ICrudRepository<T>
{
    int Create(T entity);

    T GetById(int id);

    bool Update(T entity);

    bool Delete(int id);
}