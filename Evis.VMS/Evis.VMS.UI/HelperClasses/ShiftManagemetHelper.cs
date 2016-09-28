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
        public IList<ShiftManagementVM> GetShiftData(DateTime fromDate,DateTime toDate, int buldingId, int gateId)
        {
            var userDataWithShift=_genericService.ShiftDetails.GetAll().Where(x=>x.GateID==gateId)
                .Select(x=> new ShiftManagementVM
                {
                    Securityname=x.ApplicationUser.UserName,
                    ShiftManageDates=GetShiftWthDate(fromDate,toDate,x.ApplicationUser.UserName)
                }).ToList();


            return userDataWithShift;
        }

        private List<ShiftManagemetDates> GetShiftWthDate(DateTime fromDate, DateTime toDate, string securityName)
        {
            var datesWithShift = _genericService.ShiftDetails.GetAll().Where(x => x.ApplicationUser.UserName == securityName && x.ShiftDate >= fromDate && x.ShiftDate <= toDate)
                .Select(x => new ShiftManagemetDates
            {
                ShiftDate=x.ShiftDate,
                ShiftManage=ListAllShifts(x.ShiftDate,x.ApplicationUser.UserName)
               

            }).ToList();

            return datesWithShift; 
        }

        private List<ShiftDatesShifts> ListAllShifts(DateTime shiftDate, string securityName)
        {

            var listAllShiftsOneDay = _genericService.ShiftDetails.GetAll().Where(x => x.ApplicationUser.UserName == securityName && x.ShiftDate == shiftDate)
                .Select(x=> new ShiftDatesShifts
                {
                    ShiftId=x.ShiftID
                }).ToList();
            return listAllShiftsOneDay;
        }
    }
}