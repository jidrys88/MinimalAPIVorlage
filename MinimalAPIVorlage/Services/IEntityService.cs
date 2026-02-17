using DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IEntityService<TEntity, TData>
    where TEntity : BaseEntity, new()
    {
        Task<EntityResponse<TData>> CreateAsync(EntityCreateRequest<TData> request);
        Task<PagedResult<EntityResponse<TData>>> GetPagedAsync(int page, int pageSize);
        Task<bool> SoftDeleteAsync(int id);
    }

}
