using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Evis.VMS.UI.ViewModel
{
    public class ShiftReportGridVM
    {
        public string SecurityName { get;set; }
        public string  BuildingName{get;set;}
        public string Gate { get; set; }
        public string ShiftName{get;set;}
        public string ShiftDates { get; set; }
        public DateTime FromDate { get; set; }
       // public DateTime ToDate { get; set; }
        public string strFromDate { get; set; }
        public string strToDate { get; set; }
        public DateTime Fromtime { get; set; }
        public DateTime Totime { get; set; }
        public string UserName { get; set; }
        public string CompanyName { get; set; }

        
       
    }
}