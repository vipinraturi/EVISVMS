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
    public class Organization : BaseEntity<int>
    {
        public Organization() { }

        [Required]
        [MaxLength(100)]
        public string CompanyName { get; set; }

        [MaxLength(50)]
        public string WebSite { get; set; }

        public int? CountryId { get; set; }
        [ForeignKey("CountryId")]
        public virtual LookUpValues CountryMaster { get; set; }

        public string ImagePath { get; set; }

        public string ThemeName { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public string UpdatedBy { get; set; }
    }
}
