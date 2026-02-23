using DataModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataHandler
{
    public interface IProductDataHandler
    {
        Task<List<Product>> GetProductsAsync();
        Task<Product> CreateProductAsync(Product product);
    }
}
