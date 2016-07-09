using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Evis.VMS.Utilities
{
    public enum SortDirection
    {
        Ascending,
        Descending
    }

    public class PaginationRequest
    {
        public string SearchText { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public Sort Sort { get; set; }
    }

    public class Sort
    {
        public string SortBy { get; set; }
        public SortDirection SortDirection { get; set; }
    }
}
