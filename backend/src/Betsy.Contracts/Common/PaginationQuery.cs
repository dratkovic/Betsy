namespace Betsy.Contracts.Common;

public class PaginationQuery
{
    private const int DefaultPage = 1;
    private const int DefaultPageSize = 10;

    public int? Page { get; set; }
    public int? PageSize { get; set; }

    public int GetPage() => Page ?? DefaultPage;
    public int GetPageSize() => PageSize ?? DefaultPageSize;

    public string? Filter { get; set; }
}
