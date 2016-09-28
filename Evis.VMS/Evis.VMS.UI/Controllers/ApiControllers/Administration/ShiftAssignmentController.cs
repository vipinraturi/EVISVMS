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

        IQueryable<ShiftAssignmentVM> LSTShiftAssignmentVM;
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
            var user = (await _userService.GetAllAsync()).Where(x => x.Id == HttpContext.Current.User.Identity.GetUserId() && x.IsActive == true).FirstOrDefault();
            if (user == null)
            {
                LSTShiftAssignmentVM = _genericService.ShitfAssignment.GetAll().Where(x => x.IsActive == true).ToList()
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
                        }).AsQueryable();

            }
            else
            {
                int orgId = user.Organization.Id;
                if (orgId != null)
                {
                    var data = _genericService.BuildingMaster.GetAll().Where(x => x.OrganizationId == orgId).FirstOrDefault();
                    LSTShiftAssignmentVM = _genericService.ShitfAssignment.GetAll().Where(x => x.IsActive == true && x.BuildingId == data.Id).ToList()
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
                            }).AsQueryable();
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
                        ).AsQueryable();
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
                   GenericSorterPager.GetSortedPagedList<ShiftAssignmentVM>(LSTShiftAssignmentVM, paginationRequest, out totalCount);

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
                ShitfAssignment.FromDate = Convert.ToDateTime(ShitfAssignment.strFromDate);
                ShitfAssignment.ToDate = Convert.ToDateTime(ShitfAssignment.strToDate);

                var data = _genericService.ShitfAssignment.GetAll().Where(x => (x.FromDate >= ShitfAssignment.FromDate
                    && x.ToDate <= ShitfAssignment.ToDate
                    || x.FromDate >= ShitfAssignment.FromDate
                    && x.FromDate <= ShitfAssignment.ToDate
                    || x.ToDate >= ShitfAssignment.FromDate
                    && x.ToDate <= ShitfAssignment.ToDate
                    || x.FromDate <= ShitfAssignment.ToDate
                    && x.ToDate >= ShitfAssignment.FromDate) && (x.UserId == ShitfAssignment.UserId)).ToList();
                if (data.Count() == 0)
                {
                    ShitfAssignment obj = new Data.Model.Entities.ShitfAssignment();
                    ShiftDetails ShiftDetails = new Data.Model.Entities.ShiftDetails();
                    //ShitfAssignment.IsActive = true;
                    //var ToDate = ShitfAssignment.ToDate.ToShortDateString();
                    //ShitfAssignment.ToDate = Convert.ToDateTime(ToDate);
                    //var FromDate = ShitfAssignment.FromDate.ToShortDateString();
                    //ShitfAssignment.FromDate = Convert.ToDateTime(FromDate);

                    obj.ShitfId = ShitfAssignment.ShitfId;
                    obj.UserId = ShitfAssignment.UserId;
                    obj.GateId = ShitfAssignment.GateId;
                    obj.FromDate = ShitfAssignment.FromDate;
                    obj.ToDate = ShitfAssignment.ToDate;
                    obj.BuildingId = ShitfAssignment.BuildingId;
                    obj.FromDate = DateTime.ParseExact(ShitfAssignment.strFromDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture); //Convert.ToDateTime(ShitfAssignment.strFromDate);
                    obj.ToDate = DateTime.ParseExact(ShitfAssignment.strToDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);//Convert.ToDateTime(ShitfAssignment.strToDate);
                    obj.IsActive = true;
                    for (DateTime frDate = ShitfAssignment.FromDate; frDate > ShitfAssignment.ToDate; frDate.AddDays(1))
                    {
                        ShiftDetails.ShiftID = ShitfAssignment.ShitfId;
                        ShiftDetails.SecurityID = ShitfAssignment.UserId;
                        ShiftDetails.ShiftDate = ShitfAssignment.FromDate;
                    }
                    _genericService.ShiftDetails.Insert(ShiftDetails);
                    _genericService.ShitfAssignment.Insert(obj);
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
                    && (y.UserId == ShitfAssignment.UserId)
                    ).ToList();
                    //existingShift.FromDate = Convert.ToDateTime(ShitfAssignment.strFromDate);
                    //existingShift.ToDate = Convert.ToDateTime(ShitfAssignment.strToDate);
                    //var data = _genericService.ShitfAssignment.GetAll().Where(x => x.Id != ShitfAssignment.Id && x.FromDate >= existingShift.FromDate

                    //&& x.ToDate <= existingShift.ToDate
                    //|| x.FromDate >= existingShift.FromDate
                    //&& x.FromDate <= existingShift.ToDate
                    //|| x.ToDate >= existingShift.FromDate
                    //&& x.ToDate <= existingShift.ToDate
                    //|| x.FromDate <= existingShift.ToDate
                    //&& x.ToDate >= existingShift.FromDate).ToList();
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
                    }
                    else
                    {
                        return new ReturnResult { Message = "UnSuccess", Success = false };

                    }

                };
            }
            _genericService.Commit();
            return new ReturnResult { Message = Message, Success = success };
        }
        [Route("~/Api/ShiftAssignment/DeleteShift")]
        [HttpPost]
        public ReturnResult DeleteShift([FromBody] ShitfAssignment ShitfAssignment)
        {
            if (ShitfAssignment != null)
            {
                var Delete = _genericService.ShitfAssignment.GetAll().Where(x => x.Id == ShitfAssignment.Id).FirstOrDefault();
                if (Delete != null)
                {
                    Delete.IsActive = false;
                    _genericService.ShitfAssignment.Delete(Delete);
                    _genericService.Commit();
                    return new ReturnResult { Message = "Success", Success = true };
                }
            }
            return new ReturnResult { Message = "Failure", Success = false };
        }
    }
}
