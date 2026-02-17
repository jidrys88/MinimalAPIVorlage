namespace Services
{
    public record EntityCreateRequest<TData>
    {
        public TData Data { get; init; } = default!;
    }

    public record EntityResponse<TData>
    {
        public int Id { get; init; }
        public TData Data { get; init; } = default!;
    }

    // Paging
    public record PagedResult<T>(IReadOnlyList<T> Items, int TotalCount);

}
