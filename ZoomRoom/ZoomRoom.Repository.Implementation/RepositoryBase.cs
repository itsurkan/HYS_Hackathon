using Microsoft.EntityFrameworkCore;
using ZoomRoom.Domain.Entities;
using ZoomRoom.Persistence;
using ZoomRoom.Repository.Contracts;
using System.Linq.Expressions;


namespace ZoomRoom.Repository.Implementation
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity>
     where TEntity : BaseEntity
    {
        private readonly SqliteDbContext _context;
        public RepositoryBase(SqliteDbContext sqliteDbContext)
        {
            _context = sqliteDbContext;
        }
        public void Delete(TEntity entity)
        {
            _context.Remove(entity);
        }

        public IQueryable<TEntity> GetAll()
        {
            return _context.Set<TEntity>().AsNoTracking();
        }

        public async Task<TEntity?> GetByIdAsync(long id)
        {
            return await _context.Set<TEntity>().SingleOrDefaultAsync(entity => entity.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Update(TEntity entity)
        {
            _context.Update(entity);
        }

        public TEntity Create(TEntity entity)
        {
            _context.Add(entity);

            return entity;
        }

        public async Task<TEntity> FindByIdAsync(long id)
        {
            return await _context.Set<TEntity>().FirstOrDefaultAsync(entity => entity.Id == id);
        }
    }
}
