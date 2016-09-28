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
            var allDates=new  List<string>();
            for (DateTime startDate = fromDate; startDate <= todate;startDate= startDate.AddDays(1))
            {
                allDates.Add(startDate.ToString("ddd MMM dd"));
            }
            return allDates;

        }
        public IList<ShiftManagementVM> GetShiftData(DateTime fromDate, DateTime toDate, int buldingId, int gateId)
        {
            var userDataWithShift = _genericService.ShiftDetails.GetAll().Where(x => x.GateID == gateId)
                .Select(x => new ShiftManagementVM
                {
                    Securityname = x.ApplicationUser.FullName,
                    GateName=x.Gates.GateNumber,
                    BuldingName=x.Gates.BuildingMaster.BuildingName
                }).Distinct().ToList();

            var result = userDataWithShift.AsEnumerable().Select(x => new ShiftManagementVM
                {
                    Securityname = x.Securityname,
                    GateName=x.GateName,
                    BuldingName=x.BuldingName,
                    ShiftManageDates = GetShiftWthDate(fromDate, toDate, x.Securityname)
                }).ToList();
            return result;
        }

        private List<ShiftManagemetDates> GetShiftWthDate(DateTime fromDate, DateTime toDate, string securityName)
        {
            var datesWithShift = _genericService.ShiftDetails.GetAll().Where(x => x.ApplicationUser.UserName == securityName && x.ShiftDate >= fromDate && x.ShiftDate <= toDate)
                .Select(x => new ShiftManagemetDates
            {
                ShiftDate = x.ShiftDate



            }).Distinct().ToList();
            var result = datesWithShift.AsEnumerable().Select(x => new ShiftManagemetDates
           {
               ShiftDate=x.ShiftDate,
               
               ShiftManage = ListAllShifts(x.ShiftDate,securityName)
           }).ToList();
            return result;
        }

        private List<ShiftDatesShifts> ListAllShifts(DateTime shiftDate, string securityName)
        {

            var listAllShiftsOneDay = _genericService.ShiftDetails.GetAll().Where(x => x.ApplicationUser.UserName == securityName && x.ShiftDate == shiftDate)
                .Select(x => new ShiftDatesShifts
                {
                    ShiftId = x.ShiftID,
                    ShiftName=x.Shitfs.ShitfName
                }).Distinct().ToList();
            return listAllShiftsOneDay;
        }
    }
}