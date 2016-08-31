/********************************************************************************
 * Company Name : Visitor's Management System
 * Team Name    : Evis Dev Team
 * Author       : Junaid Ameen
 * Created On   : 24/06/2016
 * Description  : 
 *******************************************************************************/

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Evis.VMS.Data.Model.Entities
{
    public class BuildingMaster : BaseEntity<int>
    {
        public BuildingMaster() { }

        public int OrganizationId { get; set; }

        [Required]
        [MaxLength(100)]
        public string BuildingName { get; set; }

        //[Required]
        public int? CityId { get; set; }


        //[Required]
        [MaxLength(50)]
        public string EmailId { get; set; }

        [Required]
        [MaxLength(15)]
        public string ContactNumber { get; set; }

        [MaxLength(15)]
        public string FaxNumber { get; set; }

        [MaxLength(50)]
        public string WebSite { get; set; }

        [Required]
        [MaxLength(500)]
        public string Address { get; set; }

        [Required]
        public string ZipCode { get; set; }

        [ForeignKey("OrganizationId")]
        public virtual Organization Organization { get; set; }

        [ForeignKey("CityId")]
        public virtual LookUpValues CityMaster { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public string UpdatedBy { get; set; }

        public string OtherCountry { get; set; }
        public string OtherState { get; set; }
        public string OtherCity { get; set; }
    }
}
