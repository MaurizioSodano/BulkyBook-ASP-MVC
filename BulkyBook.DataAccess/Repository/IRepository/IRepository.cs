using System.Linq.Expressions;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        //T - Category Model
        IEnumerable<T> GetAll();

        //T Ge(int Id); this method finds only for the Id
        T Get(Expression<Func<T, bool>> filter); // 
        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }

}
