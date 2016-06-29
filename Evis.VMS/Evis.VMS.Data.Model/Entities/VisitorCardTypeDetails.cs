using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Evis.VMS.Data.Model.Entities
{
    public class VisitorCardTypeDetails:BaseEntity<int>
    {
        public VisitorCardTypeDetails() { }

        [Required]
        public long VisitorMasterId { get; set; }

        [Required]
        public int CardTypeId { get; set; }

        [Required]
        [MaxLength(20)]
        public string CardNo { get; set; }

        [MaxLength(100)]
        public string Description { get; set; }

        [ForeignKey("VisitorMasterId")]
        public virtual VisitorMaster VisitorMaster { get; set; }

        [ForeignKey("CardTypeId")]
        public virtual CardTypeMaster CardTypeMaster { get; set; }
    }
}
