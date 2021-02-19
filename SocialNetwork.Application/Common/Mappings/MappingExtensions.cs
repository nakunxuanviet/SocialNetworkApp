using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Application.Common.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetwork.Application.Common.Mappings
{
    public static class MappingExtensions
    {
        /// <summary>
        /// Converts queryable to a paginated list.
        /// </summary>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static Task<PaginatedList<TDestination>> PaginatedListAsync<TDestination>(this IQueryable<TDestination> queryable, int pageNumber, int pageSize)
            => PaginatedList<TDestination>.CreateAsync(queryable, pageNumber, pageSize);

        /// <summary>
        /// Converts the queryable to a list and maps it automatically to the declared object.
        /// </summary>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static Task<List<TDestination>> ProjectToListAsync<TDestination>(this IQueryable queryable, IConfigurationProvider configuration)
            => queryable.ProjectTo<TDestination>(configuration).ToListAsync();
    }
}