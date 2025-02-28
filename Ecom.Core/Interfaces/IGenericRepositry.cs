using System.Linq.Expressions;

namespace Ecom.Core.Interfaces;

public interface IGenericRepositry<T> where T : class
{
    Task<IReadOnlyList<T>> GetAllAsync();
    Task<IReadOnlyList<T>> GetAllAsync(params Expression<Func<T, object>>[] includes);
    Task AddAsync(T entity);
    Task<T> GetByIdAsync(int id);
    Task<T> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
    Task<int>CountAsync();
}