namespace Napoleon.Shared.Contract.ApiResponse;
public record PagingResult<TType>(IEnumerable<TType> Items, int TotalItem, int PageSize, int CurrentPage)
    where TType : class
{
    public int ItemCount => Items.Count();
}
