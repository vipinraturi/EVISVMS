/********************************************************************************
 * Company Name : Visitor's Management System
 * Team Name    : Evis Dev Team
 * Author       : Junaid Ameen
 * Created On   : 24/06/2016
 * Description  : 
 *******************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evis.VMS.Data.Model.Entities
{
    public class BaseEntity<TEntity>
    {
        public TEntity Id { get; set; }

        public bool IsActive { get; set; }
    }
}
