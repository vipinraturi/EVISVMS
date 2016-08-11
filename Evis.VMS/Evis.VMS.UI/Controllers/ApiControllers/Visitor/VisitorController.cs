/********************************************************************************
 * Company Name : East Vision Information System
 * Team Name    : EVIS IT Team
 * Author       : Vipin Raturi
 * Created On   : 06/05/2016
 * Description  : 
 *******************************************************************************/


using Evis.VMS.UI.HelperClasses;
using Evis.VMS.UI.ViewModel;
using Evis.VMS.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using Evis.VMS.Business;
using MODI;


namespace Evis.VMS.UI.Controllers.ApiControllers
{
    public partial class VisitorController : BaseApiController
    {
        public static IList<VisitorDetailsVM> lstVisitorDetails = new List<VisitorDetailsVM>();


        public readonly VisitorHelper _visitorHelper = null;
        public readonly UserService _userService = null;
        public readonly VisitorCheckInCheckOutHelper _visitorCheckInCheckOutHelper = null;

        public readonly ScanVisitorHelper _scanVisitorHelper = null;

        public VisitorController()
        {
            _visitorHelper = new VisitorHelper();
            _visitorCheckInCheckOutHelper = new VisitorCheckInCheckOutHelper();
            _userService = new UserService();
            _scanVisitorHelper = new ScanVisitorHelper();
        }

        [Route("~/Api/Visitor/SaveVisitor")]
        [HttpPost]
        public ReturnResult SaveVisitor([FromBody] VisitorDetailsVM visitorDetailsVM)
        {

            var message = "";
            var result = false;

            if (visitorDetailsVM.IsInsert)
            {
                message = _visitorHelper.IsVisitorEmailExist(visitorDetailsVM);
            }

            if (string.IsNullOrEmpty(message))
            {
                result = _visitorHelper.SaveVisitor(visitorDetailsVM);
                return new ReturnResult { Message = "Success", Success = true };
            }

            return new ReturnResult { Message = message, Success = false };
        }
        [Route("~/Api/Visitor/ScanImage")]
        [HttpPost]
        public ScanVisitorVM ScanImage([FromBody] string[] values)
        {
            ScanVisitorVM result = null;
            foreach (var item in values)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    result = _scanVisitorHelper.ScanDetails(item);
                }
            }
            return result;
        }
        [Route("~/Api/Visitor/GetVisitorData")]
        [HttpPost]
        public async Task<string> GetVisitorData(int pageIndex, int pageSize, string sortField = "", string sortOrder = "ASC", string globalSearch = "")
        {
            var user = (await _userService.GetAllAsync()).Where(x => x.Id == HttpContext.Current.User.Identity.GetUserId() && x.IsActive == true).FirstOrDefault();

            if (string.IsNullOrEmpty(sortField))
            {
                sortField = "VisitorName";
            }

            if (string.IsNullOrEmpty(globalSearch))
            {
                globalSearch = "";
            }

            int totalCount = 0;
            pageIndex = (pageIndex - 1);

            var lstVisitorsFromDb = _visitorHelper.GetAllVisitorsData(globalSearch, pageIndex, pageSize, sortField, sortOrder, out totalCount, (user == null) ? null : user.OrganizationId);
            var jsonData = JsonConvert.SerializeObject(lstVisitorsFromDb.OrderByDescending(x => x.Id));
            var total = totalCount;
            return JsonConvert.SerializeObject(new { totalRows = total, result = jsonData });
        }

        [Route("~/Api/Visitor/DeleteVisitor")]
        [HttpPost]
        public ReturnResult DeleteVisitor([FromBody] VisitorDetailsVM visitorVM)
        {
            try
            {
                var result = _visitorHelper.DeleteVisitor(visitorVM);
                return new ReturnResult { Message = (result == true ? "Success" : "Failure"), Success = (result == true ? true : false) };
            }
            catch (Exception ex)
            {
                return new ReturnResult { Message = "Failure", Success = false };
            }
            return null;
        }
    }
}
