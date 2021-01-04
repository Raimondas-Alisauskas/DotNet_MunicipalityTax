namespace MunicipalityTax.Services.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class PagingHelper<T> : List<T>
    {
        public int CurrentPage { get; private set; }

        public int TotalPages { get; private set; }

        public int PageSize { get; private set; }

        public int TotalCount { get; private set; }

        public bool HasPrevious => this.CurrentPage > 1;

        public bool HasNext => this.CurrentPage < this.TotalPages;

        public PagingHelper(List<T> items, int count, int pageNumber, int pageSize)
        {
            this.TotalCount = count;
            this.PageSize = pageSize;
            this.CurrentPage = pageNumber;
            this.TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            this.AddRange(items);
        }

        public static PagingHelper<T> MakePage(IEnumerable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new PagingHelper<T>(items, count, pageNumber, pageSize);
        }
    }
}