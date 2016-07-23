/********************************************************************************
 * Company Name : East Vision Information System
 * Team Name    : EVIS IT Team
 * Author       : Vipin Raturi
 * Created On   : 06/05/2016
 * Description  : 
 *******************************************************************************/

using Evis.VMS.UI.HelperClasses;
using Evis.VMS.UI.ViewModel;
using Newtonsoft.Json;
using System.Linq;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Evis.VMS.Business;
using System.Threading.Tasks;

namespace Evis.VMS.UI.Controllers.ApiControllers
{
    public partial class VisitorController
    {
        [Route("~/Api/VisitorManagement/GetVisitorCheckInHistory")]
        [HttpPost]
        public async Task<VisitorDataVM> GetVisitorCheckInHistory(long visitorId)//int pageIndex, int pageSize, string sortField = "", string sortOrder = "ASC"
        {
            var visitorDataVMData = _visitorCheckInCheckOutHelper.GetVisitorCheckInHistory(visitorId);

            if (visitorDataVMData != null)
            {
                var userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
                var _userService = new UserService();
                var _applicationRole = new ApplicationRoleService();
                var user = await _userService.GetAsync(x => x.Id == userId);
                var roleID = user.Roles.FirstOrDefault().RoleId;
                var role = await _applicationRole.FindByIdAsync(roleID);
                if (role.Name == "Security")
                {
                    visitorDataVMData.IsSecurityPerson = true;
                }
            }
            return visitorDataVMData;
        }

        [Route("~/Api/VisitorManagement/SaveVisitorCheckIn")]
        [HttpPost]
        public ReturnResult SaveVisitorCheckIn([FromBody] VisitorCheckInVM visitorCheckInVM)
        {
            visitorCheckInVM.CreatedBy = HttpContext.Current.User.Identity.GetUserId();
            visitorCheckInVM.CheckInGate = 1;
            var result = _visitorCheckInCheckOutHelper.SaveVisitorCheckIn(visitorCheckInVM);
            return new ReturnResult { Message = (result == true ? "Success" : "Failure"), Success = (result == true ? true : false) };
        }
    }
}
