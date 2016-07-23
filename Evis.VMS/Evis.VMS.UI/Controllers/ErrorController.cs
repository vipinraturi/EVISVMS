using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Evis.VMS.UI.Controllers
{
    public class ErrorController : Controller
    {
        //
        // GET: /Error/

        public ActionResult Error(int status, Exception error)
        {
            Response.StatusCode = status;
            if (status == 404)
            {
                return RedirectToAction("Error404");
            }

            else

                if (status == 500)
                {
                    return RedirectToAction("Error500");
                }

            return View(status);

        }

        //Get / Error 500 
        public ActionResult Error500()
        {
            return View("~/Views/Error/Error500.cshtml");
        }

        // Get / Error404
        public ActionResult Error404()
        {
            return View("~/Views/Error/Error404.cshtml");
        }
    }
}