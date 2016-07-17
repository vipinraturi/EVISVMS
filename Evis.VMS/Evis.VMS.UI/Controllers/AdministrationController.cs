/********************************************************************************
 * Company Name : East Vision Information System
 * Team Name    : EVIS IT Team
 * Author       : Vipin Raturi
 * Created On   : 06/05/2016
 * Description  : 
 *******************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Net.Http;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using Evis.VMS.Business;

namespace Evis.VMS.UI.Controllers
{
    public class AdministrationController : Controller
    {
        private readonly UserService _userService = null;
        private readonly GenericService _genericService = null;

        public AdministrationController()
        {
            _userService = new UserService();
            _genericService = new GenericService();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _Organization()
        {
            return View();
        }

        public ActionResult _MyOrganization()
        {
            return View();
        }

        public ActionResult _Building()
        {
            return View();
        }

        public ActionResult _Gates()
        {
            return View();
        }

        public ActionResult _Users()
        {
            return View();
        }

        public ActionResult _Shifts()
        {
            return View();
        }


        public ActionResult _ShiftAssignment()
        {
            return View();
        }

        public ActionResult _Myprofile()
        {
            return View();
        }

        public ActionResult _ChangePassword()
        {
            return View();
        }
        public ActionResult _ThemeSelection()
        {
            return View();
        }

        public async Task<ActionResult> SaveUploadedFile()
        {
            bool isSavedSuccessfully = true;
            string userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            //string userId = HttpContext.Current.User.Identity.GetUserId();
            var currentUser = await _userService.GetAsync(x => x.Id == userId);
           
           // var result = _genericService.Organization.GetAll().FirstOrDefault(item => item.Id == currentUser.OrganizationId);
            string orginization = currentUser.Organization.CompanyName;
            //// result.CityMaster = null;
            string fName = "";
            try
            {
                foreach (string fileName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fileName];
                    //Save file content goes here
                    fName = file.FileName;
                    if (file != null && file.ContentLength > 0)
                    {

                        var originalDirectory = new DirectoryInfo(string.Format("{0}images\\logo//"+orginization, Server.MapPath(@"\")));

                        string fileWithPath = System.IO.Path.Combine(originalDirectory.ToString(), "logo.png");
                      //  string path = System.IO.Path(originalDirectory.ToString());
                        var fileName1 = Path.GetFileName(file.FileName);

                        bool isfolderExists = System.IO.Directory.Exists(originalDirectory.ToString());
                        bool isExists = System.IO.File.Exists(fileWithPath);
                        if (!isfolderExists)
                            System.IO.Directory.CreateDirectory(originalDirectory.ToString());
                        if (isExists)
                            System.IO.File.Delete(fileWithPath);

                        // var path = string.Format("{0}\\{1}", pathString, file.FileName);
                        file.SaveAs(fileWithPath);

                    }

                }

            }
            catch (Exception ex)
            {
                isSavedSuccessfully = false;
            }

            //return View();


            if (isSavedSuccessfully)
            {
                return Json(new { Message = fName });
            }
            else
            {
                return Json(new { Message = "Error in saving file" });
            }
        }

    }
}