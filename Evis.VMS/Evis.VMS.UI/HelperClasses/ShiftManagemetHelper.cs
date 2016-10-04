using Evis.VMS.Business;
using Evis.VMS.Data.Model.Entities;
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
            var userDataWithShift = _genericService.ShiftDetails.GetAll().Where(x => (x.GateID == gateId || gateId == -1) && x.IsActive)
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
                    UserProfilePicPath = x.UserProfilePicPath,
                    IsImageAvailable = System.IO.File.Exists(HttpContext.Current.Server.MapPath("\\") + "" + x.UserProfilePicPath.Replace("/", "\\"))
                }).ToList();


            return result;
        }

        private string GetShiftName(string shiftName)
        {
            if (shiftName.Length >= 10)
            {
                return shiftName.Substring(0, 8) + "..";
            }
            return shiftName;
        }

        private List<ShiftDetails_Shift> GetShiftDetails_Shift(DateTime shiftDate, DateTime toDate, string userId, int gateId)
        {
            var lstShiftDetails = new List<ShiftDetails_Shift>();

            var lstDates = new List<string>();
            var lstShiftsAssigned = new List<bool>();
            var lstShiftsAssignedCell = new List<ShiftsAssignedCell>();


            int counter = new Random().Next(1, 9999);//This should be unique number for checking or unchecking the checkboxed based on id

            foreach (int shiftId in GetAllShiftIds(-1))
            {
                lstDates = new List<string>();
                lstShiftsAssigned = new List<bool>();
                lstShiftsAssignedCell = new List<ShiftsAssignedCell>();
                var shift = _genericService.ShitfMaster.GetById(shiftId);

                counter = counter + 1;

                DateTime tempShiftAssignmentDate = DateTime.MinValue;
                for (DateTime startDate = shiftDate; startDate <= toDate; startDate = startDate.AddDays(1))
                {
                    counter = counter + 1;
                    var isShiftAssignsed = _genericService.ShiftDetails.GetAll()
                                       .Where(item => item.GateID == gateId && item.SecurityID == userId && item.ShiftID == shiftId
                                       && item.IsActive)
                                       .AsQueryable();

                    var isShiftAssigned = isShiftAssignsed.Where(item => item.ShiftDate == startDate).FirstOrDefault();
                    //TODO vipin
                    // 

                    if (isShiftAssigned != null)
                    {
                        lstShiftsAssigned.Add(true);
                        lstShiftsAssignedCell.Add(new ShiftsAssignedCell { Id = new Random().Next(10000, 99999) + counter, IsAssigned = true, ShiftDate = startDate, ShiftId = shiftId, ShiftName = shift.ShitfName, UserId = userId, GateId = gateId });
                    }
                    else
                    {
                        lstShiftsAssignedCell.Add(new ShiftsAssignedCell { Id = new Random().Next(100000, 999999) + counter, IsAssigned = false, ShiftDate = startDate, ShiftId = shiftId, ShiftName = shift.ShitfName, UserId = userId, GateId = gateId });
                        lstShiftsAssigned.Add(false);
                    }

                    lstDates.Add(startDate.ToString("ddd MMM dd"));
                }

                lstShiftDetails.Add(new ShiftDetails_Shift { ShiftName = shift.ShitfName, ShiftDates = lstDates, ShiftsAssigned = lstShiftsAssigned, ShiftsAssignedCells = lstShiftsAssignedCell });
            }

            return lstShiftDetails;
        }

        private List<string> GetAllShifts(int gateId)
        {
            var shifts = new List<string>();

            _genericService.ShitfMaster.GetAll().ToList().ForEach(item =>
            {
                shifts.Add(GetShiftName(item.ShitfName) + " (" + item.FromTime.ToString("h:mm tt") + "-" + item.ToTime.ToString("h:mm tt") + ")");
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
                .Where(x => x.ApplicationUser.UserName == securityUserName && x.ShiftDate >= fromDate && x.ShiftDate <= toDate && x.IsActive)
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

        public ReturnResult ApplyShiftAssignmentChanges(IList<ShiftAssignmentChanges> request)
        {
            var returnResult = new ReturnResult();
            request.ToList().ForEach(item =>
            {
                if (item.IsAssigned)
                {
                    var shiftAssignmentChange = new ShiftDetails { Id = 0, IsActive = true, GateID = item.GateId, ShiftDate = item.ShiftDate, ShiftID = item.ShiftId, SecurityID = item.UserId };
                    _genericService.ShiftDetails.Insert(shiftAssignmentChange);
                }
                else
                {
                    var shiftAssignmentChange = _genericService.ShiftDetails.GetAll()
                         .FirstOrDefault(item_db => item_db.ShiftID == item.ShiftId && item_db.SecurityID == item.UserId
                             && item_db.ShiftDate == item.ShiftDate && item_db.IsActive
                        );

                    var shiftAssignmentChangeLst = _genericService.ShiftDetails.GetAll()
                     .Where(item_db => item_db.ShiftID == item.ShiftId && item_db.SecurityID == item.UserId && item_db.IsActive
                    );

                    if (shiftAssignmentChangeLst != null)
                    {
                        if (shiftAssignmentChangeLst.Count() == 1)
                        {
                            var shiftAssignmentObj = _genericService.ShitfAssignment.GetAll().FirstOrDefault(item_db => item_db.ShitfId == shiftAssignmentChange.ShiftID
                                && item_db.UserId == shiftAssignmentChange.SecurityID && item_db.IsActive);

                            if (shiftAssignmentObj != null)
                            {
                                shiftAssignmentObj.IsActive = false;
                                _genericService.ShitfAssignment.Update(shiftAssignmentObj);
                            }
                        }
                    }


                    if (shiftAssignmentChange != null)
                    {
                        shiftAssignmentChange.IsActive = false;
                        _genericService.ShiftDetails.Update(shiftAssignmentChange);
                    }
                }
            });

            _genericService.Commit();
            returnResult.Success = true;
            return returnResult;

        }
    }
}