using Application.DTOs;
using AutoMapper;

namespace Application.Helpers
{
    public static class PaginationHelper
    {
        public static PaginationResponseDTO<TDestination> Paginate<TSource, TDestination>(
            IEnumerable<TSource> source,
            PaginationRequestDTO pagination,
            IMapper mapper)
        {
            var paginated = source
                .Skip((pagination.Page - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToList();

            return new PaginationResponseDTO<TDestination>
            {
                Page = pagination.Page,
                PageSize = pagination.PageSize,
                TotalCount = source.Count(),
                Data = mapper.Map<List<TDestination>>(paginated)
            };
        }
    }
}
