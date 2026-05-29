namespace Api.Models.Shared.Responses;

public class Meta
{
    public required int Page { get; set; } = 1;

    public required int PageSize { get; set; } = 100;

    public required int Count { get; set; }

    public required int Total { get; set; }

    // delete what gets unused here
    public int TotalPages => Math.Max((int)Math.Ceiling((double)(Total / PageSize)), 1);
    public int OffSet => (Page - 1) * PageSize;
    public bool HasNext => Page < TotalPages;
    public bool HasPrevious => Page > 1;
}

public class PaginatedResponse<T>
{
    public required IEnumerable<T> Data { get; set; }

    public required Meta Meta { get; set; }
}
