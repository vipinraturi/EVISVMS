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
        [Route("~/Api/MyOrginization/GetMyOrginization")]
        [HttpGet]
        public async Task<ApplicationUser> GetMyOrginization()
        {
            string userId = HttpContext.Current.User.Identity.GetUserId();
            var currentUser = await _userService.GetAsync(x => x.Id == userId);
            var currentOrginization = _genericService.Organization.GetAll().Where(x => x.Id == currentUser.OrganizationId).ToList();
            return currentUser;
        }
    }

}
