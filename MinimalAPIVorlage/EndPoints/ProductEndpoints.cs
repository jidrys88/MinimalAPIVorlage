using Common;
using DataModels.Dtos;
using ProduktService.Interfaces;

namespace MinimalAPIVorlage.EndPoints
{
    public class ProductEndpoints : IEndpointDefinition
    {
        public void RegisterEndpoints(IEndpointRouteBuilder app)
        {
            app.MapGet("/products", async (IProductService service) =>
                    await service.GetAllAsync())
                .WithTags("Products")
                .WithOpenApi();

            app.MapGet("/products/{id:int}", async (int id, IProductService service) =>
                    await service.GetByIdAsync(id))
                .WithTags("Products")
                .WithOpenApi();

            app.MapPost("/products", async (
                    IProductService service,
                    ApiRequest<ProductRequestDto> request) =>
                        await service.CreateAsync(request))
                .WithTags("Products")
                .WithOpenApi();

            app.MapPut("/products/{id:int}", async (
                    int id,
                    IProductService service,
                    ApiRequest<ProductRequestDto> request) =>
                        await service.UpdateAsync(id, request))
                .WithTags("Products")
                .WithOpenApi();

            app.MapDelete("/products/{id:int}", async (int id, IProductService service) =>
                    await service.DeleteAsync(id))
                .WithTags("Products")
                .WithOpenApi();
        }
    }
}
