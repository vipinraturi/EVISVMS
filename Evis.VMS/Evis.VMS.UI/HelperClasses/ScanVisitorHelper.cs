﻿/********************************************************************************
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
using Evis.VMS.Business;
namespace Evis.VMS.UI.HelperClasses
{
    public class ScanVisitorHelper
    {
        ScanVisitorVM objScannedData = null;
        GenericService _genericSer = null;
       
        public ScanVisitorHelper()
        {
            objScannedData = new ScanVisitorVM();
            _genericSer = new GenericService();
        }

        public ScanVisitorVM ScanDetails(string item)
        {
            var modiDocument = new Document();
            var rootImagePath = System.Web.HttpContext.Current.Server.MapPath(@"~/images");
            var filePath = string.Format("{0}\\VisitorIdentityImages\\{1}", rootImagePath, item); 
            modiDocument.Create(filePath);
            modiDocument.OCR(MiLANGUAGES.miLANG_ENGLISH);
            MODI.Image modiImage = (modiDocument.Images[0] as MODI.Image);
            var actualtText = modiImage.Layout.Text;

            var extractedText = modiImage.Layout.Text.Replace(Environment.NewLine, "<br />").Trim(new char[] { '\\' });
            string[] resultSplitted = extractedText.Split('>');
            if (objScannedData.Nationality != null)
            {
                var data = _genericSer.LookUpValues.GetAll().Where(x => x.LookUpValue == objScannedData.Nationality).FirstOrDefault();
                if (data != null)
                {
                    objScannedData.NationalityId = data.Id;
                }
                else
                {
                    objScannedData.NationalityId = 38;
                }
            }
            if (extractedText.Contains("United Arab Emêrates") || extractedText.Contains("United Arab Emirates") 
                || extractedText.Contains("Idenmy Card"))
            //Front page of emirates id card
            {
                objScannedData.TypeOfCard = "Emirates Id";
                if (resultSplitted.Length >= 4 && !string.IsNullOrEmpty(resultSplitted[3]))
                {
                    objScannedData.IDNumber = resultSplitted[3].Replace("<br /", "").Replace(" ", "");
                }
                if (resultSplitted.Length >= 6 && !string.IsNullOrEmpty(resultSplitted[5]))
                {
                    objScannedData.VisitorName = resultSplitted[5].Replace("<br /", "").Replace("Name:", "").Replace("Name","");
                    //.Replace(" ", "")
                }
                if (resultSplitted.Length >= 8 && !string.IsNullOrEmpty(resultSplitted[7]))
                {
                    objScannedData.Nationality = resultSplitted[7].Replace("<br /", "").Replace("NaonahIy:", "").Replace("Nationality:", "").Replace(" ", "");

                }
            }
            else if (extractedText.Contains("Date of Birth") || extractedText.Contains("Dale B.rlh"))
            {
                objScannedData.TypeOfCard = "Emirates Id";
                //Back page of emirates id card 
                if (resultSplitted.Length >= 1 && !string.IsNullOrEmpty(resultSplitted[0]))
                {
                    //		resultSplitted[0].Split(' ')[5][0]	48 '0'	char
                    if (resultSplitted[0].Contains(":") && !resultSplitted[0].Contains("Dale B.rlh"))
                    {

                        objScannedData.Gender = resultSplitted[0].Split(':')[1];
                        objScannedData.DateOfBirth = resultSplitted[0].Split(':')[2].Replace("<br /", " ").Replace("Date of Birth", " ").Replace("j1", "").Replace(" ", "");    
                    }
                    else
                    {
                        objScannedData.Gender = resultSplitted[0].Split(' ')[1];
                        objScannedData.DateOfBirth = (resultSplitted[0].Split(' ')[5][0].ToString() + resultSplitted[0].Split(' ')[5][1].ToString() 
                            + "/" + resultSplitted[0].Split(' ')[5][3].ToString() + resultSplitted[0].Split(' ')[5][4].ToString() 
                            + "/" + resultSplitted[0].Split(' ')[5][6].ToString() + resultSplitted[0].Split(' ')[5][7].ToString()
                            + resultSplitted[0].Split(' ')[5][8].ToString()
                            + resultSplitted[0].Split(' ')[5][9].ToString());    

                    }
                }
            }
            else if (extractedText.Contains("License No"))
            //Driving licence
            {
                objScannedData.TypeOfCard = "License";
                if (resultSplitted.Length >= 4 && !string.IsNullOrEmpty(resultSplitted[3]))
                {
                    objScannedData.LienceNo = resultSplitted[16].Replace("License No.", "").Replace("<br /", "");
                }
            }
            else //visiting card
            {
                objScannedData.CompanyName = "";
                //logic to retrieve email address
                foreach (var curreIitem in extractedText.Split('<'))
                {
                    if (curreIitem.Contains("@") || curreIitem.ToLower().Contains("email"))
                    {
                        objScannedData.EmailAddress = curreIitem
                                            .Replace("br />", "")
                                            .Replace(":", "")
                                            .Replace("—", "")
                                            .Replace("Email", "").Replace("email", "")
                                            .Replace(" ", "");
                        break;
                    }
                }
                //logic to retrieve contact number
                foreach (var curreIitem in extractedText.Split('<'))
                {
                    if (curreIitem.Contains("+") || curreIitem.ToLower().Contains("tel")
                        || curreIitem.ToLower().Contains("cell") || curreIitem.ToLower().Contains("mob"))
                    {
                        objScannedData.ContactNumber = curreIitem.Replace("br />", "")
                            .Replace(":", "")
                            .Replace("Tel", "").Replace("tel", "")
                            .Replace("Mob", "").Replace("mob", "")
                            .Replace("Cell", "").Replace("cell", "")
                            .Replace(" ", "");
                        break;
                    }
                }
            }
            objScannedData.CompanyName = "For Testing";

            modiDocument.Close();
            return objScannedData;
        }
    }
}
//Review Comment
//1) No Server.MapPath, it was hardcode Ashish path
//2) Wrong logic,   if (obj.IDNumber != null). It will never go inside this condition
//3) for type of card, no other if condition needed. if coming witin condition, it should show. 
//4) No logics written, if user is scanning on driving licenece
//5) No logic for visiting card