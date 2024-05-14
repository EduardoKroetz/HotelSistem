using Hotel.Domain.DTOs.Base;
using Hotel.Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;
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
        return query.OrderBy(propertySelector);
        case "desc": //Descending
        return query.OrderByDescending(propertySelector);
      }
    }else
    {
      switch (filterOperator.ToLower())
      {
        case "gt": // Greater than    
          return query.Where(
            Expression.Lambda<Func<T, bool>>(
              Expression.GreaterThan(
                  Expression.Property(propertySelector.Body,"Value"),
                  Expression.Constant(value)),
              propertySelector.Parameters)
           );
        case "lt": // Less than
        return query.Where(
          Expression.Lambda<Func<T, bool>>(
            Expression.LessThan(
              Expression.Property(propertySelector.Body, "Value"),
              Expression.Constant(value)),
            propertySelector.Parameters)
        );
        case "eq": // Equal to
        return query.Where(
          Expression.Lambda<Func<T, bool>>(
            Expression.Equal(
              Expression.Property(propertySelector.Body,"Value"),
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
    query = query.AsNoTracking();

    return query;
  }

}

