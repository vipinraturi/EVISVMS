using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Evis.VMS.UI.ViewModel
{
    public class VisitorsDetailsVM
    {
        public long VisitorId { get; set; }

        public string VisitorName { get; set; }

        public DateTime VisitDate { get; set; }

        public DateTime CheckIn { get; set; }

        public DateTime? CheckOut { get; set; }

        public string ContactNumber { get; set; }

        public string VisitDetails { get; set; }

        public int BuildingId { get; set; }

        public int GateId { get; set; }

        public string SecurityId { get; set; }
       
    }
}