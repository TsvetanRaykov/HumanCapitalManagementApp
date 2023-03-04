using HCM.Shared.Data.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace HCM.Api.Data.Repositories;

public class EfRepository<TEntity> : IRepository<TEntity>
    where TEntity : class
{
    private readonly ApiDbContext _dbContext;
    private readonly DbSet<TEntity> _dbSet;

    public EfRepository(ApiDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<TEntity>();
    }

    public void Dispose() => _dbContext.Dispose();

    public IQueryable<TEntity> All() => _dbSet;

    public IQueryable<TEntity> AllAsNoTracking() => _dbSet.AsNoTracking();

    public ValueTask<EntityEntry<TEntity>> AddAsync(TEntity entity) => _dbSet.AddAsync(entity);

    public void Update(TEntity entity)
    {
        var entry = _dbContext.Entry(entity);
        if (entry.State == EntityState.Detached)
        {
            _dbSet.Attach(entity);
        }

        entry.State = EntityState.Modified;
    }

    public void Delete(TEntity entity) => _dbSet.Remove(entity);

    public Task<int> SaveChangesAsync() => _dbContext.SaveChangesAsync();
}