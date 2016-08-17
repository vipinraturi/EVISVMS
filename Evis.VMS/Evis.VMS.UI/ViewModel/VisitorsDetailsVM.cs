using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Evis.VMS.UI.ViewModel
{
    public class VisitorsDetailsVM : SearchVisitorVM
    {
        public long VisitorId { get; set; }

        public string ContactNumber { get; set; }

        public string VisitDetails { get; set; }

        public string Building { get; set; }

        public string Gate { get; set; }

        public string Security { get; set; }

        public string CompanyName { get; set; }
    }

    public class SearchVisitorVM
    {
        public SearchVisitorVM()
        {
            SecurityId = VisitorName = string.Empty;
        }
        public int BuildingId { get; set; }

        public int GateId { get; set; }

        public string SecurityId { get; set; }

        public string VisitorName { get; set; }

        public string CheckIn { get; set; }

        public string CheckOut { get; set; }

    }
}