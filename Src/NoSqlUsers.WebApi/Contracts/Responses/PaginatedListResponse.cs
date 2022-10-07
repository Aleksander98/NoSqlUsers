namespace MyEmployees.WebApi.Contracts.Responses;

[Serializable]
public sealed class PaginatedListResponse<T>
{
    public PaginatedListResponse(List<T> items, PaginationResponse pagination)
    {
        Items = items ?? throw new ArgumentNullException(nameof(items));
        Pagination = pagination ?? throw new ArgumentNullException(nameof(pagination));
    }

    public List<T> Items { get; }
    
    public PaginationResponse Pagination { get; }
}