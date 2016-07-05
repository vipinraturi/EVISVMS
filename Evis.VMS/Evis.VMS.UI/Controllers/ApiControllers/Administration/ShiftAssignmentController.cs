/********************************************************************************
 * Company Name : East Vision Information System
 * Team Name    : EVIS IT Team
 * Author       : Vipin Raturi
 * Created On   : 06/05/2016
 * Description  : 
 *******************************************************************************/

using Evis.VMS.UI.HelperClasses;
using Evis.VMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Evis.VMS.UI.Controllers.ApiControllers
{
    public partial class AdministrationController 
    {
        [Route("~/Api/ShiftAssignment/GetAllShiftAssignment")]
        [HttpGet]
        public IEnumerable<GeneralDropDownVM> GetAllShiftAssignment()
        {
            var result = _genericService.GateMaster.GetAll().Where(x => x.IsActive == true)
                .Select(y => new GeneralDropDownVM { Id = y.Id, Name = y.GateNumber });
            return result;
        }
    }
}
