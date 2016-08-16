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

namespace Evis.VMS.UI.ViewModel
{
    public class ScanVisitorVM
    {

        public string VisitorName { get; set; }
        public string Gender { get; set; }
        public string DateOfBirth { get; set; }
        public string TypeOfCard { get; set; }
        public string IDNumber { get; set; }
        public string CompanyName { get; set; }
        public string EmailAddress { get; set; }
        public string ContactNumber { get; set; }
        public string Nationality { get; set; }
        public string LienceNo { get; set; }
    }

}