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
    public class UsersVM
    {
        public string UserId { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string ContactNumber { get; set; }

        public int GenderId { get; set; }

        public string UserName { get; set; }

        public string RoleId { get; set; }

        public string RoleName { get; set; }

        public int? OrganizationId { get; set; }

        public string OrganizationName { get; set; }

        public int Nationality { get; set; }

        public string ThemeName { get; set; }

        public string ProfilePicturePath { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public string UpdatedBy { get; set; }

        public bool IsImageAvailable { get; set; }
    }
}