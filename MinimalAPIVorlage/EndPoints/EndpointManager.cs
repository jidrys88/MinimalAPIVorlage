namespace MinimalAPIVorlage.EndPoints
{
    public static class EndpointManager
    {
        public static void RegisterEndpoints(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var endpointDefinitions = scope.ServiceProvider
                .GetServices<IEndpointDefinition>();

            foreach (var endpoint in endpointDefinitions)
            {
                endpoint.RegisterEndpoints(app);
            }
        }
    }
}
