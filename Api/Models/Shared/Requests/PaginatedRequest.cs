using Microsoft.AspNetCore.Mvc;

namespace Api.Models.Shared.Requests;

public class PaginatedRequest
{
    [FromQuery(Name = "page")]
    public int Page { get; set; } = 1;

    [FromQuery(Name = "page_size")]
    public int PageSize { get; set; } = 100;

    public int OffSet => Page < 2 ? 0 : (Page - 1) * PageSize;
}
