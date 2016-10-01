/********************************************************************************
 * Company Name : East Vision Information System
 * Team Name    : EVIS IT Team
 * Author       : Vipin Raturi
 * Created On   : 06/05/2016
 * Description  : 
 *******************************************************************************/

using Evis.VMS.Business;
using Evis.VMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Evis.VMS.UI.HelperClasses;
namespace Evis.VMS.UI.Controllers
{
    [Authorize]
    public partial class DashboardController : Controller
    {
        GenericService _genericService = new GenericService();
        //
        // GET: /Dashboard/
        public async Task<ActionResult> _Dashboard()
        {

            //ShiftManagemetHelper obj = new ShiftManagemetHelper();
            ////DateTime dd = DateTime.Now;
            ////var a = DateTime.DaysInMonth(dd.Year, dd.Month);
            //var q = obj.GetHeaders(DateTime.Now.AddDays(-12), DateTime.Now);
            //var test = obj.GetShiftData(DateTime.Now.AddDays(-12), DateTime.Now, 1, 23);

            var userId = System.Web.HttpContext.Current.User.Identity.GetUserId();

            var _userService = new UserService();
            var _applicationRole = new ApplicationRoleService();
            var user = await _userService.GetAsync(x => x.Id == userId);
            int BuldingCount = 0;
            int GatesCount = 0;
            int CheckInCount = 0;
            int VisitorCount = 0;
            if (user.OrganizationId != null)
            {
                CheckInCount = _genericService.VisitDetails.GetAll().Where(x => (EntityFunctions.TruncateTime(x.CheckIn) == EntityFunctions.TruncateTime(DateTime.Now) && x.CheckOut == null) && x.GateMaster.BuildingMaster.OrganizationId == user.OrganizationId).Count();
                VisitorCount = _genericService.VisitDetails.GetAll().Where(x => (EntityFunctions.TruncateTime(x.CheckIn) == EntityFunctions.TruncateTime(DateTime.Now)) && x.GateMaster.BuildingMaster.OrganizationId == user.OrganizationId).Count();
                BuldingCount = _genericService.BuildingMaster.GetAll().Where(x => x.IsActive && x.OrganizationId == user.OrganizationId).Count();
                GatesCount = _genericService.GateMaster.GetAll().Where(x => x.IsActive && x.BuildingMaster.OrganizationId == user.OrganizationId).Count();

            }
            else
            {
                CheckInCount = _genericService.VisitDetails.GetAll().Where(x => (EntityFunctions.TruncateTime(x.CheckIn) == EntityFunctions.TruncateTime(DateTime.Now) && x.CheckOut == null) ).Count();
                VisitorCount = _genericService.VisitDetails.GetAll().Where(x => (EntityFunctions.TruncateTime(x.CheckIn) == EntityFunctions.TruncateTime(DateTime.Now))).Count();
                BuldingCount = _genericService.BuildingMaster.GetAll().Where(x => x.IsActive).Count();
                GatesCount = _genericService.GateMaster.GetAll().Where(x => x.IsActive).Count();
            }
            var DashboardData = new DashboardVM
            {
                CheckInCount = CheckInCount,
                VisitorsCount = VisitorCount,
                BuldingCount = BuldingCount,
                GatesCount = GatesCount
            };
            Session["DashboardData"] = DashboardData;
            return View();
        }
    }
}