using AutoMapper;
using LibraryManagementSystem.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.ViewModels
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int) Math.Ceiling(count / (double) pageSize);
            this.AddRange(items);
        }

        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;

        public async static Task<PaginatedList<T>> CreateAsync(IQueryable<BaseEntity> source, int pageIndex, int pageSize, IMapper mapper)
        {
            var count  = source.Count();

            var items = await source
                                .Skip((pageIndex - 1) * pageSize)
                                .Take(pageSize)
                                .Select(_ => mapper.Map<T>(_))
                                .ToListAsync();

            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }

    }
}
