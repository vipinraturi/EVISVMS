/********************************************************************************
 * Company Name : East Vision Information System
 * Team Name    : EVIS IT Team
 * Author       : Vipin Raturi
 * Created On   : 06/05/2016
 * Description  : 
 *******************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Evis.VMS.UI.ViewModel
{
    public class OrganizationVM
    {
        public int CompanyId { get; set; }

        [Required]
        public string CompanyName { get; set; }

        [Required]
        public string EmailAddress { get; set; }

        [Required]
        public string ContactNo { get; set; }

        [Required]
        public string Address { get; set; }
        
        [Required]
        public string FaxNo { get; set; }
       
        public string ZipCode { get; set; }

        public string WebSite { get; set; }

        public bool IsInsert { get; set; }

    }
}