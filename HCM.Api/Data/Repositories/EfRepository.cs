using HCM.Api.Data.Contracts;

namespace HCM.Api.Data.Repositories;

public class EfRepository<TEntity> : IRepository<TEntity>
    where TEntity : class
{
    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public IQueryable<TEntity> All()
    {
        throw new NotImplementedException();
    }

    public IQueryable<TEntity> AllAsNoTracking()
    {
        throw new NotImplementedException();
    }

    public Task AddAsync(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public void Update(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public Task<int> SaveChangesAsync()
    {
        throw new NotImplementedException();
    }
}