using System.Linq.Expressions;
using GiftOfTheGivers.Web.Data;
using GiftOfTheGivers.Web.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GiftOfTheGivers.Web.Repositories;

/// <summary>EF Core backed generic repository. Concrete repositories inherit this
/// to get common CRUD behaviour and add entity-specific queries.</summary>
public class Repository<T> : IRepository<T> where T : class
{
    protected readonly ApplicationDbContext Context;
    protected readonly DbSet<T> DbSet;

    public Repository(ApplicationDbContext context)
    {
        Context = context;
        DbSet = context.Set<T>();
    }

    public virtual async Task<T?> GetByIdAsync(int id) => await DbSet.FindAsync(id);

    public virtual async Task<IReadOnlyList<T>> GetAllAsync() => await DbSet.ToListAsync();

    public virtual async Task<IReadOnlyList<T>> FindAsync(Expression<Func<T, bool>> predicate) =>
        await DbSet.Where(predicate).ToListAsync();

    public virtual async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate) =>
        await DbSet.FirstOrDefaultAsync(predicate);

    public virtual async Task AddAsync(T entity) => await DbSet.AddAsync(entity);

    public virtual void Update(T entity) => DbSet.Update(entity);

    public virtual void Remove(T entity) => DbSet.Remove(entity);

    public virtual async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null) =>
        predicate is null ? await DbSet.CountAsync() : await DbSet.CountAsync(predicate);

    public virtual async Task<int> SaveChangesAsync() => await Context.SaveChangesAsync();
}


