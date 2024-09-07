using ZoomRoom.Domain.Entities;

namespace ZoomRoom.Repository.Contracts
{
    public interface IRepositoryBase<TEntity>
        where TEntity : BaseEntity
    {
        public IQueryable<TEntity> GetAll();
        public Task<TEntity?> GetByIdAsync(long id);

        public void Update(TEntity entity);

        public void Delete(TEntity entity);

        public Task SaveChangesAsync();

        public TEntity Create(TEntity entity);

        public Task<TEntity> FindByIdAsync(long id);

    }
}
