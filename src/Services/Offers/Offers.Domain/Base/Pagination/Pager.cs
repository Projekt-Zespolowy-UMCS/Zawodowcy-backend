namespace Offers.Domain.Base.Pagination;

public class Pager<T> where T: class
{
    public int TotalItems { get; private set; }
    public int CurrentPage { get; private set; }
    public int PageSize { get; private set; }
    public int TotalPages { get; private set; }
    public IEnumerable<T> Items { get; private set; }

    public Pager(int totalItems, int currentPage, int pageSize, IEnumerable<T> items)
    {
        TotalItems = totalItems;
        CurrentPage = currentPage;
        PageSize = pageSize;
        TotalPages = GetTotalPages();
        Items = items;
    }

    public int GetTotalPages()
    {
        return (int)Math.Ceiling(TotalItems / (float)PageSize);
    }
}
