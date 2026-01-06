using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace backend.Core.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> AsPage<T>(this IOrderedQueryable<T> queryable, IPageable page)
        {
            int entries = (page.Page - 1) * page.PageSize;
            return queryable.Skip(entries).Take(page.PageSize);
        }

        public static PageResult<TOut> PageResponse<T, TOut>(this IOrderedQueryable<T> queryable, IPageable page, Expression<Func<T, TOut>> selector)
        {
            int count = queryable.Count();
            TOut[] results = queryable.AsPage(page)
                .Select(selector)
                .ToArray();

            return new PageResult<TOut>(results, count);
        }
    }
}
