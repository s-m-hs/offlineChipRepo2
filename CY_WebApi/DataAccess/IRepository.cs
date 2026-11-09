using System.Linq.Expressions;

namespace CY_WebApi.DataAccess
{
    internal interface IRepository<T> where T : class
    {
        Task<T> Find(Guid id);

        T FirstOrDefault(Expression<Func<T, bool>> predicate);

        Task<T> Insert(T entity);

        Task<int> Update(T entity);

        Task<int> Delete(Guid id);

        IQueryable<T> Table { get; }

        IQueryable<T> TableNoTracking { get; }

        DateTime GetDbTime();

        int SaveChanges();

        void DetectChanges();
    }
}
