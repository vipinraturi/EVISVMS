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
    public class VisitorController : Controller
    {
        public ActionResult _ScanVisitor()
        {
            return View();
        }

        public ActionResult _ManageVisitorManually()
        {
            return View();
        }

        public ActionResult _VisitorCheckInCheckout()
        {
            return View();
        }
	}
}