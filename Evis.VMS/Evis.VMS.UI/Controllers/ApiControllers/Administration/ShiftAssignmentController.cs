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

namespace Evis.VMS.UI.Controllers.ApiControllers
{
    public partial class AdministrationController
    {
        readonly UserManager<ApplicationUser> _userManager;

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
            var result = _genericService.ShitfMaster.GetAll().Where(x => x.IsActive == true)
                .Select(y => new GeneralDropDownVM { Id = y.Id, Name = y.ShitfName });////y.ShitfName + '(' + ' ' + y.FromTime + ' ' + y.ToTime + ')'
            return result;
        }
        [Route("~/Api/ShiftAssignment/GetAllUsers")]
        [HttpGet]
        public async Task<IEnumerable<DropDownVM>> GetAllUsers(int GateId)
        {

            var user = (await _userService.GetAllAsync()).Where(x => x.Id == HttpContext.Current.User.Identity.GetUserId() && x.IsActive == true).FirstOrDefault();
            var gates = _genericService.GateMaster.GetAll().FirstOrDefault(x => x.Id == GateId && x.IsActive == true);
            //var result = (await _userService.GetAllAsync()).Where(x => x.IsActive == true && x.OrganizationId == gates.BuildingMaster.OrganizationId && x.Id == "1")
            //   .Select(y => new DropDownVM { Id = y.Id, Name = y.FullName });
            var getUsers = (await _userService.GetAllAsync()).Where(x => x.Organization.IsActive == true &&
                           (user == null || (user != null && x.OrganizationId == user.OrganizationId))).AsQueryable();

            var getRoles = (await _applicationRoleService.GetAllAsync()).AsQueryable();
            var result = (from users in getUsers
                          join roles in getRoles on users.Roles.First().RoleId equals roles.Id
                          where roles.Id == "8179895d-54f2-4316-aab2-e86431fa8b3c"
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
            var Shift = _genericService.ShitfAssignment.GetAll().Where(x => x.IsActive == true).ToList()
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
                    FromDate = Convert.ToDateTime(x.FromDate.ToShortDateString()),
                    ToDate = Convert.ToDateTime(x.ToDate.ToShortDateString()),
                    Id = x.Id

                })
                .ToList();
            if (Shift.Count() > 0)
            {
                if (!string.IsNullOrEmpty(globalSearch))
                {
                    Shift = Shift.Where(item =>
                        item.UserName.ToLower().Contains(globalSearch.ToLower()) ||
                        item.BuildingName.ToLower().Contains(globalSearch.ToLower()) ||
                         item.GateName.ToLower().Contains(globalSearch.ToLower()) ||
                        item.ShiftName.ToLower().Contains(globalSearch.ToLower())
                        ).ToList();
                }


                bool sortAscending = (sortOrder == "ASC" ? true : false);
                if (!string.IsNullOrEmpty(sortField))
                {
                    if (!sortAscending)
                    {
                        Shift = Shift
                               .OrderBy(r => r.GetType().GetProperty(sortField).GetValue(r, null))
                               .ToList();
                    }
                    else
                    {
                        Shift = Shift
                               .OrderByDescending(r => r.GetType().GetProperty(sortField).GetValue(r, null))
                               .ToList();
                    }
                }
            }

            var data = Shift.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
            var jsonData = JsonConvert.SerializeObject(data);
            var total = Shift.Count();
            return JsonConvert.SerializeObject(new { totalRows = total, result = jsonData });
        }
        [Route("~/Api/ShiftAssignment/SaveShiftAssignment")]
        [HttpPost]
        public ReturnResult SaveShiftAssignment([FromBody] ShitfAssignment ShitfAssignment)
        {

            if (ShitfAssignment.Id == 0)
            {
                ShitfAssignment.IsActive = true;
                _genericService.ShitfAssignment.Insert(ShitfAssignment);
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
