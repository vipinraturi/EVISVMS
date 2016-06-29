using System.ComponentModel.DataAnnotations;

namespace Evis.VMS.Data.Model.Entities
{
    public class LookUpType:BaseEntity<int>
    {
        public LookUpType() { }

        [Required]
        [MaxLength(50)]
        public string TypeName { get; set; }

        [MaxLength(50)]
        public string TypeCode { get; set; }

        [MaxLength(100)]
        public string Description { get; set; }
    }
}
