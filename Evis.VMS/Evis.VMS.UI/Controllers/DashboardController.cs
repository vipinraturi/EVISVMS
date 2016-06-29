/********************************************************************************
 * Company Name : East Vision Information System
 * Team Name    : EVIS IT Team
 * Author       : Vipin Raturi
 * Created On   : 06/05/2016
 * Description  : 
 *******************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Evis.VMS.UI.Controllers
{
    [Authorize]
    public partial class DashboardController : Controller
    {
        //
        // GET: /Dashboard/
        public ActionResult _Dashboard()
        {
            return View();
        }
	}
}