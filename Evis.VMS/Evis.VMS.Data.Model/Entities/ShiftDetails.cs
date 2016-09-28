using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evis.VMS.Data.Model.Entities
{
    public class ShiftDetails : BaseEntity<int>
    {
        public string SecurityID { get; set; }

        public int ShiftID { get; set; }

        public DateTime ShiftDate { get; set; }

        [ForeignKey("SecurityID")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        [ForeignKey("ShiftID")]
        public virtual ShitfMaster Shitfs { get; set; }
    }
}
