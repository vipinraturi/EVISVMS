using Evis.VMS.Data.Model.Entities;
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
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Evis.VMS.UI.ViewModel
{
    public class BuildingVM
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public int OrganizationId { get; set; }
        public string BuildingName { get; set; }
        public int? CityId { get; set; }
        public string Address { get; set; }
        public int? NationalityId { get; set; }
        public string OrganizationName { get; set; }
        public string ZipCode { get; set; }
        [ForeignKey("OrganizationId")]
        public virtual Organization Organization { get; set; }
        [ForeignKey("CityId")]
        public virtual LookUpValues CityMaster { get; set; }
        public int? StateId { get; set; }
        public string EmailId { get; set; }
        public string ContactNumber { get; set; }
        public string FaxNumber { get; set; }
        public string WebSite { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public string txtcountry { get; set; }
        public string txtstate { get; set; }
        public string txtcity { get; set; }
        public string Country { get; set; }
        //public string OtherState { get; set; }
        //public string OtherCity { get; set; }
    }
}