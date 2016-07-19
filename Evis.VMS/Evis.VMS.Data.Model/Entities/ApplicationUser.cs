/********************************************************************************
 * Company Name : Visitor's Management System
 * Team Name    : Evis Dev Team
 * Author       : Junaid Ameen
 * Created On   : 24/06/2016
 * Description  : 
 *******************************************************************************/

using Evis.VMS.Data.Model.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evis.VMS.Data.Model.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser() { }

        public int? OrganizationId { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        [Required]
        public int GenderId { get; set; }
        
        public DateTime? DateOfBirth { get; set; }

        public int? IdNumber { get; set; }

        public int Nationality { get; set; }

        [MaxLength(100)]
        public string ContactAddress { get; set; }

        public bool IsActive { get; set; }

        public int? CardTypeId { get; set; }
        
        [ForeignKey("GenderId")]
        public virtual LookUpValues Gender { get; set; }

        [ForeignKey("OrganizationId")]
        public virtual Organization Organization { get; set; }

        public virtual ApplicationRole ApplicationRole { get; set; }

        [ForeignKey("Nationality")]
        public virtual LookUpValues CountryMaster { get; set; }

        [ForeignKey("CardTypeId")]
        public virtual CardTypeMaster CardType { get; set; }

        [MaxLength(50)]
        public string Country { get; set; }

        [MaxLength(50)]
        public string State { get; set; }

        [MaxLength(50)]
        public string City { get; set; }

        public string ThemeName { get; set; }

    }
}
