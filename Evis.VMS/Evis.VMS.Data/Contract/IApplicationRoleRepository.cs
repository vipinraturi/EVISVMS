/********************************************************************************
 * Company Name : Visitor's Management System
 * Team Name    : Evis Dev Team
 * Author       : Junaid Ameen
 * Created On   : 22/06/2016
 * Description  : 
 *******************************************************************************/

using Evis.VMS.Data.Model.Entities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Evis.VMS.Data.Contract
{
    public interface IApplicationRoleRepository
    {
        Task<IEnumerable<ApplicationRole>> GetAllAsync();
        Task<IEnumerable<ApplicationRole>> GetAsync(Expression<Func<ApplicationRole, bool>> @where, bool allowSystemAdmin = false);
        Task<ApplicationRole> FindByIdAsync(string id);
        Task<ApplicationRole> FindByNameAsync(string name);
        Task InsertAsync(string name, string description);
        Task<IdentityResult> UpdateAsync(ApplicationRole user);
        Task DeleteAsync(string roleId);
        Task DeleteAsync(ApplicationRole role);
        Task<IEnumerable<ApplicationRole>> GetManyAsync(Expression<Func<ApplicationRole, bool>> @where);
    }
}
