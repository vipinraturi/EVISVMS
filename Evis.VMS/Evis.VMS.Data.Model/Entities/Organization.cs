/********************************************************************************
 * Company Name : Visitor's Management System
 * Team Name    : Evis Dev Team
 * Author       : Junaid Ameen
 * Created On   : 24/06/2016
 * Description  : 
 *******************************************************************************/

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

        [MaxLength(20)]
        public string CompanyCode { get; set; }

        [MaxLength(20)]
        public string EIN { get; set; }
        //Employer Identification Number (EIN) 
        //are defined as a nine-digit number that the IRS assigns to organizations.

        public int CityId { get; set; }

        [Required]
        [MaxLength(50)]
        public string EmailId { get; set; }

        [Required]
        [MaxLength(15)]
        public string ContactNumber { get; set; }

        [Required]
        [MaxLength(500)]
        public string ContactAddress { get; set; }

        [Required]
        [MaxLength(8)]
        public string ZipCode { get; set; }

        [MaxLength(15)]
        public string FaxNumber { get; set; }

        [MaxLength(50)]
        public string WebSite { get; set; }

        [ForeignKey("CityId")]
        public virtual LookUpValues CityMaster { get; set; }
    }
}
