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
    public class MyProfileVM
    {

        public string FullName { get; set; }

        public string PhoneNumber { get; set; }

        public int GenderId { get; set; }

        public string ContactAddress { get; set; }

        public string ProfilePicturePath { get; set; }
    }
}