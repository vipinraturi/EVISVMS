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
    public class VisitorDetailsVM
    {
        public bool IsImageAvailable { get; set; }

        public long Id { get; set; }

        public string VisitorName { get; set; }

        public string EmailAddress { get; set; }

        public int Gender { get; set; }

        public DateTime DOB { get; set; }

        public int TypeOfCard { get; set; }

        public string IdNo { get; set; }

        public int Nationality { get; set; }
        
        public string NationalityVal { get; set; }

        public string ContactNo { get; set; }

        public string ContactAddress { get; set; }

        public bool IsInsert { get; set; }

        public string ImagePath { get; set; }

        public string IdentityImage1_Path { get; set; }
        public string IdentityImage2_Path { get; set; }
        public string IdentityImage3_Path { get; set; }

        public string CompanyName { get; set; }

        public DateTime CreatedOn { get; set; }

    }
}