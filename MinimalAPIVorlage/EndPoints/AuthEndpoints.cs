using Common;
using DataModels.Dtos;
using ProduktService.Interfaces;

namespace MinimalAPIVorlage.EndPoints
{
    public class AuthEndpoints : IEndpointDefinition
    {
        public void RegisterEndpoints(IEndpointRouteBuilder app)
        {
            app.MapPost("/auth/register", async (
                    IAuthService service,
                    ApiRequest<RegisterDto> request) =>
                        await service.RegisterAsync(request))
                .WithTags("Auth")
                .WithOpenApi();

            app.MapPost("/auth/login", async (
                    IAuthService service,
                    ApiRequest<LoginDto> request) =>
                        await service.LoginAsync(request))
                .WithTags("Auth")
                .WithOpenApi();
        }
    }
}
