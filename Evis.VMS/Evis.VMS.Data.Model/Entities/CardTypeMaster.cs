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
    public class CardTypeMaster:BaseEntity<int>
    {
        public CardTypeMaster() { }

        [Required]
        [MaxLength(50)]
        public string CardName { get; set; }

        [MaxLength(100)]
        public string Description { get; set; }
    }
}
