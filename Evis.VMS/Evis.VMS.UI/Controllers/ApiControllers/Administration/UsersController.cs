﻿/********************************************************************************
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
using System.Web;
using Microsoft.AspNet.Identity;
using System.Web.Http;
using System.Web.Http.Controllers;
using Evis.VMS.Utilities;

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
        public IEnumerable<GeneralDropDownVM> GetAllOrganizations()
        {
            var organizations = _genericService.Organization.GetAll().Where(x => x.IsActive == true).Select(x => new GeneralDropDownVM { Id = x.Id, Name = x.CompanyName });
            return organizations;
        }


        [Route("~/Api/Users/GetAllCountries")]
        [HttpGet]
        public IEnumerable<GeneralDropDownVM> GetAllCountries()
        {
            var result = _genericService.LookUpValues.GetAll().Where(x => x.LookUpType.TypeName == "Country" && x.IsActive == true && x.LookUpType.IsActive == true)
                .Select(y => new GeneralDropDownVM { Id = y.Id, Name = y.LookUpValue });
            return result;
        }
        [Route("~/Api/Users/GetUsersData")]
        [HttpPost]
        public async Task<string> GetUsersData(string globalSearch, int pageIndex, int pageSize, string sortField = "", string sortOrder = "ASC")
        {
            var getUsers = (await _userService.GetAllAsync()).Where(x => x.Organization.IsActive == true).AsQueryable();

            var getRoles = (await _applicationRoleService.GetAllAsync()).AsQueryable();

            var temp = (from users in getUsers
                        join roles in getRoles on users.Roles.First().RoleId equals roles.Id
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
                            OrganizationId = users.OrganizationId,
                            Nationality = users.Nationality

                        }).AsQueryable();

            if (temp.Count() > 0)
            {
                if (!string.IsNullOrEmpty(globalSearch))
                {
                    temp = temp.Where(item =>
                        item.ContactNumber.ToLower().Contains(globalSearch.ToLower()) ||
                        item.UserName.ToLower().Contains(globalSearch.ToLower()) ||
                        item.RoleName.ToLower().Contains(globalSearch.ToLower()))
                        .AsQueryable();
                }

                //creating pager object to send for filtering and sorting
                var paginationRequest = new PaginationRequest
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    SearchText = globalSearch,
                    Sort = new Sort { SortDirection = (sortOrder == "ASC" ? SortDirection.Ascending : SortDirection.Descending), SortBy = sortField }
                };

                int totalCount = 0;
                pageIndex = (pageIndex - 1);

                IList<UsersVM> result =
                    GenericSorterPager.GetSortedPagedList<UsersVM>(temp, paginationRequest, out totalCount);

                var jsonData = JsonConvert.SerializeObject(result);
                return JsonConvert.SerializeObject(new { totalRows = totalCount, result = jsonData });
            }

            return null;
        }

        [Route("~/Api/Users/SaveUser")]
        [HttpPost]
        public async Task<ReturnResult> SaveOrganization([FromBody] UsersVM usersVM)
        {
            string message = "Error occured, please try again with valid entered data again!";
            bool success = false;
            if (usersVM != null && string.IsNullOrEmpty(usersVM.UserId))
            {
                var existingUser = await _userService.GetAsync(x => x.Email.ToString().Equals(usersVM.Email.ToString()));
                if (existingUser != null && existingUser.Email.ToString() == usersVM.Email.ToString())
                {
                    message = "User with email id is already exist! Please use some other email id.";
                    return new ReturnResult { Message = message, Success = success };
                }
                ApplicationUser user = new ApplicationUser();
                string password = System.Web.Security.Membership.GeneratePassword(8, 0);
                user.OrganizationId = usersVM.OrganizationId;
                user.FullName = usersVM.FullName;
                user.Email = user.UserName = usersVM.Email;
                user.PhoneNumber = usersVM.ContactNumber;
                user.GenderId = usersVM.GenderId;
                user.Nationality = usersVM.Nationality;
                
                await _userService.InsertAsync(user, password, usersVM.RoleId);
                var proto = Request.GetRequestContext().Url.Request.RequestUri.Scheme;
                var baseUrl = Request.GetRequestContext().Url.Request.RequestUri.Authority;
                var callbackUrl = proto + "://" + baseUrl + "/Account/ConfirmEmail?activationCode=" + user.SecurityStamp + "&email=" + user.Email;

                string body = "Dear " + user.FullName + ", <br/>Your account has been created, click <a href=\"" + callbackUrl + "\">here</a> to activate the account.<br/>" +
                    "Use the below credentials after successfull activation <br/>UserName: " + user.Email + " <br/> " +
                    "Password: " + password + "<br/><br/>Regards,<br/>Administrator";

                string orgName = _genericService.Organization.GetById((int)usersVM.OrganizationId).CompanyName;


                string subject = "User in company <b>" + orgName + "<b> created successfully!";
                // Send email on account creation.
                EmailHelper.SendMail(user.Email, subject, body);
                message = "Save sucessfully!";
                success = true;
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
                    existingUser.Nationality = usersVM.Nationality;
                    await _userService.UpdateAsync(existingUser, usersVM.RoleId);
                    message = "Update sucessfully!";
                    success = true;
                }

            }
            _genericService.Commit();
            return new ReturnResult { Message = message, Success = success };
        }

        [Route("~/Api/User/DeleteUser")]
        [HttpPost]
        public async Task<ReturnResult> DeleteUser([FromBody] UsersVM usersVM)
        {
            if (usersVM != null)
            {
                var existingUser = await _userService.GetAsync(x => x.Id == usersVM.UserId);
                if (existingUser != null)
                {
                    //existingUser.IsActive = false;
                    await _userService.DeleteAsync(existingUser);
                    _genericService.Commit();
                    return new ReturnResult { Message = "Success", Success = true };
                }
            }
            return new ReturnResult { Message = "Failure", Success = false };
        }

        [Route("~/Api/User/ChangePassword")]
        [HttpPost]
        public async Task<ReturnResult> ChangePassword(ChangePasswordVM changePasswordVM)
        {
            if (changePasswordVM != null && !string.IsNullOrEmpty(changePasswordVM.Password))
            {
                string userName = HttpContext.Current.User.Identity.GetUserName();
                var user = await _userService.FindAsync(userName, changePasswordVM.Password);

                if (user != null)
                {
                    var passwordHash = new Microsoft.AspNet.Identity.PasswordHasher();
                    var hashedPassword = passwordHash.HashPassword(changePasswordVM.NewPassword);
                    user.PasswordHash = hashedPassword;
                    await _userService.UpdateAsync(user, string.Empty);
                    return new ReturnResult { Message = "Password changed successfully!!", Success = true };
                }
                return new ReturnResult { Message = "Current entered password is incorrect, please enter the correct password!!", Success = false };
            }
            return new ReturnResult { Message = "Current entered password cannot be empty", Success = false };
        }


        [Route("~/Api/Users/SaveTheme")]
        [HttpPost]
        public async Task<ReturnResult> SaveTheme([FromBody] UsersVM usersVM)
        {

            string userId = HttpContext.Current.User.Identity.GetUserId();
            {
                var existingUser = await _userService.GetAsync(x => x.Id == userId);
                if (existingUser != null)
                {

                    existingUser.ThemeName = usersVM.ThemeName;
                    await _userService.UpdateAsync(existingUser, usersVM.RoleId);
                }
            }
            _genericService.Commit();
            return new ReturnResult { Message = "Success", Success = true };
        }
    }

}
