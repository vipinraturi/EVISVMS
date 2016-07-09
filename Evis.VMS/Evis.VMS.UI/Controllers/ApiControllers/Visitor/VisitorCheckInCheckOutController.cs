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
using System.Web.Http;

namespace Evis.VMS.UI.Controllers.ApiControllers
{
    public partial class VisitorController 
    {
        [Route("~/Api/VisitorManagement/GetVisitorDetail")]
        [HttpPost]
        public string GetVisitorDetail(string globalSearch)
        {
            var vistorData = new VisitorCheckInCheckOutHelper().GetVisitorsDetail(globalSearch);
            
            var jsonData = JsonConvert.SerializeObject(vistorData);
            return JsonConvert.SerializeObject(new { result = jsonData });
        }

        [Route("~/Api/VisitorManagement/GetVisitorDetailsAutoComplete")]
        [HttpPost]
        public string GetVisitorDetailsAutoComplete(string globalSearch, int pageIndex, int pageSize, string sortField = "", string sortOrder = "ASC")
        {
            var vistorAutoCompleteDataList = new VisitorCheckInCheckOutHelper().GetVisitorsAutoCompleteData(globalSearch);

            var jsonData = JsonConvert.SerializeObject(vistorAutoCompleteDataList);
            return JsonConvert.SerializeObject(new { result = jsonData });
        }

        [Route("~/Api/VisitorManagement/GetVisitorCheckInHistory")]
        [HttpPost]
        public string GetVisitorCheckInHistory(long visitorId, int pageIndex, int pageSize, string sortField = "", string sortOrder = "ASC")
        {
            var vistorCheckInHistoryList = new VisitorCheckInCheckOutHelper().GetVisitorCheckInHistory(visitorId, pageIndex, pageSize);

            var jsonData = JsonConvert.SerializeObject(vistorCheckInHistoryList);
            return JsonConvert.SerializeObject(new { totalRows = vistorCheckInHistoryList.Count(), result = jsonData });
        }

        [Route("~/Api/VisitorManagement/SaveVisitorCheckIn")]
        [HttpPost]
        public ReturnResult SaveVisitorCheckIn([FromBody] VisitorCheckInVM visitorCheckInVM)
        {
            var result = new VisitorCheckInCheckOutHelper().SaveVisitorCheckIn(visitorCheckInVM);
            return new ReturnResult { Message = (result == true ? "Success" : "Failure"), Success = (result == true ? true : false) };
        }
    }
}
