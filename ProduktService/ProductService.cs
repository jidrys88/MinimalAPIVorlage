using Common;
using DataModels.Dtos;
using DataModels.Entities;
using DBUmgebung.Repositories;
using ProduktService.Interfaces;

namespace ProduktService
{
    public class ProductService : IProductService
    {
        private readonly IGenericRepository<Product> _repository;

        public ProductService(IGenericRepository<Product> repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponse<List<ProductDto>>> GetAllAsync()
        {
            var products = await _repository.GetAllAsync();
            var dtos = products.Select(p => p.ToDto()).ToList();

            return ApiResponse<List<ProductDto>>.Ok(dtos);
        }

        public async Task<ApiResponse<ProductDto>> GetByIdAsync(int id)
        {
            var product = await _repository.GetByIdAsync(id);

            if (product == null)
                return ApiResponse<ProductDto>.Fail("Produkt nicht gefunden");

            return ApiResponse<ProductDto>.Ok(product.ToDto());
        }

        public async Task<ApiResponse<ProductDto>> CreateAsync(ApiRequest<ProductRequestDto> request)
        {
            if (request.Data == null)
                return ApiResponse<ProductDto>.Fail("Produktdaten fehlen");

            var errors = ValidationHelper.Validate(request.Data);
            if (errors.Count > 0)
                return ApiResponse<ProductDto>.Fail("Validierung fehlgeschlagen", errors);

            var product = request.Data.ToEntity();

            await _repository.AddAsync(product);
            await _repository.SaveAsync();

            return ApiResponse<ProductDto>.Ok(product.ToDto());
        }

        public async Task<ApiResponse<ProductDto>> UpdateAsync(int id, ApiRequest<ProductRequestDto> request)
        {
            if (request.Data == null)
                return ApiResponse<ProductDto>.Fail("Produktdaten fehlen");

            var errors = ValidationHelper.Validate(request.Data);
            if (errors.Count > 0)
                return ApiResponse<ProductDto>.Fail("Validierung fehlgeschlagen", errors);

            var product = await _repository.GetByIdAsync(id);
            if (product == null)
                return ApiResponse<ProductDto>.Fail("Produkt nicht gefunden");

            request.Data.ApplyTo(product);
            await _repository.SaveAsync();

            return ApiResponse<ProductDto>.Ok(product.ToDto());
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
