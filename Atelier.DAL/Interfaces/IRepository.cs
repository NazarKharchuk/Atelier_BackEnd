namespace Atelier.DAL.Interfaces
{
    public interface IRepository<T>
    {
        IQueryable<T> GetAll();
        Task<T> Get(int id);
        List<T> Find(Func<T, Boolean> predicate);
        Task Create(T item);
        void Update(T item);
        Task Delete(int id);
    }
}
