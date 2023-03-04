using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace HCM.Shared.Data.Contracts;

public interface IRepository<TEntity> : IDisposable
    where TEntity : class
{
    IQueryable<TEntity> All();

    IQueryable<TEntity> AllAsNoTracking();

    ValueTask<EntityEntry<TEntity>> AddAsync(TEntity entity);

    void Update(TEntity entity);

    void Delete(TEntity entity);

    Task<int> SaveChangesAsync();

    //DbSet<TEntity> GetIncludingChildren(Expression<Func<TEntity, bool>> filter,
    //    params Expression<Func<TEntity, object>>[] children);
}