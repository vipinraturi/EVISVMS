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
using Evis.VMS.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.Reporting.WebForms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace Evis.VMS.UI.Controllers.ApiControllers
{
    public partial class ReportController
    {
        [Route("~/Api/VisitorsDetails/GetVisitorsDetails")]
        [HttpPost]
        public string GetVisitorData(string search, int pageIndex, int pageSize, string sortField = "", string sortOrder = "ASC", string globalSearch = "")
        {
            int totalCount = 0;
            var result = _visitorDetailsReportHelper.VisitorData(search, pageIndex, pageSize, sortField, sortOrder, out totalCount);
            var jsonData = JsonConvert.SerializeObject(result);
            return JsonConvert.SerializeObject(new { totalRows = totalCount, result = jsonData });
        }
        [Route("~/Api/VisitorsDetails/GetGates")]
        [HttpGet]
        public IEnumerable<GeneralDropDownVM> GetAllGates(int BuildingId)
        {
            var result = _genericService.GateMaster.GetAll().Where(x => x.IsActive == true & x.BuildingId == BuildingId)
                .Select(y => new GeneralDropDownVM { Id = y.Id, Name = y.GateNumber });
            return result.OrderByDescending(x => x.Id);
        }

        [Route("~/Api/VisitorsDetails/GetUsers")]
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

    }

}
