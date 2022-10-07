namespace MyEmployees.Application.Models;

public sealed class PaginatedList<T>
{
    public PaginatedList(List<T> items, Pagination? pagination = null)
    {
        Items = items ?? throw new ArgumentNullException(nameof(items));
        Pagination = pagination ?? new Pagination();
    }

    public PaginatedList(List<T> items, int pageSize, string? pageToken, string? nextPageToken)
    {
        Items = items ?? throw new ArgumentNullException(nameof(items));
        Pagination = new Pagination(pageSize, pageToken, nextPageToken);
    }
    
    public List<T> Items { get; }

    public Pagination Pagination { get; }
}