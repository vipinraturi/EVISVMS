/********************************************************************************
 * Company Name : East Vision Information System
 * Team Name    : EVIS IT Team
 * Author       : Vipin Raturi
 * Created On   : 06/05/2016
 * Description  : 
 *******************************************************************************/

using Evis.VMS.Business;
using Evis.VMS.UI.HelperClasses;
using Evis.VMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using Evis.VMS.Data.Model.Entities;
using System.Web;
using Microsoft.AspNet.Identity;
using Evis.VMS.Utilities;
using System.Globalization;
using Newtonsoft.Json;

namespace Evis.VMS.UI.Controllers.ApiControllers
{
    public partial class ReportController
    {


        [Route("~/Api/Report/Getorganisation")]
        [HttpGet]
        public async Task<ApplicationUser> GetOrganisation()
        {
            string userId = HttpContext.Current.User.Identity.GetUserId();
            var organisation = await _userService.GetAsync(x => x.Id == userId);


            return organisation;


        }
        [Route("~/Api/Report/GetBuildings")]
        [HttpGet]
        public IEnumerable<GeneralDropDownVM> GetAllBuildingDetails(int? Id)
        {
            if (Id != null)
            {
                var result = _genericService.BuildingMaster.GetAll().Where(x => x.IsActive == true && x.OrganizationId == Id)
                  .Select(y => new GeneralDropDownVM { Id = y.Id, Name = y.BuildingName });
                return result;
            }
            else
            {
                var result = _genericService.BuildingMaster.GetAll().Select(y => new GeneralDropDownVM { Id = y.Id, Name = y.BuildingName });
                return result;
            }


        }

        [Route("~/Api/Report/GetGates")]
        [HttpGet]
        public IEnumerable<GeneralDropDownVM> GetAllGates(int? Id)
        {
            ////&& x.BuildingMaster.OrganizationId ==Id
            if (Id != null)
            {
                var result = _genericService.GateMaster.GetAll().Where(x => x.IsActive == true && x.BuildingMaster.OrganizationId == Id)
                    .Select(y => new GeneralDropDownVM { Id = y.Id, Name = y.GateNumber });
                return result;
            }
            else
            {
                var result = _genericService.GateMaster.GetAll().Where(x => x.IsActive == true)
                    .Select(y => new GeneralDropDownVM { Id = y.Id, Name = y.GateNumber });
                return result;

            }
        }

        [Route("~/Api/Report/GetUsers")]
        [HttpGet]
        public async Task<IEnumerable<DropDownVM>> GetUsers()
        {

            var user = (await _userService.GetAllAsync()).FirstOrDefault();

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
        [Route("~/Api/Report/GetShifts")]
        [HttpGet]
        public IEnumerable<GeneralDropDownVM> GetShifts()
        {

            ////var result = _genericService.ShitfMaster.GetAll().Where(x => x.IsActive == true)
            //// .Select(y => new GeneralDropDownVM { Id = y.Id, Name = y.ShitfName });
            ////return result;
            var result = _genericService.ShitfMaster.GetAll().Where(x => x.IsActive == true).AsEnumerable()
               .Select(y => new GeneralDropDownVM { Id = y.Id, Name = y.ShitfName + " (" + y.FromTime.ToString("hh:mm tt") + " - " + y.ToTime.ToString("hh:mm tt") + ")" });
            return result;

        }

        [Route("~/Api/Report/GetShiftDetailsGrid")]
        [HttpPost]
        public string GetShiftDataGrid(string search, int pageIndex, int pageSize, string sortField = "", string sortOrder = "ASC", string globalSearch = "")
        {
           

            int totalCount = 0;
          
            var result = _ShiftDetailsReportHelper.GetShiftData(search, pageIndex, pageSize, sortField, sortOrder, out totalCount);

            var jsonData = JsonConvert.SerializeObject(result);
            //.OrderBy(x => x.FromDate)
            return JsonConvert.SerializeObject(new { totalRows = totalCount, result = jsonData });
        }



    }



}
