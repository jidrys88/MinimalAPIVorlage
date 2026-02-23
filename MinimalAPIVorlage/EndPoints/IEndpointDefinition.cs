namespace MinimalAPIVorlage.EndPoints
{
    public interface IEndpointDefinition
    {
        void RegisterEndpoints(IEndpointRouteBuilder app);
    }
}
