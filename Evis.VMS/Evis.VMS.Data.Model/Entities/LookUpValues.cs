using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Evis.VMS.Data.Model.Entities
{
    public class LookUpValues:BaseEntity<int>
    {
        public LookUpValues() {  }

        [Required]
        public int LookUpTypeId { get; set; }

        [Required]
        [MaxLength(50)]
        public string LookUpValue { get; set; }

        [MaxLength(100)]
        public string Description { get; set; }

        [ForeignKey("LookUpTypeId")]
        public virtual LookUpType LookUpType { get; set; }
    }
}
