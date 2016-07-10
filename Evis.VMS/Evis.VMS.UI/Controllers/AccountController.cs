/********************************************************************************
 * Company Name : East Vision Information System
 * Team Name    : EVIS IT Team
 * Author       : Vipin Raturi
 * Created On   : 06/05/2016
 * Description  : 
 *******************************************************************************/

using Evis.VMS.Business;
using Evis.VMS.Data.Model.Entities;
using Evis.VMS.UI.HelperClasses;
using Evis.VMS.UI.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebGrease.Css.Extensions;

namespace Evis.VMS.UI.Controllers
{
    public class AccountController : Controller
    {
        IAuthenticationManager _authenticationManager;
        readonly UserService _userService = null;

        public AccountController()
        {
            _userService = new UserService();
        }
        //
        // GET: /Account1/
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            return View("~/Views/Account/Login.cshtml");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginVM loginVM, string returnUrl)
        {
            if (ModelState.ContainsKey("Email"))
                ModelState["Email"].Errors.Clear();

            if (!ModelState.IsValid)
                return View(loginVM);

            var user = await _userService.FindAsync(loginVM.UserName, loginVM.Password);

            if (user == null)
            {
                ModelState.AddModelError("authstatusmessage", "Invalid credentials");
                return View(loginVM);
            }
            else if (user.IsActive)
            {
                await SignInAsync(AuthenticationManager, user, false);
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("authstatusmessage", "User is inactive");
                return View(loginVM);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> ResetPassword(string email, string activationId)
        {
            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(HttpUtility.UrlDecode(activationId)))
            {
                var user = await _userService.FindAsync(email, activationId);
                if (user != null)
                {
                    ResetPasswordVM resetPasswordVM = new ResetPasswordVM();
                    resetPasswordVM.UserId = user.Id;
                    return View(resetPasswordVM);
                }
            }
            return RedirectToAction("ForgotPassword", "");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ResetPassword(ResetPasswordVM resetPasswordVM)
        {
            if (!ModelState.IsValid)
                return View(resetPasswordVM);

            var user = await _userService.GetAsync(x => x.Id == resetPasswordVM.UserId);
            if (user != null)
            {
                var passwordHash = new Microsoft.AspNet.Identity.PasswordHasher();
                var hashedPassword = passwordHash.HashPassword(resetPasswordVM.NewPassword);
                user.PasswordHash = hashedPassword;
                user.IsActive = true;
                await _userService.UpdateAsync(user, string.Empty);
                return RedirectToAction("Login", "Account");
            }
            return null;
        }

        public async Task<ActionResult> ForgotPassword(LoginVM loginVM, string returnUrl)
        {
            if (ModelState.ContainsKey("Password"))
                ModelState["Password"].Errors.Clear();
            if (ModelState.ContainsKey("UserName"))
                ModelState["UserName"].Errors.Clear();

            if (!ModelState.IsValid)
                return View("~/Views/Account/Login.cshtml", loginVM);

            var user = await _userService.GetAsync(x => x.Email == loginVM.Email && x.IsActive == true);
            if (user == null)
            {
                ModelState.AddModelError("errormessage", "Invalid email address/user is inactive");
            }
            else
            {
                string password = System.Web.Security.Membership.GeneratePassword(8, 0);
                var passwordHash = new Microsoft.AspNet.Identity.PasswordHasher();
                var hashedPassword = passwordHash.HashPassword(password);
                user.PasswordHash = hashedPassword;
                user.IsActive = false;
                await _userService.UpdateAsync(user, string.Empty);

                var proto = Request.Url.Scheme;
                var baseUrl = Request.Url.Authority;
                var callbackUrl = proto + "://" + baseUrl +"/Account/ResetPassword?email=" + user.Email + "&activationId=" + HttpUtility.UrlEncode(password);

                string body = "Dear " + user.FullName + ", <br/>Your password has been reset, click <a href=\"" + callbackUrl + "\">here</a> to reset the password.<br/>" +
                    "<br/><br/>Regards,<br/>Administrator";
                // Send email on account creation.
                //EmailHelper.SendMail(user.Email, "Reset Password", body);

                ModelState.AddModelError("errormessage", "Log in using the password which is to your email address");
            }
            return View("Login");
        }



        public async Task<ActionResult> ConfirmEmail(string activationCode, string email)
        {
            if (!string.IsNullOrEmpty(activationCode))
            {
                var user = await _userService.GetAsync(x => x.Email == email && x.SecurityStamp == activationCode);
                if (user != null)
                {
                    user.IsActive = true;
                    user.SecurityStamp = Guid.NewGuid().ToString();
                    await _userService.UpdateAsync(user, string.Empty);
                    var proto = Request.Url.Scheme;
                    var baseUrl = Request.Url.Authority;
                    var callbackUrl = proto + "://" + baseUrl + "/Account/Login";
                    ViewBag.Message = "<p>User is activated please click <a href=\"" + callbackUrl + "\">here</a> to login.</p>";
                    return View();
                }
                ViewBag.Message = "<p>This activation code is already used/invalid!</p>";
                return View();
            }
            ViewBag.Message = "<p>Problem in activating user, please try again or contact to admin.</p>";
            return View();
        }


        [Authorize]
        public async Task<ActionResult> ChangePassword(ChangePasswordVM changePasswordVM)
        {
            if (!ModelState.IsValid)
            {
                return View(changePasswordVM);
            }

            string userName = "";
            var user = await _userService.FindAsync(userName, changePasswordVM.Password);

            if (user != null)
            {
                var passwordHash = new Microsoft.AspNet.Identity.PasswordHasher();
                var hashedPassword = passwordHash.HashPassword(changePasswordVM.Password);
                user.PasswordHash = hashedPassword;
                await _userService.UpdateAsync(user, string.Empty);
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("errormessage", "Current entered password is invalid");
            return View(changePasswordVM);
        }

        public ActionResult LogOff()
        {
            SignOut(AuthenticationManager);

            Request.Cookies.AllKeys.ForEach(x =>
            {
                var httpCookie = Response.Cookies[x];
                if (httpCookie != null) httpCookie.Expires = DateTime.Now.AddDays(-1);
            });

            return RedirectToActionPermanent("Login");
        }

        private async Task SignInAsync(IAuthenticationManager authenticationManager, ApplicationUser user, bool isPersistent)
        {
            if (authenticationManager != null)
            {
                _authenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                SignOut(authenticationManager);

                var identity = await _userService.SignInAsync(user);
                if (identity != null)
                {
                    var roleId = user.Roles.FirstOrDefault(x => x.UserId.Equals(user.Id));
                    identity.AddClaim(new Claim(ClaimTypes.Role, roleId == null ? "0" : roleId.ToString()));
                    identity.AddClaim(new Claim(ClaimTypes.Sid, user.Id));
                    identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
                }
                _authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, identity);
            }

        }

        public static void SignOut(IAuthenticationManager authenticationManager)
        {
            authenticationManager.SignOut();
        }

        public IAuthenticationManager AuthenticationManager
        {
            get { return _authenticationManager ?? (_authenticationManager = HttpContext.GetOwinContext().Authentication); }
            set { _authenticationManager = value; }

        }

    }

}