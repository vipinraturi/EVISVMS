using Evis.VMS.Business;
using Evis.VMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Evis.VMS.UI.HelperClasses
{
    public class ShiftManagemetHelper
    {

        GenericService _genericService = null;
        public ShiftManagemetHelper()
        {
            _genericService = new GenericService();
        }
        public ShiftHeaders GetHeaders(DateTime fromDate, DateTime todate)
        {
            ShiftHeaders shiftHeader = new ShiftHeaders();
            shiftHeader.ShiftMainDates = fromDate.ToString("MMM d yyyy") + " - " + todate.ToString("MMM d yyyy");
            shiftHeader.ListAllDates = GetAllDate(fromDate, todate);
            return shiftHeader;
        }

        private List<string> GetAllDate(DateTime fromDate, DateTime todate)
        {
            var allDates = new List<string>();
            for (DateTime startDate = fromDate; startDate <= todate; startDate = startDate.AddDays(1))
            {
                allDates.Add(startDate.ToString("ddd MMM dd"));
            }
            return allDates;

        }
        public IList<ShiftManagementVM> GetShiftData(DateTime fromDate, DateTime toDate, int buldingId, int gateId)
        {
            var userDataWithShift = _genericService.ShiftDetails.GetAll().Where(x => (x.GateID == gateId || gateId == -1))
                .Select(x => new ShiftManagementVM
                {
                    Securityname = x.ApplicationUser.FullName,
                    UserName = x.ApplicationUser.UserName,
                    GateName = x.Gates.GateNumber,
                    GateId = x.GateID,
                    BuldingName = x.Gates.BuildingMaster.BuildingName
                }).Distinct().ToList();

            var result = userDataWithShift.AsEnumerable().Select(x => new ShiftManagementVM
                {
                    Securityname = x.Securityname,
                    UserName = x.UserName,
                    GateName = x.GateName,
                    BuldingName = x.BuldingName,
                    Shifts = GetAllShifts(x.GateId),
                    //ShiftDetails_PerShift = GetShiftWthDate(fromDate, toDate, x.UserName),
                    ShiftDetails_Shift = GetShiftDetails_Shift(fromDate, toDate)
                }).ToList();


            return result;
        }

        private List<ShiftDetails_Shift> GetShiftDetails_Shift(DateTime fromDate, DateTime toDate)
        {
            var lstShiftDetails = new List<ShiftDetails_Shift>();


            List<string> lstDates = new List<string>();
            for (DateTime startDate = fromDate; startDate <= toDate; startDate = startDate.AddDays(1))
            {
                lstDates.Add(startDate.ToString("ddd MMM dd"));
            }

            foreach (string shiftName in GetAllShifts(0))
            {
                lstShiftDetails.Add(new ShiftDetails_Shift { ShiftName = shiftName, ShiftDates = lstDates });
            }

            return lstShiftDetails;
        }

        private List<string> GetAllShifts(int gateId)
        {
            var shifts = new List<string>();

             _genericService.ShitfMaster.GetAll().ToList().ForEach(item => {
                 shifts.Add(item.ShitfName + " (" + item.FromTime.ToString("h:mm tt") + "-" + item.ToTime.ToString("h:mm tt") + ")");
             });

            return shifts;
        }

        private List<ShiftDetails_PerShift> GetShiftWthDate(DateTime fromDate, DateTime toDate, string securityUserName)
        {
            var datesWithShift =
                _genericService.ShiftDetails.GetAll()
                .Where(x => x.ApplicationUser.UserName == securityUserName && x.ShiftDate >= fromDate && x.ShiftDate <= toDate)
                .Select(x => new ShiftDetails_PerShift
            {
                ShiftDate = x.ShiftDate

            }).Distinct().ToList();

            var result = datesWithShift.AsEnumerable().Select(x => new ShiftDetails_PerShift
           {
               ShiftDate = x.ShiftDate,

               ShiftManage = ListAllShifts(x.ShiftDate, securityUserName)
           }).ToList();
            return result;


        }

        private List<ShiftDatesShifts> ListAllShifts(DateTime shiftDate, string securityUserName)
        {

            var listAllShiftsOneDay = _genericService.ShiftDetails.GetAll().Where(x => x.ApplicationUser.UserName == securityUserName && x.ShiftDate == shiftDate)
                .Select(x => new ShiftDatesShifts
                {
                    ShiftId = x.ShiftID,
                    ShiftName = x.Shitfs.ShitfName
                }).Distinct().ToList();
            return listAllShiftsOneDay;
        }
    }
}