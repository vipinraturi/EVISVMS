using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Evis.VMS.UI.ViewModel
{
    public class ShiftManagementVM
    {
        public int SecurityId { get; set; }
        public int BuldingId { get; set; }
        public int GateId { get; set; }
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
    }
}