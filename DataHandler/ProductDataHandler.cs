using DataModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataHandler
{
    public class ProductDataHandler
    : GenericDataHandler<Product>, IProductDataHandler
    {
        public ProductDataHandler(
            DBUmgebung.Repositories.IGenericRepository<Product> repository)
            : base(repository)
        {
        }

        public async Task<List<Product>> GetProductsAsync()
            => await GetAllAsync();

        public async Task<Product> CreateProductAsync(Product product)
            => await CreateAsync(product);
    }
}
