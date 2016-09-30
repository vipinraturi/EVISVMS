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
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserProfilePicPath { get; set; }
        public string BuldingName { get; set; }
        public string GateName { get; set; }
        public int GateId { get; set; }
        public List<ShiftDetails_Shift> ShiftDetails_Shift { get; set; }

        public List<string> Shifts { get; set; }
       // public List<ShiftDetails_PerShift> ShiftDetails_PerShift { get; set; }
    }

    public class ShiftDetails_Shift
    {
        public string ShiftName { get; set; }
        public List<string> ShiftDates { get; set; }
        public List<bool> ShiftsAssigned { get; set; }
    }

    

    public class ShiftDetails_PerShift
    {
        public string ShiftName { get; set; }
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