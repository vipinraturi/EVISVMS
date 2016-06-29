﻿/********************************************************************************
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
        public int CompanyId { get; set; }

        public string VisitorName { get; set; }

        public string EmailAddress { get; set; }

        public int Gender { get; set; }

        public DateTime DOB { get; set; }

        public int TypeOfCard { get; set; }

        public string IdNo { get; set; }

        public int Nationality { get; set; }

        public string ContactNo { get; set; }

        public string ContactAddress { get; set; }

        public bool IsInsert { get; set; }
    }
}