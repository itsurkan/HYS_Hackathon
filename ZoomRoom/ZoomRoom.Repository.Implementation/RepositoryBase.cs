using Microsoft.EntityFrameworkCore;
using ZoomRoom.Domain.Entities;
using ZoomRoom.Persistence;
using ZoomRoom.Repository.Contracts;


namespace ZoomRoom.Repository.Implementation
{
    public class RepositoryBase<TEntity>(SqliteDbContext dbContext) : IRepositoryBase<TEntity>
        where TEntity : BaseEntity
    {
        public void Delete(TEntity entity)
        {
            dbContext.Remove(entity);
        }

        public IQueryable<TEntity> GetAll()
        {
            return dbContext.Set<TEntity>().AsNoTracking();
        }

        public async Task<TEntity?> GetByIdAsync(long id)
        {
            return await dbContext.Set<TEntity>().SingleOrDefaultAsync(entity => entity.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }

        public void Update(TEntity entity)
        {
            dbContext.Update(entity);
        }

        public TEntity Create(TEntity entity)
        {
            dbContext.Add(entity);

            return entity;
        }

        public async Task<TEntity> FindByIdAsync(long id)
        {
            return await dbContext.Set<TEntity>().FirstOrDefaultAsync(entity => entity.Id == id);
        }
    }
}
