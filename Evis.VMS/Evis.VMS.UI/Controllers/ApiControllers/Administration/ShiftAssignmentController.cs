/********************************************************************************
 * Company Name : East Vision Information System
 * Team Name    : EVIS IT Team
 * Author       : Vipin Raturi
 * Created On   : 06/05/2016
 * Description  : 
 *******************************************************************************/

using Evis.VMS.Data.Model.Entities;
using Evis.VMS.UI.HelperClasses;
using Evis.VMS.UI.ViewModel;
using Evis.VMS.Utilities;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Globalization;
using System.Data.Entity.SqlServer;
using System.Data.Entity;


namespace Evis.VMS.UI.Controllers.ApiControllers
{
    public partial class AdministrationController
    {

        IList<ShiftAssignmentVM> LSTShiftAssignmentVM;
        [Route("~/Api/ShiftAssignment/GetAllGates")]
        [HttpGet]
        public IEnumerable<GeneralDropDownVM> GetAllGates(int BuildingId)
        {
            var result = _genericService.GateMaster.GetAll().Where(x => x.IsActive == true & x.BuildingId == BuildingId)
                .Select(y => new GeneralDropDownVM { Id = y.Id, Name = y.GateNumber });
            return result.OrderByDescending(x => x.Id);
        }

        [Route("~/Api/ShiftAssignment/GetAllShift")]
        [HttpGet]
        public IEnumerable<GeneralDropDownVM> GetAllShift()
        {
            //TODO-- organization check needed
            var result = _genericService.ShitfMaster.GetAll().Where(x => x.IsActive == true).AsEnumerable()
                .Select(y => new GeneralDropDownVM { Id = y.Id, Name = y.ShitfName + " (" + y.FromTime.ToString("hh:mm tt") + " - " + y.ToTime.ToString("hh:mm tt") + ")" });////y.ShitfName + '(' + ' ' + y.FromTime + ' ' + y.ToTime + ')'

            return result.OrderByDescending(x => x.Id);
        }

        [Route("~/Api/ShiftAssignment/GetAllUsers")]
        [HttpGet]
        public async Task<IEnumerable<DropDownVM>> GetAllUsers(int GateId)
        {
            IEnumerable<DropDownVM> result;
            var user = (await _userService.GetAllAsync()).Where(x => x.Id == HttpContext.Current.User.Identity.GetUserId() && x.IsActive == true).FirstOrDefault();
            if (user == null)
            {
                var gates = _genericService.GateMaster.GetAll().FirstOrDefault(x => x.Id == GateId && x.IsActive == true);
                var building = _genericService.BuildingMaster.GetAll().Where(x => x.Id == gates.BuildingId).FirstOrDefault();
                var getUsers = (await _userService.GetAllAsync()).Where(x => x.Organization.IsActive == true &&
                               (user == null || (user != null && x.OrganizationId == user.OrganizationId))).AsQueryable();
                var getRoles = (await _applicationRoleService.GetAllAsync()).AsQueryable();
                result = (from users in getUsers
                          join roles in getRoles
                          on users.Roles.First().RoleId equals roles.Id
                          where (user == null || (user != null && user.OrganizationId == building.OrganizationId)) && roles.Name == "Security"
                          select new DropDownVM
                          {
                              Id = users.Id,
                              Name = users.FullName
                          }).AsQueryable();
            }
            else
            {
                var gates = _genericService.GateMaster.GetAll().FirstOrDefault(x => x.Id == GateId && x.IsActive == true);
                var building = _genericService.BuildingMaster.GetAll().Where(x => x.Id == gates.BuildingId).FirstOrDefault();
                var getUsers = (await _userService.GetAllAsync()).Where(x => x.Organization.IsActive == true &&
                               (user == null || (user != null && x.OrganizationId == user.OrganizationId))).AsQueryable();

                var getRoles = (await _applicationRoleService.GetAllAsync()).AsQueryable();
                result = (from users in getUsers
                          join roles in getRoles
                          on users.Roles.First().RoleId equals roles.Id
                          where (user == null || (user != null && user.OrganizationId == building.OrganizationId)) && roles.Name == "Security"
                          select new DropDownVM
                          {
                              Id = users.Id,
                              Name = users.FullName
                          }).AsQueryable();
            }
            return result;
        }

        [Route("~/Api/ShiftAssignment/GetAllShiftAssignment")]
        [HttpPost]
        public async Task<string> GetAllShiftAssignment(string globalSearch, int pageIndex, int pageSize, string sortField = "", string sortOrder = "ASC")
        {
            IList<ShiftAssignmentVM> LSTShiftAssignmentVMTemp = null;
            LSTShiftAssignmentVM = new List<ShiftAssignmentVM>();

            var user = (await _userService.GetAllAsync()).Where(x => x.Id == HttpContext.Current.User.Identity.GetUserId() && x.IsActive == true).FirstOrDefault();
            if (user == null)
            {
                LSTShiftAssignmentVMTemp = _genericService.ShitfAssignment.GetAll().Where(x => x.IsActive == true).ToList()
                        .Select(x => new ShiftAssignmentVM
                        {
                            BuildingId = x.BuildingId,
                            BuildingName = x.Gates.BuildingMaster.BuildingName,
                            GateId = x.GateId,
                            GateName = x.Gates.GateNumber,
                            ShitfId = x.ShitfId,
                            ShiftName = x.Shitfs.ShitfName,
                            UserId = x.UserId,
                            UserName = x.ApplicationUser.FullName,
                            FromDate = x.FromDate,
                            ToDate = x.ToDate,
                            strFromDate = x.FromDate.ToString("dd/MM/yyyy"),
                            strToDate = x.ToDate.ToString("dd/MM/yyyy"),
                            Id = x.Id,
                            City = (x.BuildingMaster.CityId == null) ? x.BuildingMaster.OtherCity : x.BuildingMaster.CityMaster.LookUpValue,
                            OtherCity = x.BuildingMaster.OtherCity
                        }).ToList();

                LSTShiftAssignmentVMTemp.ToList().ForEach(item =>
                {
                    var shiftDetails = _genericService.ShiftDetails.GetAll().FirstOrDefault(item_db => item_db.ShiftID == item.ShitfId && item_db.SecurityID == item.UserId && item_db.IsActive);

                    if (shiftDetails != null)
                    {
                        LSTShiftAssignmentVM.Add(item);
                    }
                });

            }
            else
            {
                int orgId = user.Organization.Id;
                if (orgId != null)
                {
                    //var data = _genericService.BuildingMaster.GetAll().Where(x => x.OrganizationId == orgId).FirstOrDefault();
                    LSTShiftAssignmentVMTemp = _genericService.ShitfAssignment.GetAll().Where(x => x.IsActive == true && x.BuildingMaster.OrganizationId == orgId).ToList()
                            .Select(x => new ShiftAssignmentVM
                            {
                                BuildingId = x.BuildingId,
                                BuildingName = x.Gates.BuildingMaster.BuildingName,
                                GateId = x.GateId,
                                GateName = x.Gates.GateNumber,
                                ShitfId = x.ShitfId,
                                ShiftName = x.Shitfs.ShitfName,
                                UserId = x.UserId,
                                UserName = x.ApplicationUser.FullName,
                                FromDate = x.FromDate,
                                ToDate = x.ToDate,
                                strFromDate = x.FromDate.ToString("dd/MM/yyyy"),
                                strToDate = x.ToDate.ToString("dd/MM/yyyy"),
                                Id = x.Id,
                                City = x.BuildingMaster.CityMaster.LookUpValue
                            }).ToList();

                    LSTShiftAssignmentVMTemp.ToList().ForEach(item =>
                    {
                        var shiftDetails = _genericService.ShiftDetails.GetAll().FirstOrDefault(item_db => item_db.ShiftID == item.ShitfId && item_db.SecurityID == item.UserId && item_db.IsActive);

                        if (shiftDetails != null)
                        {
                            LSTShiftAssignmentVM.Add(item);
                        }
                    });
                }
            }
            if (LSTShiftAssignmentVM.Count() > 0)
            {
                if (!string.IsNullOrEmpty(globalSearch))
                {
                    LSTShiftAssignmentVM = LSTShiftAssignmentVM.Where(item =>
                        item.UserName.ToLower().Contains(globalSearch.ToLower()) ||
                        item.BuildingName.ToLower().Contains(globalSearch.ToLower()) ||
                         item.GateName.ToLower().Contains(globalSearch.ToLower()) ||
                        item.ShiftName.ToLower().Contains(globalSearch.ToLower())
                        ).ToList();
                }
                if (string.IsNullOrEmpty(sortField))
                {
                    sortField = " ShiftName";
                }
                var paginationRequest = new PaginationRequest
                {
                    PageIndex = (pageIndex - 1),
                    PageSize = pageSize,
                    SearchText = globalSearch,
                    Sort = new Sort { SortDirection = (sortOrder == "ASC" ? SortDirection.Ascending : SortDirection.Descending), SortBy = sortField }
                };

                int totalCount = 0;
                IList<ShiftAssignmentVM> result =
                   GenericSorterPager.GetSortedPagedList<ShiftAssignmentVM>(LSTShiftAssignmentVM.AsQueryable(), paginationRequest, out totalCount);

                var jsonData = JsonConvert.SerializeObject(result.OrderByDescending(x => x.Id));
                return JsonConvert.SerializeObject(new { totalRows = totalCount, result = jsonData });
            }
            return null;
        }
        [Route("~/Api/ShiftAssignment/SaveShiftAssignment")]
        [HttpPost]
        public ReturnResult SaveShiftAssignment([FromBody] ShiftAssignmentVM ShitfAssignment)
        {
            bool success = false;
            string Message = "";
            string currentUserId = HttpContext.Current.User.Identity.GetUserId();
            if (ShitfAssignment.Id == 0)
            {
                ShitfAssignment.FromDate = DateTime.ParseExact(ShitfAssignment.strFromDate, "dd/MM/yyyy", null);
                ShitfAssignment.ToDate = DateTime.ParseExact(ShitfAssignment.strToDate, "dd/MM/yyyy", null);

                var data = _genericService.ShitfAssignment.GetAll().Where(x => (x.FromDate >= ShitfAssignment.FromDate
                    && x.ToDate <= ShitfAssignment.ToDate
                    || x.FromDate >= ShitfAssignment.FromDate
                    && x.FromDate <= ShitfAssignment.ToDate
                    || x.ToDate >= ShitfAssignment.FromDate
                    && x.ToDate <= ShitfAssignment.ToDate
                    || x.FromDate <= ShitfAssignment.ToDate
                    && x.ToDate >= ShitfAssignment.FromDate) && (x.UserId == ShitfAssignment.UserId) && x.IsActive && (x.ShitfId == ShitfAssignment.ShitfId)).AsQueryable();

                if (data.Count() == 0)
                {
                    ShitfAssignment obj = new Data.Model.Entities.ShitfAssignment();
                    ShiftDetails _shiftDetails = null;

                    obj.ShitfId = ShitfAssignment.ShitfId;
                    obj.UserId = ShitfAssignment.UserId;
                    obj.GateId = ShitfAssignment.GateId;
                    obj.FromDate = ShitfAssignment.FromDate;
                    obj.ToDate = ShitfAssignment.ToDate;
                    obj.BuildingId = ShitfAssignment.BuildingId;
                    obj.FromDate = DateTime.ParseExact(ShitfAssignment.strFromDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture); //Convert.ToDateTime(ShitfAssignment.strFromDate);
                    obj.ToDate = DateTime.ParseExact(ShitfAssignment.strToDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);//Convert.ToDateTime(ShitfAssignment.strToDate);
                    obj.IsActive = true;
                    _genericService.ShitfAssignment.Insert(obj);



                    for (DateTime startDate = ShitfAssignment.FromDate.Date; startDate <= ShitfAssignment.ToDate.Date; startDate = startDate.AddDays(1))
                    {
                        _shiftDetails = new Data.Model.Entities.ShiftDetails();
                        _shiftDetails.ShiftID = ShitfAssignment.ShitfId;
                        _shiftDetails.SecurityID = ShitfAssignment.UserId;
                        _shiftDetails.ShiftDate = startDate;
                        _shiftDetails.GateID = ShitfAssignment.GateId;
                        _shiftDetails.IsActive = true;
                        _genericService.ShiftDetails.Insert(_shiftDetails);
                    }

                    _genericService.Commit();


                    Message = "Shift saved successfully!!";
                    success = true;
                }
                else
                {
                    return new ReturnResult { Message = "UnSuccess", Success = false };
                }
            }
            else
            {
                var existingShift = _genericService.ShitfAssignment.GetById(ShitfAssignment.Id);
                if (existingShift != null)
                {

                    ShitfAssignment.FromDate = Convert.ToDateTime(ShitfAssignment.strFromDate);
                    ShitfAssignment.ToDate = Convert.ToDateTime(ShitfAssignment.strToDate);
                    var result = _genericService.ShitfAssignment.GetAll().Where(y => y.Id != ShitfAssignment.Id && (
                     y.FromDate >= ShitfAssignment.FromDate
                    && y.ToDate <= ShitfAssignment.ToDate
                    || y.FromDate >= ShitfAssignment.FromDate
                    && y.FromDate <= ShitfAssignment.ToDate
                    || y.ToDate >= ShitfAssignment.FromDate
                    && y.ToDate <= ShitfAssignment.ToDate
                    || y.FromDate <= ShitfAssignment.ToDate
                    && y.ToDate >= ShitfAssignment.FromDate)
                    && (y.UserId == ShitfAssignment.UserId) && (y.ShitfId == ShitfAssignment.ShitfId) && y.IsActive
                    ).AsQueryable();

                    if (result.Count() == 0)
                    {

                        existingShift.ShitfId = ShitfAssignment.ShitfId;
                        existingShift.UserId = ShitfAssignment.UserId;
                        existingShift.GateId = ShitfAssignment.GateId;
                        existingShift.BuildingId = ShitfAssignment.BuildingId;
                        existingShift.FromDate = DateTime.Parse(ShitfAssignment.strFromDate);
                        existingShift.ToDate = DateTime.Parse(ShitfAssignment.strToDate);
                        existingShift.FromDate = DateTime.ParseExact(ShitfAssignment.strFromDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture); //Convert.ToDateTime(ShitfAssignment.strFromDate);
                        existingShift.ToDate = DateTime.ParseExact(ShitfAssignment.strToDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);//Convert.ToDateTime(ShitfAssignment.strToDate);
                        existingShift.IsActive = true;
                        _genericService.ShitfAssignment.Update(existingShift);
                        Message = "Shift update successfully!!";
                        success = true;
                        _genericService.Commit();
                    }
                    else
                    {
                        return new ReturnResult { Message = "UnSuccess", Success = false };

                    }

                };
            }

            return new ReturnResult { Message = Message, Success = success };
        }
        [Route("~/Api/ShiftAssignment/DeleteShift")]
        [HttpPost]
        public ReturnResult DeleteShift([FromBody] ShitfAssignment ShitfAssignment)
        {
            if (ShitfAssignment != null)
            {
                var updateShiftAssignment = _genericService.ShitfAssignment.GetAll().Where(x => x.Id == ShitfAssignment.Id).FirstOrDefault();
                if (updateShiftAssignment != null)
                {
                    updateShiftAssignment.IsActive = false;
                    _genericService.ShitfAssignment.Update(updateShiftAssignment);

                    var shiftDetails = _genericService.ShiftDetails.GetAll().Where(item_db => item_db.ShiftID == updateShiftAssignment.ShitfId &&item_db.IsActive );
                    for (DateTime shiftDate = updateShiftAssignment.FromDate; shiftDate <= updateShiftAssignment.ToDate; shiftDate = shiftDate.AddDays(1))
                    {
                        foreach (var shiftDetail in shiftDetails)
                        {
                            if (shiftDetail.ShiftDate == shiftDate && shiftDetail.IsActive)
                            {
                                shiftDetail.IsActive = false;
                                _genericService.ShiftDetails.Update(shiftDetail);
                            }
                        }
                    }





                    _genericService.Commit();
                    return new ReturnResult { Message = "Success", Success = true };
                }
            }
            return new ReturnResult { Message = "Failure", Success = false };
        }
    }
}
