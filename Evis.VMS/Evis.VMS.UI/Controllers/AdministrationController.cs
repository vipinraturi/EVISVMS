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

        public ActionResult _ShiftManagement()
        {
            return View();
        }

        public ActionResult _ShiftManagement_dynamic()
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
            string orginization = string.Empty;
            string logoPath = string.Empty;
            string imageLocation = string.Empty;
            if (currentUser.Organization == null)// in case of super admin only
            {
                imageLocation = "\\images\\logo\\main_logo.png";
                logoPath = string.Format("{0}\\images\\logo\\main_logo.png", Server.MapPath(@"\"));

            }
            else
            {
                orginization = currentUser.Organization.CompanyName;
                imageLocation = "\\images\\logo\\" + orginization + "\\logo.png";
                logoPath = string.Format("{0}images\\logo\\" + orginization, Server.MapPath(@"\"));
            }
            
            var existingOrg = _genericService.Organization.GetById(currentUser.Organization.Id);
            existingOrg.ImagePath = imageLocation;
            _genericService.Organization.Update(existingOrg);
            string fName = "";
            try
            {
                foreach (string fileName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fileName];
                    fName = string.Format("{0}_{1}", fileName, file.FileName);
                    if (file != null && file.ContentLength > 0)
                    {
                        var originalDirectory = new DirectoryInfo(logoPath);
                        string fileWithPath = System.IO.Path.Combine(originalDirectory.ToString(), "logo.png");
                        var fileName1 = Path.GetFileName(fName);

                        bool isfolderExists = System.IO.Directory.Exists(originalDirectory.ToString());
                        bool isExists = System.IO.File.Exists(fileWithPath);
                        if (!isfolderExists)
                            System.IO.Directory.CreateDirectory(originalDirectory.ToString());
                        if (isExists)
                            System.IO.File.Delete(fileWithPath);

                        file.SaveAs(fileWithPath);

                    }

                }

            }
            catch (Exception ex)
            {
                isSavedSuccessfully = false;
            }

            //return View();

            if (Request.Files.Count > 0)
            {
                if (isSavedSuccessfully)
                {
                    _genericService.Commit();
                    return Json(new { Message = fName, ImagePath = imageLocation });
                }
                else
                {
                    return Json(new { Message = "Error in saving file", ImagePath = imageLocation });
                }
            }
            return Json(new { Message = "No file to upload", ImagePath = imageLocation });
            
        }

    }
}