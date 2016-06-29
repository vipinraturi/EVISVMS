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
    public class ShitfAssignment : BaseEntity<int>
    {
        public ShitfAssignment() { }

        [Required]
        public string UserId { get; set; }
        
        [Required]
        public int ShitfId { get; set; }

        [Required]
        public int BuildingId { get; set; }
        
        [Required]
        public int GateId { get; set; }

        [Required]
        public DateTime FromDate { get; set; }

        [Required]
        public DateTime ToDate { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        [ForeignKey("ShitfId")]
        public virtual ShitfMaster Shitfs { get; set; }

        [ForeignKey("BuildingId")]
        public virtual BuildingMaster BuildingMaster { get; set; }

        [ForeignKey("GateId")]
        public virtual GateMaster Gates { get; set; }
    }
}
