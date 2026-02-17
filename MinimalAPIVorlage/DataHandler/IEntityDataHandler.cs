using DataModels;

namespace DataHandler
{
    public interface IEntityDataHandler<TEntity>
    where TEntity : BaseEntity
    {
        Task<TEntity> CreateAsync(TEntity entity);
        Task<PagedResult<TEntity>> GetPagedAsync(int page, int pageSize);
        Task<bool> SoftDeleteAsync(int id);
    }

}
