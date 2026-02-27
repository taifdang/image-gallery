namespace Application.Common;

public class PageList<TEntity>(IEnumerable<TEntity> items, int pageSize, int pageIndex, int count)
{
    public int PageSize => pageSize;
    public int PageIndex => pageIndex;
    public int Count => count;
    public IEnumerable<TEntity> Items => items;
}