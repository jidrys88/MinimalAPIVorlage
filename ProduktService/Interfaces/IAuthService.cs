using Common;
using DataModels.Dtos;

namespace ProduktService.Interfaces
{
    public interface IAuthService
    {
        Task<ApiResponse<AuthResponseDto>> RegisterAsync(ApiRequest<RegisterDto> request);
        Task<ApiResponse<AuthResponseDto>> LoginAsync(ApiRequest<LoginDto> request);
    }
}
