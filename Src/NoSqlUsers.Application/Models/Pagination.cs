namespace MyEmployees.Application.Models;

public sealed class Pagination
{
    public Pagination()
    {
    }

    public Pagination(int pageSize, string? pageToken, string? nextPageToken)
    {
        PageSize = pageSize;
        PageToken = pageToken;
        NextPageToken = nextPageToken;
    }
    
    public int PageSize { get; set; }
    
    public string? PageToken { get; set; }
    
    public string? NextPageToken { get; set; }
}