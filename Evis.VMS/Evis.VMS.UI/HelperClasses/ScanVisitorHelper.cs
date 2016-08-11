/********************************************************************************
 * Company Name : East Vision Information System
 * Team Name    : EVIS IT Team
 * Author       : Vipin Raturi
 * Created On   : 06/05/2016
 * Description  : 
 *******************************************************************************/

using Evis.VMS.UI.ViewModel;
using MODI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Evis.VMS.UI.HelperClasses
{
    public class ScanVisitorHelper
    {
        ScanVisitorVM obj = null;

        public ScanVisitorHelper()
        {
            obj = new ScanVisitorVM();
        }

        public ScanVisitorVM ScanDetails(string item)
        {
            Document modiDocument = new Document();
            var filePath = @"E:\Ashish_3\Evis VMS\Evis.VMS\Evis.VMS.UI\images\VisitorIdentityImages\file_" + item + "";
            modiDocument.Create(filePath);
            modiDocument.OCR(MiLANGUAGES.miLANG_ENGLISH);
            MODI.Image modiImage = (modiDocument.Images[0] as MODI.Image);
            string extractedText = modiImage.Layout.Text.Replace(Environment.NewLine, "<br />").Trim(new char[] { '\\' });
            string[] result1 = extractedText.Split('>');

            if (extractedText.Contains("United Arab Emêrates") && extractedText.Contains("Idenmy Card") && extractedText.Contains("Date of Birth"))
            //Front and back page of emirates id card
            {
                if (obj.IDNumber != null)
                {
                    obj.IDNumber = result1[3].Replace("<br /", "");
                }
                if (obj.VisitorName != null)
                {
                    obj.VisitorName = result1[5].Replace("<br /", "").Replace("Name:", "");
                }
                if (obj.Nationality != null)
                {
                    obj.Nationality = result1[7].Replace("<br /", "").Replace("NaonahIy:", "");
                }
                if (obj.TypeOfCard != null)
                {
                    obj.TypeOfCard = "Emirates Id";
                }
                if (obj.Gender != null)
                {
                    obj.Gender = result1[0].Split(':')[1]; ;
                }
                if (obj.DateOfBirth != null)
                {
                    obj.DateOfBirth = result1[0].Split(':')[2].Replace("<br /", " ").Replace("Date of Birth", " ").Replace("j1", "");
                }
            }
            else if (extractedText.Contains("United Arab Emêrates") && extractedText.Contains("Idenmy Card"))
            //Front page of emirates id card
            {
                if (obj.IDNumber != null)
                {
                    obj.IDNumber = result1[3].Replace("<br /", "");
                }
                if (obj.VisitorName != null)
                {
                    obj.VisitorName = result1[5].Replace("<br /", "").Replace("Name:", "");
                }
                if (obj.Nationality != null)
                {
                    obj.Nationality = result1[7].Replace("<br /", "").Replace("NaonahIy:", "");
                }
                if (obj.TypeOfCard != null)
                {
                    obj.TypeOfCard = "Emirates Id";
                }
            }
            else if (extractedText.Contains("Date of Birth"))
            {
                //Back page of emirates id card 
                if (obj.Gender != null)
                {
                    obj.Gender = result1[0].Split(':')[1]; ;
                }
                if (obj.DateOfBirth != null)
                {
                    obj.DateOfBirth = result1[0].Split(':')[2].Replace("<br /", " ").Replace("Date of Birth", " ").Replace("j1", "");
                }
                if (obj.TypeOfCard != null)
                {
                    obj.TypeOfCard = "Emirates Id";
                }
            }
            else if (extractedText.Contains("License No"))
            //Driving licence
            {
                if (obj.LienceNo != null)
                {
                    obj.LienceNo = result1[16].Replace("License No.", "").Replace("<br /", "");
                }
                if (obj.TypeOfCard != null)
                {
                    obj.TypeOfCard = "License";
                }
            }
            else //visiting card
            {
                if (obj.EmailAddress != null)
                {
                    obj.EmailAddress = "";
                }
                if (obj.ContactNumber != null)
                {
                    obj.ContactNumber = "";
                }
            }
            modiDocument.Close();
            return obj;
        }
    }
}