using Common;
using DataHandler;
using DataModels.Entities;
using ProduktService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProduktService
{
    public class ProductService : IProductService
    {
        private readonly IProductDataHandler _dataHandler;

        public ProductService(IProductDataHandler dataHandler)
        {
            _dataHandler = dataHandler;
        }

        public async Task<ApiResponse<List<Product>>> GetAllAsync()
        {
            var products = await _dataHandler.GetProductsAsync();
            return ApiResponse<List<Product>>.Ok(products);
        }

        public async Task<ApiResponse<Product>> CreateAsync(ApiRequest<Product> request)
        {
            if (request.Data == null)
                return ApiResponse<Product>.Fail("Produktdaten fehlen");

            var product = await _dataHandler.CreateProductAsync(request.Data);
            return ApiResponse<Product>.Ok(product);
        }
        public async Task<ApiResponse<Product>> GetByIdAsync(int id)
        {
            var product = await _dataHandler.GetByIdAsync(id);

            if (product == null)
                return ApiResponse<Product>.Fail("Produkt nicht gefunden");

            return ApiResponse<Product>.Ok(product);
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            var deleted = await _dataHandler.DeleteProductAsync(id);

            if (!deleted)
                return ApiResponse<bool>.Fail("Produkt nicht gefunden");

            return ApiResponse<bool>.Ok(true);
        }
    }
}
