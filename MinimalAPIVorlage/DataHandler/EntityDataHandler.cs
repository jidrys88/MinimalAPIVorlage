using DataModels;
using DBUmgebung;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataHandler
{
    public class EntityDataHandler<TEntity> : IEntityDataHandler<TEntity>
     where TEntity : BaseEntity
    {
        private readonly AppDbContext _db;

        public EntityDataHandler(AppDbContext db) => _db = db;

        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            _db.Set<TEntity>().Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<PagedResult<TEntity>> GetPagedAsync(int page, int pageSize)
        {
            var query = _db.Set<TEntity>().AsNoTracking();
            var total = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<TEntity>(items, total);
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            var entity = await _db.Set<TEntity>().FindAsync(id);
            if (entity == null) return false;

            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return true;
        }
    }

}
