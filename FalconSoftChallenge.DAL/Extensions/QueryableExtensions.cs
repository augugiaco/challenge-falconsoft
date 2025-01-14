using FalconSoftChallenge.DAL.DTO;
using Microsoft.EntityFrameworkCore;

namespace FalconSoftChallenge.DAL.Extensions
{
    public static class QueryableExtensions
    {
        public static async Task<PagedResultDTO<C>> Paginate<T, C>(this IQueryable<T> query, int page, int size, Func<T, C> projection) 
            where T : class 
            where C : class
        {
            var count = await query.CountAsync();

            page = page < 1 ? 1 : page;
            size = size < 1 ? 10 : size;

            var data = await query
                        .Skip((page - 1) * size)
                        .Take(size)
                        .Select(dd => projection(dd))
                        .ToListAsync();

            var totalPagesCount =  count > size ? count / size : 1;

            var result = new PagedResultDTO<C>(data, count, page, size, totalPagesCount);

            return result;
        }
    }
}
