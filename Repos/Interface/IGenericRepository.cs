using System.Linq.Expressions;

namespace POC_TaskBoard.API.Repos.Interface
{
    public interface IGenericRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<List<T>> GetListWhereAsync(Expression<Func<T, bool>>? predicate);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity, int id);
        Task DeleteAsync(int id);
        Task<bool> BoardExistAsync(Expression<Func<T, bool>> predicate);
    }
}
