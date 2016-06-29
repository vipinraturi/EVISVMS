using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Evis.VMS.Data.Model.Entities
{
    public class VisitDetails:BaseEntity<long>
    {
        public VisitDetails() { }

        [Required]
        public long VisitorId { get; set; }

        [Required]
        [MaxLength(100)]
        public string ContactPerson { get; set; }

        [Required]
        public int NoOfPerson { get; set; }

        [MaxLength(200)]
        public string PurposeOfVisit { get; set; }

        [Required]
        public DateTime CheckIn { get; set; }

        public DateTime? CheckOut { get; set; }

        [Required]
        [MaxLength(40)]
        public string CreatedBy { get; set; }

        [Required]
        public int CheckInGate { get; set; }

        public int? CheckOutGate { get; set; }

        [ForeignKey("VisitorId")]
        public virtual VisitorMaster VisitorMaster { get; set; }

        [ForeignKey("CheckInGate")]
        public virtual GateMaster GateMaster { get; set; }
    }
}
