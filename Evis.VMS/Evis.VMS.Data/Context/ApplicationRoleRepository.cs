/********************************************************************************
 * Company Name : Visitor's Management System
 * Team Name    : Evis Dev Team
 * Author       : Junaid Ameen
 * Created On   : 22/06/2016
 * Description  : 
 *******************************************************************************/

using Evis.VMS.Data.Contract;
using Evis.VMS.Data.DBContext;
using Evis.VMS.Data.Model.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Evis.VMS.Data.Context
{
    public class ApplicationRoleRepository : IApplicationRoleRepository
    {
        readonly RoleManager<ApplicationRole> _userRoleManager;

        public ApplicationRoleRepository()
        {
            _userRoleManager = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(new VMSContext()));
        }

        public async Task<IEnumerable<ApplicationRole>> GetAllAsync()
        {
            var roles = await _userRoleManager.Roles.ToListAsync();
            return roles;
        }

        public async Task<IEnumerable<ApplicationRole>> GetAsync(Expression<Func<ApplicationRole, bool>> @where, bool allowSystemAdmin = false)
        {
            var role = await _userRoleManager.Roles.Where(@where).ToListAsync();
            return role;
        }

        public async Task<ApplicationRole> FindByIdAsync(string id)
        {
            return await _userRoleManager.FindByIdAsync(id);
        }

        public async Task<ApplicationRole> FindByNameAsync(string name)
        {
            return await _userRoleManager.FindByNameAsync(name);
        }

        public async Task InsertAsync(string name, string description)
        {
            await _userRoleManager.CreateAsync(new ApplicationRole { Name = name, Description = description, IsActive = true });
        }

        public async Task<IdentityResult> UpdateAsync(ApplicationRole role)
        {
            return await _userRoleManager.UpdateAsync(role);
        }

        public async Task DeleteAsync(string roleId)
        {
            var role = await _userRoleManager.FindByIdAsync(roleId);
            await _userRoleManager.DeleteAsync(role);
        }

        public async Task DeleteAsync(ApplicationRole role)
        {
            await _userRoleManager.DeleteAsync(role);
        }

        public async Task<IEnumerable<ApplicationRole>> GetManyAsync(Expression<Func<ApplicationRole, bool>> @where)
        {
            await Task.Delay(1);
            var roles = _userRoleManager.Roles.Where(@where);
            return roles;
        }
    }
    
}
