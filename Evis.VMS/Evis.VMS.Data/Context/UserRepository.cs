/********************************************************************************
 * Company Name : Visitor's Management System
 * Team Name    : Evis Dev Team
 * Author       : Junaid Ameen
 * Created On   : 22/06/2016
 * Description  : 
 *******************************************************************************/

using Evis.VMS.Data.Contract;
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
using Evis.VMS.Data.DBContext;
using Evis.VMS.Data;
using System.Security.Claims;

namespace Evis.VMS.Data.Context
{
    public class UserRepository
    {
        readonly UserManager<ApplicationUser> _userManager;
        readonly IApplicationRoleRepository _applicationRoleRepository;
        readonly IUnitOfWork _unitOfWork;

        public UserRepository()
        {
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new VMSContext()));
            _applicationRoleRepository = new ApplicationRoleRepository();
            _unitOfWork = new UnitOfWork();
        }

        public async Task<ApplicationUser> FindAsync(string userName, string password)
        {
            return await _userManager.FindAsync(userName, password);
        }

        public async Task<ApplicationUser> GetAsync(Expression<Func<ApplicationUser, bool>> where)
        {
            return await _userManager.Users.FirstOrDefaultAsync(where);
        }

        public async Task InsertAsync(ApplicationUser user, string password, string userRole)
        {
            var superAdminRole = await GetSuperAdminRole();

            if (userRole == superAdminRole.Id)
            {
                throw new ApplicationException("Adding this role is not permitted");
            }

            // Roles
            user.Roles.Add(new IdentityUserRole() { UserId = user.Id, RoleId = userRole });

            // Defaults for New User Creation
            user.EmailConfirmed = false;
            user.PhoneNumberConfirmed = false;
            user.TwoFactorEnabled = false;
            user.LockoutEnabled = false;
            user.AccessFailedCount = 0;
            user.IsActive = true;

            await _userManager.CreateAsync(user, password);
        }

        public async Task<ApplicationUser> UpdateAsync(ApplicationUser user, string userRole)
        {
            if (!string.IsNullOrEmpty(userRole) && user.Roles.FirstOrDefault().RoleId != userRole)
            {
                user.Roles.Remove(user.Roles.FirstOrDefault());
                user.Roles.Add(new IdentityUserRole() { UserId = user.Id, RoleId = userRole });
            }

            try
            {
                await _userManager.UpdateAsync(user);
            }
            catch (Exception ex)
            {

            }
            return user;
        }

        public async Task<ClaimsIdentity> SignInAsync(ApplicationUser user)
        {
            return await _userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var superAdminRole = await GetSuperAdminRole();
            return users.Where(p => (p.Roles.Count == 0) || (p.Roles.Count > 0 && p.Roles.FirstOrDefault().RoleId != superAdminRole.Id)).ToList();
        }

        public async Task<IEnumerable<ApplicationUser>> GetManyAsync(Expression<Func<ApplicationUser, bool>> @where)
        {
            await Task.Delay(1);
            var users = _userManager.Users.Where(@where);
            var superAdminRole = await GetSuperAdminRole();
            users = users.Where(p => (p.Roles.Count == 0) || (p.Roles.Count > 0 && p.Roles.FirstOrDefault().RoleId != superAdminRole.Id));

            return users;
        }

        public async Task DeleteAsync(string userId)
        {
            var systemAdminRole = await GetSuperAdminRole();
            var user = await GetAsync(x => x.Id == userId);

            if (user.Roles.Count > 0 && user.Roles.FirstOrDefault().RoleId == systemAdminRole.Id)
            {
                throw new ApplicationException("Deleting this role is not permitted");
            }
            await _userManager.DeleteAsync(user);
        }

        public async Task DeleteAsync(ApplicationUser user)
        {
            var systemAdminRole = await GetSuperAdminRole();
            if (user.Roles.Count > 0 && user.Roles.FirstOrDefault().RoleId == systemAdminRole.Id)
            {
                throw new ApplicationException("Deleting this role is not permitted");
            }

            await _userManager.DeleteAsync(user);
        }

        private async Task<ApplicationRole> GetSuperAdminRole()
        {
            var systemAdminRole = await _applicationRoleRepository.GetAsync(p => p.Name.ToUpper() == "SUPERADMIN", true);
            return systemAdminRole.FirstOrDefault();
        }
    }
}
