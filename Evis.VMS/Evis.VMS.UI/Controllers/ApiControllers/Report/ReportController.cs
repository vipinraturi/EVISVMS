/********************************************************************************
 * Company Name : East Vision Information System
 * Team Name    : EVIS IT Team
 * Author       : Vipin Raturi
 * Created On   : 06/05/2016
 * Description  : 
 *******************************************************************************/

using Evis.VMS.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Evis.VMS.UI.HelperClasses;
namespace Evis.VMS.UI.Controllers.ApiControllers
{
    public partial class ReportController : BaseApiController
    {
        private readonly UserService _userService = null;
        public readonly VisitorDetailsReportHelper _visitorDetailsReportHelper = null;
        public readonly ShiftDetailsReportHelper _ShiftDetailsReportHelper = null;
        private readonly ApplicationRoleService _applicationRoleService = null;
        private readonly GenericService _genericService = null;
        //public readonly VisitorDetailsReportHelper _visitorDetailsReportHelper = null;
        public ReportController()
        {

            _userService = new UserService();

            _visitorDetailsReportHelper = new VisitorDetailsReportHelper();
            _applicationRoleService = new ApplicationRoleService();
            _genericService = new GenericService();
            _ShiftDetailsReportHelper = new ShiftDetailsReportHelper();
    }
        }
}
