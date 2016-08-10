/********************************************************************************
 * Company Name : East Vision Information System
 * Team Name    : EVIS IT Team
 * Author       : Vipin Raturi
 * Created On   : 06/05/2016
 * Description  : 
 *******************************************************************************/

using Evis.VMS.Business;
using Evis.VMS.UI.HelperClasses;
using Evis.VMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MODI;

namespace Evis.VMS.UI.Controllers
{

    public class VisitorController : Controller
    {
        public readonly VisitorHelper _visitorHelper = null;
        public readonly UserService _userService = null;
        public readonly VisitorCheckInCheckOutHelper _visitorCheckInCheckOutHelper = null;

        public VisitorController()
        {
            _visitorHelper = new VisitorHelper();
            _visitorCheckInCheckOutHelper = new VisitorCheckInCheckOutHelper();
            _userService = new UserService();
        }

        public ActionResult _ScanVisitor()
        {
            return View();
        }

        public ActionResult _ManageVisitorManually()
        {
            return View();
        }

        public ActionResult _VisitorCheckIn()
        {
            return View();
        }

        public ActionResult _VisitorCheckout()
        {
            return View();
        }

        public ActionResult SaveVisitorIdentityImages()
        {
            HttpPostedFileBase file = null;
            bool isSavedSuccessfully = true;
            string fName = "";
            var fileWithPath = string.Empty;
            try
            {
                foreach (string fileName in Request.Files)
                {
                    file = Request.Files[fileName];
                    fName = string.Format("{0}_{1}", fileName, file.FileName);
                    if (file != null && file.ContentLength > 0)
                    {
                        var directoryPath = string.Format("{0}images\\VisitorIdentityImages", Server.MapPath(@"\"));

                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }

                        var originalDirectory = new DirectoryInfo(directoryPath);
                        fileWithPath = System.IO.Path.Combine(originalDirectory.ToString(), fName);
                        var fileName1 = Path.GetFileName(fName);
                        var isExists = System.IO.File.Exists(fileWithPath);
                        
                        if (isExists)
                        { 
                            System.IO.File.Delete(fileWithPath);
                        }

                        file.SaveAs(fileWithPath);
                    }
                }
            }
            catch (Exception ex)
            {
                isSavedSuccessfully = false;
            }

            if (isSavedSuccessfully)
            {
                return Json(new { Message = fName, FilePath  = "\\images\\Visitors" + file.FileName });
            }
            else
            {
                return Json(new { Message = "Error in saving file", FilePath = "\\images\\Visitors" + file.FileName });
            }
        }


        public ActionResult SaveVisitorProfilePicture()
        {
            HttpPostedFileBase file = null;
            bool isSavedSuccessfully = true;
            string fName = "";
            var fileWithPath = string.Empty;
            try
            {
                foreach (string fileName in Request.Files)
                {
                    file = Request.Files[fileName];
                    fName = string.Format("{0}_{1}", fileName, file.FileName);
                    if (file != null && file.ContentLength > 0)
                    {
                        var directoryPath = string.Format("{0}images\\VisitorImages", Server.MapPath(@"\"));
                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }
                        var originalDirectory = new DirectoryInfo(directoryPath);
                        fileWithPath = System.IO.Path.Combine(originalDirectory.ToString(), fName);
                        var fileName1 = Path.GetFileName(fName);
                        var isExists = System.IO.File.Exists(fileWithPath);

                        if (isExists)
                        {
                            System.IO.File.Delete(fileWithPath);
                        }

                        file.SaveAs(fileWithPath);
                    }
                }
            }
            catch (Exception ex)
            {
                isSavedSuccessfully = false;
            }

            if (isSavedSuccessfully)
            {
                return Json(new { Message = fName, FilePath = "\\images\\Visitors" + file.FileName });
            }
            else
            {
                return Json(new { Message = "Error in saving file", FilePath = "\\images\\Visitors" + file.FileName });
            }
        }

        public void Capture()
        {
            var stream = Request.InputStream;
            string dump;

            using (var reader = new StreamReader(stream))
                dump = reader.ReadToEnd();

            var path = Server.MapPath("~/images/webCamImage.jpg");
            System.IO.File.WriteAllBytes(path, String_To_Bytes2(dump));
        }

        private byte[] String_To_Bytes2(string strInput)
        {
            int numBytes = (strInput.Length) / 2;
            byte[] bytes = new byte[numBytes];

            for (int x = 0; x < numBytes; ++x)
            {
                bytes[x] = Convert.ToByte(strInput.Substring(x * 2, 2), 16);
            }

            return bytes;
        }


        public async Task<JsonResult> GetVisitorsData(string searchterm)
        {
            var user = (await _userService.GetAllAsync()).Where(x => x.Id == System.Web.HttpContext.Current.User.Identity.GetUserId() && x.IsActive == true).FirstOrDefault();
            var visitorDeata = new List<VisitorJsonModel>();
            visitorDeata = _visitorCheckInCheckOutHelper.GetVisitorData(searchterm, (user == null) ? null : user.OrganizationId);
            return Json(visitorDeata, JsonRequestBehavior.AllowGet);
        }


        private string ExtractTextFromImage(string filePath)
        {
            Document modiDocument = new Document();
            modiDocument.Create(filePath);
            modiDocument.OCR(MiLANGUAGES.miLANG_ENGLISH);
            MODI.Image modiImage = (modiDocument.Images[0] as MODI.Image);
            string extractedText = modiImage.Layout.Text;
            modiDocument.Close();
            return extractedText;
        }
	}
}