namespace SportsPro.DataLayer
{
    public interface IRepository<T> where T : class
    {



        // The IRepository<T> interface defines the contract for a
        // generic repository that provides basic CRUD
        // (Create, Read, Update, Delete) operations for
        // entities of type T. It includes multiple methods that will
        // be implemented by the actual repository classes to interact
        // with the underlying data store.
        IEnumerable<T> List(QueryOptions<T> options);
        T? Get(int id);
        T? Get(QueryOptions<T> options);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Save();
    }
}
