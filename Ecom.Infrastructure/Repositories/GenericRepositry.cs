using Ecom.Core.Interfaces;
using Ecom.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Ecom.Infrastructure.Repositories;

public class GenericRepositry<T> : IGenericRepositry<T> where T : class
{
    private readonly AppDbContext _Dbcontext;

    public GenericRepositry(AppDbContext dbcontext)
    {
        _Dbcontext = dbcontext;
    }

    public async Task AddAsync(T entity)
    {
        await _Dbcontext.Set<T>().AddAsync(entity);
        await _Dbcontext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _Dbcontext.Set<T>().FindAsync(id);
        _Dbcontext.Set<T>().Remove(entity);
        await _Dbcontext.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<T>> GetAllAsync() => await _Dbcontext.Set<T>().AsNoTracking().ToListAsync();

    public async Task<IReadOnlyList<T>> GetAllAsync(params Expression<Func<T, object>>[] includes)
    {
        var query = _Dbcontext.Set<T>().AsQueryable();
        foreach (Expression<Func<T, object>> include in includes)
        {
            query = query.Include(include);
        }
        return await query.ToListAsync();
    }

    public async Task<T> GetByIdAsync(int id)
    {
        var entity=await _Dbcontext.Set<T>().FindAsync(id);
        return entity;
    }

    public async Task<T> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes)
    {
        var query = _Dbcontext.Set<T>().AsQueryable();
        foreach (Expression<Func<T, object>> include in includes)
        {
            query = query.Include(include);
        }
        var entity = await query.FirstOrDefaultAsync(x => EF.Property<int>(x, "Id") == id);
        return entity;
    }

    public async Task UpdateAsync(T entity)
    {
       _Dbcontext.Entry(entity).State= EntityState.Modified;
        await _Dbcontext.SaveChangesAsync();
    }
}