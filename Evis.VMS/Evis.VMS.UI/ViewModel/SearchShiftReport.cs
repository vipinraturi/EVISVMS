using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Evis.VMS.UI.ViewModel
{
    public class SearchShiftReport
    {
        public string SecurityId { get; set; }
        public int BuildingID { get; set; }
        public int GateId { get; set; }
        public int ShiftID { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

    }
}