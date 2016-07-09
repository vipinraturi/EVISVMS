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
using System.Web.Http;

namespace Evis.VMS.UI.Controllers.ApiControllers
{
    public partial class VisitorController : BaseApiController
    {
        public static IList<VisitorDetailsVM> lstVisitorDetails = new List<VisitorDetailsVM>();


        public readonly VisitorHelper _visitorHelper = null;

        public VisitorController()
        {
            _visitorHelper = new VisitorHelper();
        }

        [Route("~/Api/Visitor/SaveVisitor")]
        [HttpPost]
        public ReturnResult SaveVisitor([FromBody] VisitorDetailsVM visitorDetailsVM)
        {

            var isVisitorExisit = false;
            var result = false;

            if (visitorDetailsVM.IsInsert)
            {
                isVisitorExisit = _visitorHelper.IsVisitorExist(visitorDetailsVM);                
            }

            if (!isVisitorExisit)
            {
                result = _visitorHelper.SaveVisitor(visitorDetailsVM);
                return new ReturnResult { Message = "Success" , Success = true  };
            }

            return new ReturnResult { Message = "Failure", Success =  false };
        }

        [Route("~/Api/Visitor/GetVisitorData")]
        [HttpPost]
        public string GetVisitorData(int pageIndex, int pageSize, string sortField = "", string sortOrder = "ASC", string globalSearch = "")
        {
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

            var lstVisitorsFromDb = _visitorHelper.GetAllVisitorsData(globalSearch, pageIndex, pageSize, sortField, sortOrder, out totalCount);
            var jsonData = JsonConvert.SerializeObject(lstVisitorsFromDb);
            var total = totalCount;
            return JsonConvert.SerializeObject(new { totalRows = total, result = jsonData });
        }

        [Route("~/Api/Visitor/DeleteVisitor")]
        [HttpPost]
        public ReturnResult DeleteVisitor([FromBody] VisitorDetailsVM visitorVM)
        {
            var result = _visitorHelper.DeleteVisitor(visitorVM);
            return new ReturnResult { Message = (result == true ? "Success" : "Failure"), Success = (result == true ? true : false) };
        }
    }
}
