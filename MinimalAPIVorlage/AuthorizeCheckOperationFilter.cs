using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MinimalAPIVorlage
{
    // Zeigt das Schloss-Symbol in Swagger UI NUR bei Endpoints,
    // die tatsächlich .RequireAuthorization() haben - statt global bei allen.
    public class AuthorizeCheckOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var requiresAuth = context.ApiDescription.ActionDescriptor.EndpointMetadata
                .OfType<IAuthorizeData>()
                .Any();

            if (!requiresAuth)
                return;

            operation.Security.Add(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        }
    }
}
