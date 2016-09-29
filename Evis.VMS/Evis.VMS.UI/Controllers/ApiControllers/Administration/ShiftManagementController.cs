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
using Evis.VMS.Utilities;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Globalization;
using System.Data.Entity.SqlServer;
using System.Data.Entity;


namespace Evis.VMS.UI.Controllers.ApiControllers
{
    public partial class AdministrationController
    {
        [Route("~/Api/ShiftAssignment/GetAllShift")]
        [HttpPost]
        public ShiftManagementResponseVM GetAllShifts(ShiftManagementRequest request)
        {
            var header = _shiftManagemetHelper.GetHeaders(DateTime.Now.AddDays(-12), DateTime.Now);
            var body = _shiftManagemetHelper.GetShiftData(DateTime.Now.AddDays(-12), DateTime.Now, request.BuildingId, request.GateId);

            return new ShiftManagementResponseVM { Header = header, Body = body };
        }

    }
}
