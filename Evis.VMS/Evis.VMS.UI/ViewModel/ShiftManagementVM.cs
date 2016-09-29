using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Evis.VMS.UI.ViewModel
{

    public class ShiftManagementRequest
    {
        public int BuildingId { get; set; }
        public int GateId { get; set; }
        public string ShiftDate { get; set; }
        public int NoOfDays { get; set; }
    }

    public class ShiftManagementResponseVM
    {
        public ShiftHeaders Header { get; set; }   
        public IList<ShiftManagementVM> Body { get; set; }   
    }

    public class ShiftManagementVM
    {
        public string Securityname { get; set; }
        public string BuldingName { get; set; }
        public string GateName { get; set; }
        public List<ShiftManagemetDates> ShiftManageDates { get; set; }
    }

    public class ShiftManagemetDates
    {
        public DateTime ShiftDate { get; set; }
        public List<ShiftDatesShifts> ShiftManage { get; set; }

    }

    public class ShiftDatesShifts
    {
        public int ShiftId { get; set; }
        public string ShiftName { get; set; }
    }

    public class ShiftHeaders
    {
        public string ShiftMainDates { get; set; }
        public List<string> ListAllDates { get; set; }
    }

   
}