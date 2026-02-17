using DataModels;
using Services;

namespace MinimalAPIVorlage
{
    public static class EntityEndpointManager
    {
        // Global Mapping
        public static void MapEntityEndpoints(this WebApplication app)
        {
            MapProductEndpoints(app);
            // Weitere Services einfach hier hinzufügen:
            // MapCustomerEndpoints(app);
            // MapOrderEndpoints(app);
        }

        // Jeder Service bekommt eigene Methode
        private static void MapProductEndpoints(WebApplication app)
        {
            app.MapGet("/products", async (int page, int pageSize, IEntityService<Product, ProductData> service) =>
            {
                return await service.GetPagedAsync(page, pageSize);
            })
            .Produces<DataModels.PagedResult<EntityResponse<ProductData>>>(StatusCodes.Status200OK);

            app.MapPost("/products", async (EntityCreateRequest<ProductData> request, IEntityService<Product, ProductData> service) =>
            {
                var result = await service.CreateAsync(request);
                return Results.Created($"/products/{result.Id}", result);
            })
            .Produces<EntityResponse<ProductData>>(StatusCodes.Status201Created);

            app.MapDelete("/products/{id:int}", async (int id, IEntityService<Product, ProductData> service) =>
            {
                return await service.SoftDeleteAsync(id)
                    ? Results.NoContent()
                    : Results.NotFound();
            });
        }
    }
}

 
