using Common;
using DataModels.Entities;
using DBUmgebung.Repositories;
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
        private readonly IGenericRepository<Product> _repository;

        public ProductService(IGenericRepository<Product> repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponse<List<Product>>> GetAllAsync()
        {
            var products = await _repository.GetAllAsync();
            return ApiResponse<List<Product>>.Ok(products);
        }

        public async Task<ApiResponse<Product>> CreateAsync(ApiRequest<Product> request)
        {
            if (request.Data == null)
                return ApiResponse<Product>.Fail("Produktdaten fehlen");

            await _repository.AddAsync(request.Data);
            await _repository.SaveAsync();

            return ApiResponse<Product>.Ok(request.Data);
        }

        public async Task<ApiResponse<Product>> GetByIdAsync(int id)
        {
            var product = await _repository.GetByIdAsync(id);

            if (product == null)
                return ApiResponse<Product>.Fail("Produkt nicht gefunden");

            return ApiResponse<Product>.Ok(product);
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            var product = await _repository.GetByIdAsync(id);

            if (product == null)
                return ApiResponse<bool>.Fail("Produkt nicht gefunden");

            await _repository.DeleteAsync(product);
            await _repository.SaveAsync();

            return ApiResponse<bool>.Ok(true);
        }
    }
}