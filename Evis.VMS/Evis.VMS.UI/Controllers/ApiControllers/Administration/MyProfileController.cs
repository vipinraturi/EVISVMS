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

namespace Evis.VMS.UI.Controllers.ApiControllers
{
    public partial class AdministrationController
    {

        [Route("~/Api/MyProfile/SaveMyProfile")]
        [HttpPut]
        public async Task<ReturnResult> SaveMyProfile([FromBody] MyProfileVM myProfileVM)
        {
            string userId = "2d410275-a905-4476-af21-91c577fa66b0";

            var currentUser = await _userService.GetAsync(x => x.Id == userId);
            currentUser.FullName = myProfileVM.FullName;
            currentUser.PhoneNumber = myProfileVM.PhoneNumber;
            currentUser.ContactAddress = myProfileVM.ContactAddress;

            await _userService.UpdateAsync(currentUser, string.Empty);

            return new ReturnResult { Message = "Success", Success = true };
        }

        [Route("~/Api/MyProfile/GetMyProfile")]
        [HttpGet]
        public async Task<ApplicationUser> GetMyProfile()
        {
            // To do
            //var userId = HttpContext.Current.User.Identity.GetUserId();
            string userId = "2d410275-a905-4476-af21-91c577fa66b0";
            var currentUser = await _userService.GetAsync(x => x.Id == userId);
            return currentUser;
        }

        //[Route("~/Api/MyProfile/GetGender")]
        //[HttpGet]
        //public IQueryable<GenderVM> GetGender()
        //{
        //    return _genericService.Gender.GetAll();
        //}

        [Route("~/Api/MyProfile/GetAllRoles")]
        [HttpGet]
        public async Task<IEnumerable<ApplicationRole>> GetAllRoles()
        {
            return await _applicationRoleService.GetAllAsync();
        }
    }
}
