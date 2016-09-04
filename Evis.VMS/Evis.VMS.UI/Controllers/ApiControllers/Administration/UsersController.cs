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
            var user = (await _userService.GetAllAsync()).Where(x => x.Id == HttpContext.Current.User.Identity.GetUserId() && x.IsActive == true).FirstOrDefault();

            ApplicationRole role = null;
            if (user != null)
            {
                role = await _applicationRoleService.FindByIdAsync(user.Roles.FirstOrDefault().RoleId);
            }

            if (role != null && role.Name == "Supervisor")
            {
                return (await _applicationRoleService.GetAllAsync()).Where(x => x.Name.ToLower().Equals("security")).AsQueryable();
            }
            else
            {
                return (await _applicationRoleService.GetAllAsync()).Where(x => x.Name.ToLower() != "superadmin").AsQueryable();
            }
        }


        [Route("~/Api/User/GetAllOrganizations")]
        [HttpGet]
        public async Task<IEnumerable<GeneralDropDownVM>> GetAllOrganizations()
        {
            var user = (await _userService.GetAllAsync()).Where(x => x.Id == HttpContext.Current.User.Identity.GetUserId() && x.IsActive == true).FirstOrDefault();
            int orgId = (user == null) ? 0 : (int)user.OrganizationId;

            IQueryable<GeneralDropDownVM> organizations;


            organizations = _genericService.Organization.GetAll()
                                .Where(x => x.IsActive == true && (orgId == 0 || x.Id == orgId))
                                .Select(x => new GeneralDropDownVM { Id = x.Id, Name = x.CompanyName });

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
            var user = (await _userService.GetAllAsync()).Where(x => x.Id == HttpContext.Current.User.Identity.GetUserId() && x.IsActive == true).FirstOrDefault();

            var getUsers = (await _userService.GetAllAsync()).Where(x => x.Organization.IsActive == true &&
                            (user == null || (user != null && x.OrganizationId == user.OrganizationId
                            && x.Id != HttpContext.Current.User.Identity.GetUserId()))).AsQueryable();

            if (string.IsNullOrEmpty(sortField))
            {
                sortField = "CreatedOn";
                sortOrder = "DESC";
            }

            ApplicationRole role = null;
            if (user != null)
            {
                role = await _applicationRoleService.FindByIdAsync(user.Roles.FirstOrDefault().RoleId);
            }

            IQueryable<ApplicationRole> getRoles;
            if (role != null && role.Name == "Supervisor")
            {
                getRoles = (await _applicationRoleService.GetAllAsync()).Where(x => x.Name == "Security").AsQueryable();
            }
            else
            {
                getRoles = (await _applicationRoleService.GetAllAsync()).AsQueryable();
            }

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
                            Nationality = users.Nationality,
                            ProfilePicturePath = users.ProfilePicturePath,
                            CreatedOn = users.CreatedOn,
                            CreatedBy = users.CreatedBy,
                            UpdatedBy = users.UpdatedBy,
                            UpdatedOn = users.UpdatedOn
                        }).AsQueryable();

            if (temp.Count() > 0)
            {
                if (!string.IsNullOrEmpty(globalSearch))
                {
                    temp = temp.Where(item =>
                        item.ContactNumber.ToLower().Contains(globalSearch.ToLower()) ||
                        item.FullName.ToLower().Contains(globalSearch.ToLower()) ||
                        item.Email.ToLower().Contains(globalSearch.ToLower()) ||
                        item.RoleName.ToLower().Contains(globalSearch.ToLower()))
                        .AsQueryable();
                }

                //creating pager object to send for filtering and sorting
                var paginationRequest = new PaginationRequest
                {
                    PageIndex = (pageIndex - 1),
                    PageSize = pageSize,
                    SearchText = globalSearch,
                    Sort = new Sort { SortDirection = (sortOrder == "ASC" ? SortDirection.Ascending : SortDirection.Descending), SortBy = sortField }
                };

                int totalCount = 0;

                IList<UsersVM> result =
                    GenericSorterPager.GetSortedPagedList<UsersVM>(temp, paginationRequest, out totalCount);

                var jsonData = JsonConvert.SerializeObject(result);
                return JsonConvert.SerializeObject(new { totalRows = totalCount, result = jsonData });
            }

            return null;
        }

        [Route("~/Api/Users/SaveUser")]
        [HttpPost]
        public async Task<ReturnResult> SaveUser([FromBody] UsersVM usersVM)
        {

            string message = "Error occured, please try again with valid entered data again!";
            bool success = false;
            var currentUserId = HttpContext.Current.User.Identity.GetUserId();


            if (usersVM != null && string.IsNullOrEmpty(usersVM.UserId))
            {
                var existingUser = await _userService.GetAsync(x => x.Email.ToString().Equals(usersVM.Email.ToString()));
                if (existingUser != null && existingUser.Email.ToString() == usersVM.Email.ToString())
                {
                    message = "User with email id is already exist! Please use some other email id.";
                    return new ReturnResult { Message = message, Success = success };
                }
                ApplicationUser user = new ApplicationUser();
                string password = "Admin@123"; //System.Web.Security.Membership.GeneratePassword(8, 0);
                user.OrganizationId = usersVM.OrganizationId;
                user.FullName = usersVM.FullName;
                user.Email = user.UserName = usersVM.Email;
                user.PhoneNumber = usersVM.ContactNumber;
                user.GenderId = usersVM.GenderId;
                user.Nationality = usersVM.Nationality;
                user.CreatedOn = DateTime.UtcNow;
                user.CreatedBy = currentUserId;
                user.UpdatedOn = DateTime.UtcNow;
                user.UpdatedBy = currentUserId;
                user.IsActive = true;

                if ((string.IsNullOrEmpty(usersVM.ProfilePicturePath) || usersVM.ProfilePicturePath == "VisitorImage"))
                {
                    user.ProfilePicturePath = string.Format("/images/UserImages/{0}", usersVM.ProfilePicturePath);  
                }

                await _userService.InsertAsync(user, password, usersVM.RoleId);
                var proto = Request.GetRequestContext().Url.Request.RequestUri.Scheme;
                var baseUrl = Request.GetRequestContext().Url.Request.RequestUri.Authority;
                var callbackUrl = proto + "://" + baseUrl + "/Account/ConfirmEmail?activationCode=" + user.SecurityStamp + "&email=" + user.Email;

                var emailFormat = _genericService.EmailFormats.GetAll().Where(x => x.Category == "UserCreation").FirstOrDefault();
                string body = string.Format(emailFormat.Format, user.FullName, callbackUrl, user.Email, password);

                //string body = "Dear " + user.FullName + ", <br/>Your account has been created, click <a href=\"" + callbackUrl + "\">here</a> to activate the account.<br/>" +
                //    "Use the below credentials after successfull activation <br/>UserName: " + user.Email + " <br/> " +
                //    "Password: " + password + "<br/><br/>Regards,<br/>Administrator";

                string orgName = _genericService.Organization.GetById((int)usersVM.OrganizationId).CompanyName;


                string subject = "User in company " + orgName + " created successfully!";
                // Send email on account creation.
                EmailHelper.SendMail(user.Email, subject, body);
                message = "User saved sucessfully!";
                success = true;
            }
            else
            {
                var emailExist = await _userService.GetAsync(x => x.Email.ToLower() == usersVM.Email.ToLower());
                var existingUser = await _userService.GetAsync(x => x.Id == usersVM.UserId);

                if (emailExist != null && existingUser != null && emailExist.Id != existingUser.Id)
                {
                    message = "User with email id is already exist! Please use some other email id.";
                    return new ReturnResult { Message = message, Success = success };
                }

                else if ((emailExist != null && existingUser != null && emailExist.Id == existingUser.Id) || existingUser != null)
                {
                    existingUser.OrganizationId = usersVM.OrganizationId;
                    existingUser.FullName = usersVM.FullName;
                    existingUser.Email = existingUser.UserName = usersVM.Email;
                    existingUser.PhoneNumber = usersVM.ContactNumber;
                    existingUser.GenderId = usersVM.GenderId;
                    existingUser.Nationality = usersVM.Nationality;
                    existingUser.UpdatedOn = DateTime.UtcNow;
                    existingUser.UpdatedBy = currentUserId;

                    if (!string.IsNullOrEmpty(usersVM.ProfilePicturePath))
                    {
                        existingUser.ProfilePicturePath = string.Format("/images/UserImages/{0}", usersVM.ProfilePicturePath);
                    }
                   
                    await _userService.UpdateAsync(existingUser, usersVM.RoleId);
                    message = "User update sucessfully!";
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
                    if (_genericService.ShitfAssignment.SearchFor(x => x.UserId == usersVM.UserId && x.IsActive == true).Any())
                    {
                        return new ReturnResult { Message = "Please first delete all the shift assigment under this user", Success = false };
                    }
                    //existingUser.IsActive = false;
                    await _userService.DeleteAsync(existingUser);
                    _genericService.Commit();
                    return new ReturnResult { Message = "User is deleted successfully!", Success = true };
                }
            }
            return new ReturnResult { Message = "User is not deleted, please try again!", Success = false };
        }

        [Route("~/Api/User/ChangePassword")]
        [HttpPost]
        public async Task<ReturnResult> ChangePassword(ChangePasswordVM changePasswordVM)
        {
            if (changePasswordVM != null && !string.IsNullOrEmpty(changePasswordVM.Password))
            {
                string userName = HttpContext.Current.User.Identity.GetUserName();
                var user = await _userService.FindAsync(userName, changePasswordVM.Password);
                var passwordExist = await _userService.FindAsync(userName, changePasswordVM.NewPassword);

                if (user != null && passwordExist == null)
                {
                    var passwordHash = new Microsoft.AspNet.Identity.PasswordHasher();
                    var hashedPassword = passwordHash.HashPassword(changePasswordVM.NewPassword);
                    user.PasswordHash = hashedPassword;
                    await _userService.UpdateAsync(user, string.Empty);
                    return new ReturnResult { Message = "Password changed successfully!", Success = true };
                }
                else if (passwordExist != null)
                {
                    return new ReturnResult { Message = "New entered password should not be same as the current password!", Success = false };
                }
                return new ReturnResult { Message = "Current entered password is incorrect, please enter the correct password!", Success = false };
            }
            return new ReturnResult { Message = "Current entered password cannot be empty!", Success = false };
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
