using DBUmgebung.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataHandler
{
    public class GenericDataHandler<T> : IGenericDataHandler<T>
    where T : class
    {
        protected readonly IGenericRepository<T> _repository;

        public GenericDataHandler(IGenericRepository<T> repository)
        {
            _repository = repository;
        }

        public async Task<List<T>> GetAllAsync()
            => await _repository.GetAllAsync();

        public async Task<T?> GetByIdAsync(int id)
            => await _repository.GetByIdAsync(id);

        public async Task<T> CreateAsync(T entity)
        {
            await _repository.AddAsync(entity);
            await _repository.SaveAsync();
            return entity;
        }
    }
}
