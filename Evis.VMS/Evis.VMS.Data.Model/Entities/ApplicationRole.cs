/********************************************************************************
 * Company Name : Visitor's Management System
 * Team Name    : Evis Dev Team
 * Author       : Junaid Ameen
 * Created On   : 24/06/2016
 * Description  : 
 *******************************************************************************/

using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evis.VMS.Data.Model.Entities
{
    public sealed class ApplicationRole : IdentityRole
    {
        public ApplicationRole() { }

        public ApplicationRole(string name, string description)
            : base(name)
        {
            Description = description;
        }

        public string Description { get; set; }

        public bool IsActive { get; set; }
    }
}
