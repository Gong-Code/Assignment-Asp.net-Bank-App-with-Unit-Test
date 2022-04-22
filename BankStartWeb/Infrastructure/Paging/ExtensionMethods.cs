using FirstWebApp.Infrastructure.Paging;
using System.Linq.Expressions;

public static class ExtensionMethods
{
    public static PagedResult<T> GetPaged<T>(this IQueryable<T> query,
        int page, int pageSize) where T : class
    {
        var result = new PagedResult<T>();
        result.CurrentPage = page;
        result.PageSize = pageSize;
        result.RowCount = query.Count();


        var pageCount = (double)result.RowCount / pageSize;
        result.PageCount = (int)Math.Ceiling(pageCount);

        var skip = (page - 1) * pageSize;
        result.Results = query.Skip(skip).Take(pageSize).ToList();

        return result;
    }


    public enum QuerySortOrder
    {
        Asc,
        Desc
    }
    public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName, QuerySortOrder sortOrder)
    {
        if (sortOrder == QuerySortOrder.Asc)
            return source.OrderBy(ToLambda<T>(propertyName));
        return source.OrderByDescending(ToLambda<T>(propertyName));
    }


    private static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName)
    {
        return source.OrderBy(ToLambda<T>(propertyName));
    }

    private static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string propertyName)
    {
        return source.OrderByDescending(ToLambda<T>(propertyName));
    }

    private static Expression<Func<T, object>> ToLambda<T>(string propertyName)
    {
        var parameter = Expression.Parameter(typeof(T));
        var property = Expression.Property(parameter, propertyName);
        var propAsObject = Expression.Convert(property, typeof(object));

        return Expression.Lambda<Func<T, object>>(propAsObject, parameter);
    }
}