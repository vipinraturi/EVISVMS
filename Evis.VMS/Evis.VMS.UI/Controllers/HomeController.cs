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
            var user = await _userService.GetAsync(x => x.Id == userId);

            var roleType = RoleType.SuperAdmin;
            var roleID = user.Roles.FirstOrDefault().RoleId;
            
            switch (roleID)
            {
                case "5b3153c0-45ef-4ef2-99ba-7ecaca165398":
                    roleType = RoleType.SuperAdmin;
                    break;
                case "0ab21d5a-77cb-4384-bd7f-f43730e3d873":
                    roleType = RoleType.SuperVisor;
                    break;
                case "4004214e-2665-417d-a96e-adce96b3f201":
                    roleType = RoleType.Security;
                    break;
                default:
                    break;
            }

            var userSessionData =
                                new UserSessionDataVM
                                {
                                    ThemeName = (string.IsNullOrEmpty(user.ThemeName) ? "theme1" : user.ThemeName),
                                    ImageLogoPath = (user.Organization == null ? "~\\images\\logo\\main_logo.png" : (string.IsNullOrEmpty(user.Organization.ImagePath) ? "~\\images\\logo\\main_logo.png" : user.Organization.ImagePath)),
                                    UserRole = roleType
                                };
            Session["UserSession"] = userSessionData;

            return View();
        }

    }
}