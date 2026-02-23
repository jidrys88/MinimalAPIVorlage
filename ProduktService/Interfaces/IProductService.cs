using Common;
using DataModels.Entities;

namespace ProduktService.Interfaces
{
    public interface IProductService
    {
        Task<ApiResponse<List<Product>>> GetAllAsync();
        Task<ApiResponse<Product>> GetByIdAsync(int id);
        Task<ApiResponse<Product>> CreateAsync(ApiRequest<Product> request);
        Task<ApiResponse<bool>> DeleteAsync(int id);
    }
}
