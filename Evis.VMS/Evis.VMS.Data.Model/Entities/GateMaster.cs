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
    public class GateMaster : BaseEntity<int>
    {
        public GateMaster() { }

        [Required]
        public int BuildingId { get; set; }
       
        [Required]
        [MaxLength(50)]
        public string GateNumber { get; set; }

        [ForeignKey("BuildingId")]
        public virtual BuildingMaster BuildingMaster { get; set; }
    }
}
