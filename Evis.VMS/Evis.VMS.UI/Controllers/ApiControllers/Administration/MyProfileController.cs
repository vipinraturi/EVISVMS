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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Evis.VMS.UI.Controllers.ApiControllers
{
    public partial class AdministrationController
    {

        [Route("~/Api/MyProfile/SaveMyProfile")]
        [HttpPost]
        public async Task<ReturnResult> SaveMyProfile([FromBody] MyProfileVM myProfileVM)
        {
            string userId = HttpContext.Current.User.Identity.GetUserId();
            var currentUser = await _userService.GetAsync(x => x.Id == userId);
            currentUser.FullName = myProfileVM.FullName;
            currentUser.GenderId = myProfileVM.GenderId;
            currentUser.PhoneNumber = myProfileVM.PhoneNumber;
            currentUser.ContactAddress = myProfileVM.ContactAddress;

            if (!string.IsNullOrEmpty(myProfileVM.ProfilePicturePath))
            {
                currentUser.ProfilePicturePath = (string.IsNullOrEmpty(myProfileVM.ProfilePicturePath) ? string.Empty : string.Format("/images/UserImages/{0}", myProfileVM.ProfilePicturePath));
            }

            await _userService.UpdateAsync(currentUser, string.Empty);
            _genericService.Commit();

            return new ReturnResult { Message = "Success", Success = true };
        }

        [Route("~/Api/MyProfile/GetMyProfile")]
        [HttpGet]
        public async Task<ApplicationUser> GetMyProfile()
        {
            string userId = HttpContext.Current.User.Identity.GetUserId();
            var currentUser = await _userService.GetAsync(x => x.Id == userId);
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string roleId = string.Empty;
            foreach (var claim in claims)
            {
                if (claim.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")
                {
                    currentUser.Roles.Add(new IdentityUserRole { RoleId = claim.Value, UserId = currentUser.Id });
                    break;
                }

            }

            if (currentUser != null)
            {
                try
                {
                    //var filePath = HttpContext.Current.Server.MapPath("\\") + "" + currentUser.ProfilePicturePath.Replace("/", "\\");

                    var filePath = HttpContext.Current.Server.MapPath("\\") + "" +(!string.IsNullOrEmpty(currentUser.ProfilePicturePath) ? currentUser.ProfilePicturePath.Replace("/", "\\") : HttpContext.Current.Server.MapPath("~/images/avatar.jpg"));

                    currentUser.IsImageAvailable = System.IO.File.Exists(filePath);
                }
                catch (Exception e)
                { }
            }

            return currentUser;
        }

        [Route("~/Api/MyProfile/GetGender")]
        [HttpGet]
        public IQueryable<GeneralDropDownVM> GetGender()
        {
            var genders = _genericService.LookUpValues.GetAll().Where(x => x.LookUpType.TypeName == "Gender" && x.IsActive == true && x.LookUpType.IsActive == true)
                .Select(y => new GeneralDropDownVM { Id = y.Id, Name = y.LookUpValue });
            return genders;
        }

        [Route("~/Api/MyProfile/GetAllRoles")]
        [HttpGet]
        public async Task<IEnumerable<ApplicationRole>> GetAllRoles()
        {
            return await _applicationRoleService.GetAllAsync();
        }
    }
}
