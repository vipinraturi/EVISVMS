/********************************************************************************
 * Company Name : Visitor's Management System
 * Team Name    : Evis Dev Team
 * Author       : Junaid Ameen
 * Created On   : 22/06/2016
 * Description  : 
 *******************************************************************************/

using Evis.VMS.Data.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evis.VMS.Data.Contract
{
    public interface IUserRepository
    {
        Task<ApplicationUser> FindAsync(string userName, string password);
    }
}
