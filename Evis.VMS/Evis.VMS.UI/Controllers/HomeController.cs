/********************************************************************************
 * Company Name : East Vision Information System
 * Team Name    : EVIS IT Team
 * Author       : Vipin Raturi
 * Created On   : 06/05/2016
 * Description  : 
 *******************************************************************************/

using Evis.VMS.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Evis.VMS.UI.ViewModel;

namespace Evis.VMS.UI.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            var userId = System.Web.HttpContext.Current.User.Identity.GetUserId();

            var _userService = new UserService();
            var _applicationRole = new ApplicationRoleService();
            var user = await _userService.GetAsync(x => x.Id == userId);

            var roleType = RoleType.SuperAdmin;
            var roleID = user.Roles.FirstOrDefault().RoleId;

            var role = await _applicationRole.FindByIdAsync(roleID);
            switch (role.Name)
            {
                case "SuperAdmin":
                    roleType = RoleType.SuperAdmin;
                    break;
                case "Supervisor":
                    roleType = RoleType.SuperVisor;
                    break;
                case "Security":
                    roleType = RoleType.Security;
                    break;
                case "BuildingAdmin":
                    roleType = RoleType.BuildingAdmin;
                    break;
                default:
                    break;
            }

            var userSessionData =
                                new UserSessionDataVM
                                {
                                    ThemeName = (string.IsNullOrEmpty(user.ThemeName) ? "theme1" : user.ThemeName),
                                    ImageLogoPath = (user.Organization == null ? "~\\images\\logo\\main_logo.png" : (string.IsNullOrEmpty(user.Organization.ImagePath) ? "~\\images\\logo\\main_logo.png" : user.Organization.ImagePath)),
                                    UserRole = roleType,
                                    ProfilePicPath = user.ProfilePicturePath,
                                    FullName = user.FullName
                                };
            Session["UserSession"] = userSessionData;

            return View();
        }

    }
}