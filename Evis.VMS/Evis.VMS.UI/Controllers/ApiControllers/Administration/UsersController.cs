/********************************************************************************
 * Company Name : East Vision Information System
 * Team Name    : EVIS IT Team
 * Author       : Vipin Raturi
 * Created On   : 06/05/2016
 * Description  : 
 *******************************************************************************/

using Evis.VMS.Data.Model.Entities;
using Evis.VMS.UI.HelperClasses;
using Evis.VMS.UI.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Evis.VMS.UI.Controllers.ApiControllers
{
    public partial class AdministrationController
    {
        [Route("~/Api/Users/GetRoles")]
        [HttpGet]
        public async Task<IEnumerable<ApplicationRole>> GetRoles()
        {
            return await _applicationRoleService.GetManyAsync(x => x.Name != "SuperAdmin");
        }


        [Route("~/Api/Users/GetAllOrganizations")]
        [HttpGet]
        public IEnumerable<Organization> GetAllOrganizations()
        {
            return _genericService.Organization.GetAll();
        }
        [Route("~/Api/Users/GetUsersData")]
        [HttpPost]
        public async Task<string> GetUsersData(string globalSearch, int pageIndex, int pageSize, string sortField = "", string sortOrder = "ASC")
        {
            var getUsers = await _userService.GetAllAsync();
            var usersList = getUsers.ToList();

            var getRoles = await _applicationRoleService.GetAllAsync();
            var rolesList = getRoles.ToList();

            var result = (from users in usersList
                          join roles in rolesList on users.Roles.First().RoleId equals roles.Id
                          select new UsersVM
                          {
                              UserId = users.Id,
                              FullName = users.FullName,
                              Email = users.Email,
                              ContactNumber = users.PhoneNumber,
                              GenderId = users.GenderId,
                              UserName = users.UserName,
                              RoleId = roles.Id,
                              RoleName = roles.Name,
                              OrganizationId = users.OrganizationId

                          }).ToList();

            if (result.Count() > 0)
            {
                if (!string.IsNullOrEmpty(globalSearch))
                {
                    result = result.Where(item =>
                        item.ContactNumber.ToLower().Contains(globalSearch.ToLower()) ||
                        item.UserName.ToLower().Contains(globalSearch.ToLower()) ||
                        item.RoleName.ToLower().Contains(globalSearch.ToLower()))
                        .ToList();
                }

                bool sortAscending = (sortOrder == "ASC" ? true : false);
                if (!string.IsNullOrEmpty(sortField))
                {
                    if (!sortAscending)
                    {
                        result = result
                            .OrderBy(r => r.GetType().GetProperty(sortField).GetValue(r, null))
                            .ToList();
                    }
                    else
                    {
                        result = result
                            .OrderByDescending(r => r.GetType().GetProperty(sortField).GetValue(r, null))
                            .ToList();
                    }
                }
            }

            var data = result.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
            var jsonData = JsonConvert.SerializeObject(data);
            var total = result.Count();
            return JsonConvert.SerializeObject(new { totalRows = total, result = jsonData });
        }

        [Route("~/Api/Users/SaveUser")]
        [HttpPost]
        public async Task<ReturnResult> SaveOrganization([FromBody] UsersVM usersVM)
        {
            if (usersVM != null && string.IsNullOrEmpty(usersVM.UserId))
            {
                ApplicationUser user = new ApplicationUser();
                user.OrganizationId = usersVM.OrganizationId;
                user.FullName = usersVM.FullName;
                user.Email = user.UserName = usersVM.Email;
                user.PhoneNumber = usersVM.ContactNumber;
                user.GenderId = usersVM.GenderId;
                await _userService.InsertAsync(user, "password", usersVM.RoleId);
            }
            else
            {
                var existingUser = await _userService.GetAsync(x => x.Id == usersVM.UserId);
                if (existingUser != null)
                {
                    existingUser.OrganizationId = usersVM.OrganizationId;
                    existingUser.FullName = usersVM.FullName;
                    existingUser.Email = existingUser.UserName = usersVM.Email;
                    existingUser.PhoneNumber = usersVM.ContactNumber;
                    existingUser.GenderId = usersVM.GenderId;
                    await _userService.UpdateAsync(existingUser, usersVM.RoleId);
                }
            }
            _genericService.Commit();
            return new ReturnResult { Message = "Success", Success = true };
        }

        [Route("~/Api/Administration/DeleteUser")]
        [HttpPost]
        public async Task<ReturnResult> DeleteUser([FromBody] UsersVM usersVM)
        {
            if (usersVM != null)
            {
                var existingUser = await _userService.GetAsync(x => x.Id == usersVM.UserId);
                existingUser.IsActive = false;
                await _userService.UpdateAsync(existingUser, string.Empty);
                _genericService.Commit();
                return new ReturnResult { Message = "Success", Success = true };
            }
            return new ReturnResult { Message = "Failure", Success = false };
        }

    }
}
