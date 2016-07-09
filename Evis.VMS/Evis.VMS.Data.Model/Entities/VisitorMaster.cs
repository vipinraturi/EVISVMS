using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Evis.VMS.Data.Model.Entities
{
    public class VisitorMaster:BaseEntity<long>
    {
        public VisitorMaster() { }

        [Required]
        [MaxLength(100)]
        public string VisitorName { get; set; }

       
        [Required]
        public int GenderId { get; set; }

        public DateTime? DOB { get; set; }

        [MaxLength(15)]
        public string ContactNo { get; set; }

        [MaxLength(40)]
        public string EmailId { get; set; }

        public int? Nationality { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        [ForeignKey("Nationality")]
        public virtual LookUpValues CountryMaster { get; set; }

        [ForeignKey("GenderId")]
        public virtual LookUpValues GenderMaster { get; set; }

        [ForeignKey("TypeOfCardId")]
        public virtual LookUpValues TypeOfCard { get; set; }


        public int TypeOfCardId { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        [MaxLength(250)]
        public string Address { get; set; }

        [MaxLength(20)]
        public string IdNo { get; set; }

        public string ImagePath { get; set; }

    }
}
