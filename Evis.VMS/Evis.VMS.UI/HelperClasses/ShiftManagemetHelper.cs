﻿using Evis.VMS.Business;
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
            var shiftHeader = new ShiftHeaders();
            shiftHeader.ShiftMainDates = fromDate.ToString("MMM d yyyy") + "-" + todate.ToString("MMM d yyyy");
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
                    UserId = x.ApplicationUser.Id,
                    GateName = x.Gates.GateNumber,
                    GateId = x.GateID,
                    BuldingName = x.Gates.BuildingMaster.BuildingName,
                    UserProfilePicPath = x.ApplicationUser.ProfilePicturePath
                }).Distinct().ToList();

            var result = userDataWithShift.AsEnumerable().Select(x => new ShiftManagementVM
                {
                    Securityname = x.Securityname,
                    UserName = x.UserName,
                    GateName = x.GateName,
                    BuldingName = x.BuldingName,
                    Shifts = GetAllShifts(x.GateId),
                    ShiftDetails_Shift = GetShiftDetails_Shift(fromDate, toDate, x.UserId, x.GateId),
                    UserProfilePicPath = x.UserProfilePicPath
                }).ToList();


            return result;
        }

        private List<ShiftDetails_Shift> GetShiftDetails_Shift(DateTime shiftDate, DateTime toDate, string userId, int gateId)
        {
            var lstShiftDetails = new List<ShiftDetails_Shift>();

            var lstDates = new List<string>();
            var lstShiftsAssigned = new List<bool>();

            foreach (int shiftId in GetAllShiftIds(-1))
            {
                 lstDates = new List<string>();
                 lstShiftsAssigned = new List<bool>();


                for (DateTime startDate = shiftDate; startDate <= toDate; startDate = startDate.AddDays(1))
                {


                    var isShiftAssigned = _genericService.ShitfAssignment.GetAll()
                                       .Where(item => item.GateId == gateId && item.UserId == userId && item.ShitfId == shiftId)
                                       .FirstOrDefault();

                    // && item.FromDate >= shiftDate && item.ToDate <= shiftDate

                    if (isShiftAssigned != null)
                    {
                        lstShiftsAssigned.Add(true);
                    }
                    else
                    {
                        lstShiftsAssigned.Add(false);
                    }

                    lstDates.Add(startDate.ToString("ddd MMM dd"));
                }

                var shift = _genericService.ShitfMaster.GetById(shiftId);
                lstShiftDetails.Add(new ShiftDetails_Shift { ShiftName = shift.ShitfName, ShiftDates = lstDates, ShiftsAssigned = lstShiftsAssigned });
            }

            return lstShiftDetails;
        }

        private List<string> GetAllShifts(int gateId)
        {
            var shifts = new List<string>();

            _genericService.ShitfMaster.GetAll().ToList().ForEach(item =>
            {
                shifts.Add(item.ShitfName + " (" + item.FromTime.ToString("h:mm tt") + "-" + item.ToTime.ToString("h:mm tt") + ")");
            });

            return shifts;
        }

        private List<int> GetAllShiftIds(int gateId)
        {
            var shifts = new List<int>();

            _genericService.ShitfMaster.GetAll().ToList().ForEach(item =>
            {
                shifts.Add(item.Id);
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