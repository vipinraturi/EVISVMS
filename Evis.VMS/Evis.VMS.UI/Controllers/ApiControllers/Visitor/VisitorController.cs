﻿/********************************************************************************
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
            var result = _visitorHelper.SaveVisitor(visitorDetailsVM);
            return new ReturnResult { Message = (result == true ? "Success" : "Failure"), Success = (result == true ? true : false) };
        }

        [Route("~/Api/Visitor/GetVisitorData")]
        [HttpPost]
        public string GetVisitorData(string globalSearch, int pageIndex, int pageSize, string sortField = "", string sortOrder = "ASC")
        {
            var lstVisitorsFromDb = _visitorHelper.GetAllVisitorsData(globalSearch, pageIndex, pageSize, sortField, sortOrder);
            var jsonData = JsonConvert.SerializeObject(lstVisitorsFromDb);
            var total = lstVisitorsFromDb.Count();
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
