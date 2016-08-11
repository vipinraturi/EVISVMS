/********************************************************************************
 * Company Name : East Vision Information System
 * Team Name    : EVIS IT Team
 * Author       : Vipin Raturi
 * Created On   : 06/05/2016
 * Description  : 
 *******************************************************************************/

using Evis.VMS.Business;
using Evis.VMS.UI.HelperClasses;
using Evis.VMS.UI.ViewModel;
using Evis.VMS.Utilities;
using Microsoft.Reporting.WebForms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace Evis.VMS.UI.Controllers.ApiControllers
{
    public partial class ReportController
    {
        public readonly VisitorDetailsReportHelper _visitorDetailsReportHelper = null;
        public readonly UserService _userService = null;

        public ReportController()
        {
            _visitorDetailsReportHelper = new VisitorDetailsReportHelper();
            _userService = new UserService();
        }

        [Route("~/Api/VisitorsDetails/GetVisitorsDetails")]
        [HttpPost]
        public string GetVisitorData(string search, int pageIndex, int pageSize, string sortField = "", string sortOrder = "ASC", string globalSearch = "")
        {
            if (string.IsNullOrEmpty(sortField))
            {
                sortField = "VisitorName";
            }

            int totalCount = 0;
            pageIndex = (pageIndex - 1);

            var result = _visitorDetailsReportHelper.GetVisitorData(search, pageIndex, pageSize, sortField, sortOrder, out totalCount);

            var jsonData = JsonConvert.SerializeObject(result.OrderBy(x => x.VisitorName));
            return JsonConvert.SerializeObject(new { totalRows = totalCount, result = jsonData });
        }

    }

}
