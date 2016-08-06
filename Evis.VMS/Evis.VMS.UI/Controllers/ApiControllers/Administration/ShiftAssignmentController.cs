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


namespace Evis.VMS.UI.Controllers.ApiControllers
{
    public partial class AdministrationController
    {


        [Route("~/Api/ShiftAssignment/GetAllGates")]
        [HttpGet]
        public IEnumerable<GeneralDropDownVM> GetAllGates(int BuildingId)
        {
            var result = _genericService.GateMaster.GetAll().Where(x => x.IsActive == true & x.BuildingId == BuildingId)
                .Select(y => new GeneralDropDownVM { Id = y.Id, Name = y.GateNumber });
            return result;
        }

        [Route("~/Api/ShiftAssignment/GetAllShift")]
        [HttpGet]
        public IEnumerable<GeneralDropDownVM> GetAllShift()
        {
            var result = _genericService.ShitfMaster.GetAll().Where(x => x.IsActive == true).AsEnumerable()
                .Select(y => new GeneralDropDownVM { Id = y.Id, Name = y.ShitfName + " (" + y.FromTime.ToString("hh:mm tt") + " - " + y.ToTime.ToString("hh:mm tt") + ")" });////y.ShitfName + '(' + ' ' + y.FromTime + ' ' + y.ToTime + ')'

            return result;
        }

        [Route("~/Api/ShiftAssignment/GetAllUsers")]
        [HttpGet]
        public async Task<IEnumerable<DropDownVM>> GetAllUsers(int GateId)
        {

            var user = (await _userService.GetAllAsync()).Where(x => x.Id == HttpContext.Current.User.Identity.GetUserId() && x.IsActive == true).FirstOrDefault();
            var gates = _genericService.GateMaster.GetAll().FirstOrDefault(x => x.Id == GateId && x.IsActive == true);
            var getUsers = (await _userService.GetAllAsync()).Where(x => x.Organization.IsActive == true &&
                           (user == null || (user != null && x.OrganizationId == user.OrganizationId))).AsQueryable();

            var getRoles = (await _applicationRoleService.GetAllAsync()).AsQueryable();
            var result = (from users in getUsers
                          join roles in getRoles on users.Roles.First().RoleId equals roles.Id
                          where roles.Name == "Security"
                          select new DropDownVM
                          {
                              Id = users.Id,
                              Name = users.FullName
                          }).AsQueryable();

            return result;
        }

        [Route("~/Api/ShiftAssignment/GetAllShiftAssignment")]
        [HttpPost]
        public string GetAllShiftAssignment(string globalSearch, int pageIndex, int pageSize, string sortField = "", string sortOrder = "ASC")
        {
            var Shift = _genericService.ShitfAssignment.GetAll().Where(x => x.IsActive == true)
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
                   strFromDate = x.ToDate.ToString(),
                   strToDate = x.ToDate.ToString(),
                   Id = x.Id,
                   City = x.BuildingMaster.CityMaster.LookUpValue
               }).AsQueryable();

            if (Shift.Count() > 0)
            {
                if (!string.IsNullOrEmpty(globalSearch))
                {
                    Shift = Shift.Where(item =>
                        item.UserName.ToLower().Contains(globalSearch.ToLower()) ||
                        item.BuildingName.ToLower().Contains(globalSearch.ToLower()) ||
                         item.GateName.ToLower().Contains(globalSearch.ToLower()) ||
                        item.ShiftName.ToLower().Contains(globalSearch.ToLower())
                        ).AsQueryable();
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
                   GenericSorterPager.GetSortedPagedList<ShiftAssignmentVM>(Shift, paginationRequest, out totalCount);

                var jsonData = JsonConvert.SerializeObject(result.OrderByDescending(x => x.Id));
                return JsonConvert.SerializeObject(new { totalRows = totalCount, result = jsonData });

            }
            return null;
        }
        [Route("~/Api/ShiftAssignment/SaveShiftAssignment")]
        [HttpPost]
        public ReturnResult SaveShiftAssignment([FromBody] ShitfAssignment ShitfAssignment)
        {

            if (ShitfAssignment.Id == 0)
            {
                var data = _genericService.ShitfAssignment.GetAll().Where(x => x.UserId == ShitfAssignment.UserId && x.ShitfId == ShitfAssignment.ShitfId && x.IsActive == true).ToList();
                if (data.Count() == 0)
                {
                    ShitfAssignment.IsActive = true;
                    var ToDate = Convert.ToDateTime(ShitfAssignment.ToDate).ToShortDateString();
                    ShitfAssignment.ToDate = Convert.ToDateTime(ToDate);
                    var FromDate = Convert.ToDateTime(ShitfAssignment.FromDate).ToShortDateString();
                    ShitfAssignment.FromDate = Convert.ToDateTime(FromDate);
                    _genericService.ShitfAssignment.Insert(ShitfAssignment);
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
                    existingShift.BuildingId = ShitfAssignment.BuildingId;
                    existingShift.GateId = ShitfAssignment.GateId;
                    existingShift.UserId = ShitfAssignment.UserId;
                    existingShift.ShitfId = ShitfAssignment.ShitfId;
                    existingShift.FromDate = ShitfAssignment.FromDate;
                    existingShift.ToDate = ShitfAssignment.ToDate;
                    ShitfAssignment.IsActive = true;
                    _genericService.ShitfAssignment.Update(existingShift);
                };
            }
            _genericService.Commit();
            return new ReturnResult { Message = "Success", Success = true };
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
                    _genericService.ShitfAssignment.Update(Delete);
                    _genericService.Commit();
                    return new ReturnResult { Message = "Success", Success = true };
                }
            }
            return new ReturnResult { Message = "Failure", Success = false };
        }
    }
}
