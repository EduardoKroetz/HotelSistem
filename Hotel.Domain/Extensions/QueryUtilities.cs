using Hotel.Domain.Entities.Base;

namespace Hotel.Domain.Extensions;
public static class QueryUtility
{
  public static IQueryable<T> FilterCreatedAtOperator<T>(this IQueryable<T> query, string filterOperator, DateTime createdAt)
      where T : Entity
  {
    switch (filterOperator.ToLower())
    {
      case "gt": // Greater than
      query = query.Where(x => x.CreatedAt.Date > createdAt.Date);
      break;
      case "lt": // Less than
      query = query.Where(x => x.CreatedAt.Date < createdAt.Date);
      break;
      case "eq": // Equal to
      query = query.Where(x => x.CreatedAt.Date == createdAt.Date);
      break;
    }

    return query;
  }

  public static IQueryable<T> FilterOrderByCreatedAtOperator<T>(this IQueryable<T> query, string filterOperator)
    where T : Entity
  {
    switch (filterOperator.ToLower())
    {
      case "asc": // Ascending
        query = query.OrderBy(x => x.CreatedAt);
        break;
      case "desc": // Descending
        query = query.OrderByDescending(x => x.CreatedAt);
        break;
    }

    return query;
  }
}

