using System;
using System.Collections.Generic;
using System.Linq;

namespace BillyGoats.Api.Filter
{
    public class ListData<T>
    {
        public IQueryable<T> RowData { get; set; }

        public long TotalItems { get; set; }

        public long CurrentItemCount { get; set; }

        public long Page { get; set; }

        public long PageSize { get; set; }

        public string NextPage { get; set; }

        public string PrevPage { get; set; }

        public long TotalPages
        {
            get
            {
                return PageSize != 0 ? TotalItems / PageSize + Convert.ToInt32(TotalItems % PageSize != 0) : 1;
            }
        }
    }

    public class ListData2<T>
    {
        public ICollection<T> RowData { get; set; }

        public long TotalItems { get; set; }

        public long CurrentItemCount { get; set; }

        public long Page { get; set; }

        public long PageSize { get; set; }

        public string NextPage { get; set; }

        public string PrevPage { get; set; }

        public long TotalPages
        {
            get
            {
                return PageSize != 0 ? TotalItems / PageSize + Convert.ToInt32(TotalItems % PageSize != 0) : 1;
            }
        }
    }
}
