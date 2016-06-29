/********************************************************************************
 * Company Name : Visitor's Management System
 * Team Name    : Evis Dev Team
 * Author       : Junaid Ameen
 * Created On   : 24/06/2016
 * Description  : 
 *******************************************************************************/

using System;
using System.ComponentModel.DataAnnotations;

namespace Evis.VMS.Data.Model.Entities
{
    public class ShitfMaster : BaseEntity<int>
    {
        public ShitfMaster() { }

        [Required]
        [MaxLength(50)]
        public string ShitfName { get; set; }

        [Required]
        public TimeSpan FromTime { get; set; }

        [Required]
        public TimeSpan ToTime { get; set; }
    }
}
