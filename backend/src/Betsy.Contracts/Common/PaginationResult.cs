namespace Betsy.Contracts.Common;

public class PaginationResult<T> where T : class
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalRecords { get; set; }
    public IEnumerable<T> Data { get; set; } = new List<T>();
}
