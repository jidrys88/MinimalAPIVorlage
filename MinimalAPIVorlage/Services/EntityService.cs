using DataHandler;
using DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class EntityService<TEntity, TData> : IEntityService<TEntity, TData>
    where TEntity : BaseEntity, new()
    {
        private readonly IEntityDataHandler<TEntity> _dataHandler;

        public EntityService(IEntityDataHandler<TEntity> dataHandler)
        {
            _dataHandler = dataHandler;
        }

        public async Task<EntityResponse<TData>> CreateAsync(EntityCreateRequest<TData> request)
        {
            var entity = new TEntity();
            foreach (var prop in typeof(TData).GetProperties())
            {
                var value = prop.GetValue(request.Data);
                var entityProp = typeof(TEntity).GetProperty(prop.Name);
                if (entityProp != null)
                    entityProp.SetValue(entity, value);
            }

            var created = await _dataHandler.CreateAsync(entity);
            return new EntityResponse<TData> { Id = created.Id, Data = request.Data };
        }

        public async Task<PagedResult<EntityResponse<TData>>> GetPagedAsync(int page, int pageSize)
        {
            var paged = await _dataHandler.GetPagedAsync(page, pageSize);

            var items = paged.Items.Select(e =>
            {
                var data = Activator.CreateInstance<TData>();
                foreach (var prop in typeof(TData).GetProperties())
                {
                    var entityProp = typeof(TEntity).GetProperty(prop.Name);
                    if (entityProp != null)
                        prop.SetValue(data, entityProp.GetValue(e));
                }
                return new EntityResponse<TData> { Id = e.Id, Data = data };
            }).ToList();

            return new PagedResult<EntityResponse<TData>>(items, paged.TotalCount);
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            return await _dataHandler.SoftDeleteAsync(id);
        }
    }


}
