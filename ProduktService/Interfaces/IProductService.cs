using Common;
using DataModels.Entities;

namespace ProduktService.Interfaces
{
    public interface IProductService
    {
        Task<ApiResponse<List<Product>>> GetAllAsync();
        Task<ApiResponse<Product>> CreateAsync(ApiRequest<Product> request);
    }
}
