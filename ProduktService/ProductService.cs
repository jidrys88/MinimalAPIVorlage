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
        public async Task<ApiResponse<PagedResult<ProductDto>>> GetPagedAsync(int page, int pageSize)
        {
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 20;

            var (items, totalCount) = await _repository.GetPagedAsync(page, pageSize);

            var result = new PagedResult<ProductDto>
            {
                Items = items.Select(p => p.ToDto()).ToList(),
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };

            return ApiResponse<PagedResult<ProductDto>>.Ok(result);
        }
        public async Task<ApiResponse<List<ProductDto>>> CreateManyAsync(ApiRequest<List<ProductRequestDto>> request)
        {
            if (request.Data == null || request.Data.Count == 0)
                return ApiResponse<List<ProductDto>>.Fail("Produktdaten fehlen");

            var allErrors = new List<string>();
            var products = new List<Product>();

            for (int i = 0; i < request.Data.Count; i++)
            {
                var dto = request.Data[i];
                var errors = ValidationHelper.Validate(dto);

                if (errors.Count > 0)
                {
                    allErrors.AddRange(errors.Select(e => $"Produkt #{i + 1}: {e}"));
                    continue;
                }

                products.Add(dto.ToEntity());
            }

            if (allErrors.Count > 0)
                return ApiResponse<List<ProductDto>>.Fail("Validierung fehlgeschlagen", allErrors);

            foreach (var product in products)
                await _repository.AddAsync(product);

            await _repository.SaveAsync();

            return ApiResponse<List<ProductDto>>.Ok(products.Select(p => p.ToDto()).ToList());
        }
    }
}
