using System.Linq.Expressions;

namespace PortfolioManager.Application.Contracts;

public class PaginatedList<T>(List<T> items, int count, int pageIndex, int pageSize)
{
    public List<T> Items { get; private set; } = items;
    public int PageIndex { get; private set; } = pageIndex;
    public int TotalPages { get; private set; } = (int)Math.Ceiling(count / (double)pageSize);
    public int TotalItems { get; private set; } = count;

    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex < TotalPages;

    public static PaginatedList<T> Create(IQueryable<T> source, int pageIndex, int pageSize)
    {
        var count = source.Count();
        var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

        return new PaginatedList<T>(items, count, pageIndex, pageSize);
    }

    public static PaginatedList<TResult> CreateMapped<TResult>(
        IQueryable<T> source, int pageIndex, int pageSize, Expression<Func<T, TResult>> selector)
    {
        var count = source.Count();
        var items = source.Select(selector)
                          .Skip((pageIndex - 1) * pageSize)
                          .Take(pageSize)
                          .ToList(); 

        return new PaginatedList<TResult>(items, count, pageIndex, pageSize);
    }
}