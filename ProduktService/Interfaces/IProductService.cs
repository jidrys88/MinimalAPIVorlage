using Common;
using DataModels.Dtos;

namespace ProduktService.Interfaces
{
    public interface IProductService
    {
        Task<ApiResponse<List<ProductDto>>> GetAllAsync();
        Task<ApiResponse<PagedResult<ProductDto>>> GetPagedAsync(int page, int pageSize);
        Task<ApiResponse<ProductDto>> GetByIdAsync(int id);
        Task<ApiResponse<ProductDto>> CreateAsync(ApiRequest<ProductRequestDto> request);
        Task<ApiResponse<ProductDto>> UpdateAsync(int id, ApiRequest<ProductRequestDto> request);
        Task<ApiResponse<bool>> DeleteAsync(int id);
        Task<ApiResponse<List<ProductDto>>> CreateManyAsync(ApiRequest<List<ProductRequestDto>> request);

    }
}
