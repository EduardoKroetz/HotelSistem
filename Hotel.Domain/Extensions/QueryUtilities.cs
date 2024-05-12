using Hotel.Domain.DTOs.Base;
using Hotel.Domain.Entities.Base;
using System.Linq.Expressions;

namespace Hotel.Domain.Extensions;
public static class QueryUtility
{
  public static IQueryable<T> FilterByOperator<T,TProperty>(this IQueryable<T> query, string? filterOperator, Expression<Func<T,TProperty>> propertySelector,TProperty value)
      where T : Entity
  {
    if (filterOperator == null)
      return query;
   
    if (value == null)
    {
      switch (filterOperator.ToLower())
      {
        case "asc": //Ascending
        return query.OrderBy(
          Expression.Lambda<Func<T, object>>(
            propertySelector.Body,
            propertySelector.Parameters[0])
        );
        case "desc": //Descending
        return query.OrderByDescending(
          Expression.Lambda<Func<T, object>>(
            propertySelector.Body,
            propertySelector.Parameters[0])
        );
      }
    }else
    {
      switch (filterOperator.ToLower())
      {
        case "gt": // Greater than
        return query.Where(
          Expression.Lambda<Func<T, bool>>(
            Expression.GreaterThan(
              propertySelector.Body,
              Expression.Constant(value)),
            propertySelector.Parameters)
          );
        case "lt": // Less than
        return query.Where(
          Expression.Lambda<Func<T, bool>>(
            Expression.LessThan(
              propertySelector.Body,
              Expression.Constant(value)),
            propertySelector.Parameters)
        );
        case "eq": // Equal to
        return query.Where(
          Expression.Lambda<Func<T, bool>>(
            Expression.Equal(
              propertySelector.Body,
              Expression.Constant(value)),
            propertySelector.Parameters)
        );
      }
    }

    
    return query;
  }

  public static IQueryable<T> BaseQuery<T>(this IQueryable<T> query, QueryParameters queryParameters)
    where T : Entity
  {
    query = query.FilterByOperator(queryParameters.CreatedAtOperator,x => x.CreatedAt,queryParameters.CreatedAt);
    query = query.Skip(queryParameters.Skip ?? 0).Take(queryParameters.Take ?? 1);

    return query;
  }

}

