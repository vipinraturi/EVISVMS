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
        public async Task<OrganizationVM> GetMyOrginization()
        {
            string userId = HttpContext.Current.User.Identity.GetUserId();
            var currentUser = await _userService.GetAsync(x => x.Id == userId);
            var result = _genericService.Organization.GetAll().FirstOrDefault(item => item.Id == currentUser.OrganizationId);
            // result.CityMaster = null;
            return new OrganizationVM { CompanyId = result.Id, CityId = (int)result.CityId, CityMaster = result.CityMaster, Address = result.ContactAddress, CompanyName = result.CompanyName, ContactNo = result.ContactNumber, EmailAddress = result.EmailId, FaxNo = result.FaxNumber, WebSite = result.WebSite, ZipCode = result.ZipCode, Theme = result.ThemeName, ImagePath = result.ImagePath };
        }
    }

}
