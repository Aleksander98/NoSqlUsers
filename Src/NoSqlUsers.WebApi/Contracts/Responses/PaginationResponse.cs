using MyEmployees.Application.Models;

namespace MyEmployees.WebApi.Contracts.Responses;

[Serializable]
public sealed class PaginationResponse
{
    public PaginationResponse(int pageSize, string? currentPageToken, string? nextPageToken)
    {
        PageSize = pageSize;
        CurrentPageToken = currentPageToken;
        NextPageToken = nextPageToken;
    }
    
    public PaginationResponse(Pagination pagination)
    {
        PageSize = pagination.PageSize;
        CurrentPageToken = pagination.PageToken;
        NextPageToken = pagination.NextPageToken;
    }
    
    public int PageSize { get; }
    
    public string? CurrentPageToken { get; }
    
    public string? NextPageToken { get; }
}